using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;

namespace ZkemConnector.NET
{
    public partial class DeviceLogForm : Form
    {
        private ZKTecoDevice  zkteco = null;
        private List<ZKTecoDevice> zkdevices = null;
        private ZKTecoDevice[] zkDevices = new ZKTecoDevice[] { };
        private List<DeviceDTO> devices = new List<DeviceDTO>();
        private List<Tuple<int, Thread>> deviceThreads = new List<Tuple<int, Thread>>();
        private string apiEndPoint = string.Empty;
        private string token = string.Empty;
        private IRestClient client = null;
        private bool start;

 

        private bool Start
        {
            get {
                return start;
            }
            set {
                start = value;
                buttonGetAndConnect.Enabled = !value;
                buttonStop.Enabled = value; 
            } 
        }
        public DeviceLogForm()
        {

            InitializeComponent();
            Start = false;
            apiEndPoint = ConfigurationManager.AppSettings.Get("apiendpoint");
            token = ConfigurationManager.AppSettings.Get("token");
            client = new RestClient($"{apiEndPoint}");
            zkteco = new ZKTecoDevice(RaiseDeviceEvent);
            bw_deviceManager.WorkerReportsProgress = true;
            bw_deviceManager.WorkerSupportsCancellation = true;
            
        }
        private IRestRequest PrepareRequest(string resource, IEnumerable<Parameter> parameters=null) {
            var request = new RestRequest(resource, Method.POST);
            if (parameters != null) {
                request.Parameters.AddRange(parameters);
            }
            request.AddHeader("token", token);
            
            return request;
        }
        private void DeviceLogForm_Load(object sender, EventArgs e)
        { 
             
        }
        private void AddLog(string text, string type="INFO") {
            ListViewItem litem;
            Invoke(new MethodInvoker(delegate ()
            {
                litem = logList.Items.Add(text);
                if (type == "SUCCESS")
                {
                    litem.ForeColor = Color.Green;
                }
                else if (type == "WARNING")
                {
                    litem.ForeColor = Color.Yellow;
                }
                else if (type == "ERROR")
                {
                    litem.BackColor = Color.Yellow;
                    litem.ForeColor = Color.Red;

                }
                litem.EnsureVisible();
            }));
            
            
        }
        private void LoadDevices(DeviceDTO d) {
            if (zkdevices == null) zkdevices = new List<ZKTecoDevice>();
            if (!zkdevices.Any(a => a.DeviceID == d.DeviceID))
            {
                zkteco = new ZKTecoDevice(RaiseDeviceEvent);
                zkteco.DeviceStatus = false;
                zkteco.DeviceID = d.DeviceID;
                zkdevices.Add(zkteco); 
            } 

        }
        private void RaiseDeviceEvent(object sender, string actionType, int deviceId)
        {
            switch (actionType)
            {
                case DeviceUtility.acx_Connect:
                    AddLog("Connected : Device");
                    break;
                case DeviceUtility.acx_Disconnect:

                    AddLog("Disconnected : Device");
                    break;

                case DeviceUtility.acx_Transaction:
                    /*
                                 int      EmployeeId    { get; set; }
                                 int      VerifyMode    { get; set; }
                                 int      InOutMode     { get; set; }
                                 DateTime LogDate       { get; set; }
                                 string   LogTime       { get; set; }
                                 int      DeviceID      { get; set; }
                     
                     */
                    LogData a = (LogData)sender;
                    IRestResponse response = client.Execute(PrepareRequest(Constants.SaveDeviceLog, new List<Parameter>() {
                        new Parameter("EmployeeId",a.EnrollNumber,ParameterType.RequestBody),
                        new Parameter("VerifyMode",a.VerifyMethod,ParameterType.RequestBody),
                        new Parameter("InOutMode",a.AttState,ParameterType.RequestBody),
                        new Parameter("LogDate",a.Tdate.Date,ParameterType.RequestBody),
                        new Parameter("LogTime",a.Tdate.ToShortTimeString(),ParameterType.RequestBody),
                        new Parameter("DeviceID",1,ParameterType.RequestBody),

                    }));
                    var result = response.Content;
                    AddLog($"Activity Logged: User {a.EnrollNumber}, Date {a.Tdate}, Type {  ((VerifyModeEnum)a.VerifyMethod).ToString()}");
                    break;
                default:
                    break;
            }
        }
        private void buttonGetAndConnect_Click(object sender, EventArgs e)
        {
            Start = true;
            //RefreshDeviceConnections(); 
            bw_deviceManager.RunWorkerAsync();
        }
        private void CheckConnection(DeviceDTO d) {

            var dev = zkDevices.Where(x => x.DeviceID == d.DeviceID).FirstOrDefault() ;
                 
            //zkteco = zkdevices.Where(z => z.DeviceID == d.DeviceID).FirstOrDefault();
            if (!DeviceUtility.PingTheDevice(d.IPAddress))
            {
                AddLog($"Device not active - {d.DeviceName} - {d.IPAddress}. Please check network connection to with the device.", "ERROR");

                dev.DeviceStatus = false;
            }
            
            if (DeviceUtility.PingTheDevice(d.IPAddress))
            {
                if (dev.Connect_Net(d.IPAddress, int.Parse(d.PortNo)))
                {
                    AddLog($"New device - {d.DeviceName} - Connected", "SUCCESS");
                    dev.DeviceStatus = true;
                }
            }  
            
        }

        private void LoadDevicesFromAPI() {
            IRestResponse response = client.Execute(PrepareRequest(Constants.GetAllDevices));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var returnObj = JsonConvert.DeserializeObject<ResponseData<DeviceDTO>>(response.Content);
                if (!returnObj.Error)
                {
                    devices = returnObj.Data; 
                    zkDevices = new ZKTecoDevice[devices.Count];
                    if (devices != null)
                    {
                        int cnt = 0;
                        devices.ForEach(x =>
                        {
                            AddLog($"Loading Device - {x.DeviceName} - {x.IPAddress}:{x.PortNo}");
                            //LoadDevices(x);
                            zkDevices[cnt] = new ZKTecoDevice((sender, actionType,deviceId)=> {
                                if (actionType == DeviceUtility.acx_Transaction) {
                                    AddLog($"Card Swiped");
                                }
                            });
                            zkDevices[cnt].DeviceStatus = false;
                            zkDevices[cnt].DeviceID = x.DeviceID;
                            cnt++;
                        });
                    }
                }
            }
        }
        private void RefreshDeviceConnections() {
           
            IRestResponse response = client.Execute(PrepareRequest(Constants.GetAllDevices));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var returnObj = JsonConvert.DeserializeObject<ResponseData<DeviceDTO>>(response.Content);
                if (!returnObj.Error)
                {
                    devices = returnObj.Data;
                    if (devices != null)
                    {
                        devices.ForEach(x =>
                        {
                            AddLog($"Device - {x.DeviceName} - {x.IPAddress}:{x.PortNo}");
                            LoadDevices(x);
                            CheckConnection(x);
                            
                        });
                         
                    }
                    else
                    {
                        AddLog("No device lists found", "ERROR");
                    }
                }
                else
                {
                    AddLog("Error loading devices:", "ERROR");
                    foreach (var a in (string[])returnObj.Messages)
                    {
                        AddLog(a, "ERROR");
                    }
                }
            }
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (bw_deviceManager.IsBusy) bw_deviceManager.CancelAsync();
            Start = false;
            deviceThreads.Clear();
            devices.Clear();
           
        }

        private void logList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(logList.SelectedIndices.Count>0)
            detailText.Text = logList.SelectedItems[0].Text;
        }
        private void CheckConnections() {
            foreach (var device in devices) {
                CheckConnection(device);
            }
        }
        private void bw_deviceManager_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!e.Cancel) { 
                LoadDevicesFromAPI();

                CheckConnections();
            }
        }

        private void bw_deviceManager_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            logList.Items.Clear();
        }
    }
}
