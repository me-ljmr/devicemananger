using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
 
namespace ZkemConnector.NET
{
    public delegate void CallBackFunction(int deviceId, string status, ListViewItem litem);
    public partial class DeviceManager : Form
    {
        Thread _thread;
        bool m_IsAlive;
        MethodInvoker UIInvoke;
        private IRestClient client = null;
        private List<DeviceDTO> devices = new List<DeviceDTO>();

        private string apiEndPoint = string.Empty;
        private string token = string.Empty;
        private ImageList imgList = new ImageList(); 
        private Dictionary<int, ZKTecoDevice> zkDevices = null;

        private CallBackFunction deviceStatusChangeCallBack;
        public DeviceManager()
        {
            InitializeComponent();
            apiEndPoint = ConfigurationManager.AppSettings.Get("apiendpoint");
            token = ConfigurationManager.AppSettings.Get("token");
            client = new RestClient($"{apiEndPoint}");
            imgList.Images.Add("redicon", Image.FromFile($"{Environment.CurrentDirectory}/icons/24/red.png"));
            imgList.Images.Add("greenicon", Image.FromFile($"{Environment.CurrentDirectory}/icons/24/green.png"));
            imgList.Images.Add("yellowicon", Image.FromFile($"{Environment.CurrentDirectory}/icons/24/yellow.png"));
            lvDevices.SmallImageList = imgList;

            zkDevices = new Dictionary<int, ZKTecoDevice>();
            deviceStatusChangeCallBack = new CallBackFunction(DeviceStatusChanged);


        }

        private void DeviceStatusChanged(int deviceId, string status, ListViewItem litem)
        {
            Invoke(new MethodInvoker(()=> {
                litem.SubItems[1].Text = status;

                if (status == Constants.DeviceStatus.Connected)
                    litem.ImageKey = "greenicon";
                else if (status == Constants.DeviceStatus.NotInNetwork || status == Constants.DeviceStatus.CannotConnect)
                    litem.ImageKey = "redicon";
                else if (status == Constants.DeviceStatus.Active)
                    litem.ImageKey = "yellowicon";
            }));
            
        }
        private void buttonGetAndConnect_Click(object sender, EventArgs e)
        {
            NetworkMonitor.Instance.StartMonitor();
            m_IsAlive = true;
            UIInvoke = new MethodInvoker(UpdateUI); 
            GetDevicesFromAPIAsync();
             
            _thread = new Thread(new ThreadStart(UpdateStatus));
            _thread.Start();
            buttonStop.Enabled = true;
            buttonGetAndConnect.Enabled = false;

        }

        private void DeviceManager_Load(object sender, EventArgs e)
        {
            buttonStop.Enabled = false;
            buttonGetAndConnect.Enabled = true;

        }
        void UpdateStatus()
        {

            while (m_IsAlive)
            {
                try
                {
                    Invoke(UIInvoke);
                }
                catch { }
                Thread.Sleep(200);
            }
        }
        private IRestRequest PrepareRequest(string resource, IEnumerable<Parameter> parameters = null)
        {
            var request = new RestRequest(resource, Method.POST);
            if (parameters != null)
            {
                request.Parameters.AddRange(parameters);
            }
            request.AddHeader("token", token);

            return request;
        }
         void UpdateUI()
        {
            var itemList = lvDevices.Items;
            foreach (ListViewItem litem in itemList)
            {
                    var device = (DeviceDTO)litem.Tag;
                    string newStatus = NetworkMonitor.Instance.DeviceList.Where(d => d.DeviceID == device.DeviceID).Select(s => s.Status).FirstOrDefault();
                    string oldStatus = litem.SubItems[1].Text;


                    if (oldStatus != newStatus)
                    {
                        if (!(oldStatus == Constants.DeviceStatus.Connected && newStatus == Constants.DeviceStatus.Active))
                        {
                            deviceStatusChangeCallBack(device.DeviceID, newStatus, litem);
                             
                        }
                        if (newStatus == Constants.DeviceStatus.NotInNetwork)
                        {
                            zkDevices[device.DeviceID].Disconnect();
                            deviceStatusChangeCallBack(device.DeviceID, newStatus, litem); 
                            AddLog($"Disconnected : {device.DeviceName}/{device.IPAddress}. Please check network connection.");
                        }
                    }
                    if (newStatus == Constants.DeviceStatus.Active && oldStatus != Constants.DeviceStatus.Connected)
                    {
                          CreateConnector(litem, (DeviceDTO)litem.Tag);
                     }

            }
        }
        void CreateConnector(ListViewItem litem, DeviceDTO device)
        {
            zkDevices[device.DeviceID].Disconnect();
            zkDevices[device.DeviceID] = new ZKTecoDevice(RaiseDeviceEvent) { DeviceID = device.DeviceID };

            if (zkDevices[device.DeviceID].Connect_Net(device.IPAddress, int.Parse(device.PortNo)))
            {
                AddLog($"Device online: {DeviceManipulator.Instance.FetchDeviceInfo(zkDevices[device.DeviceID], device.MachineNumber)}");
                deviceStatusChangeCallBack(device.DeviceID, Constants.DeviceStatus.Connected, litem);
            }
            else
            {
                deviceStatusChangeCallBack(device.DeviceID, Constants.DeviceStatus.CannotConnect, litem); 

            }

             
        }

        private void RaiseDeviceEvent(object sender, string actionType, int deviceId)
        {
            var device = devices.Where(d => d.DeviceID == deviceId).FirstOrDefault();
            if (device == null) device = new DeviceDTO();
            switch (actionType)
            {
                case DeviceUtility.acx_Connect:

                    AddLog($"Connected : {device.DeviceName}/{device.IPAddress}");


                    break;
                case DeviceUtility.acx_Disconnect:

                    AddLog($"Disconnected : {device.DeviceName}/{device.IPAddress}");
                    break;

                case DeviceUtility.acx_Transaction:

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
                    AddLog($"Activity Logged: User {a.EnrollNumber}, Date {a.Tdate}, Type {  ((VerifyModeEnum)a.VerifyMethod).ToString()} | Device: {device.DeviceName}/{device.IPAddress}");
                    break;
                default:
                    break;
            }
        }
        async Task GetDevicesFromAPIAsync()
        {
            try
            {
                lvDevices.Items.Clear();
                zkDevices.Clear();
                ListViewItem litem;
                client.ExecuteAsync(PrepareRequest(Constants.GetAllDevices), async response =>
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var returnObj = JsonConvert.DeserializeObject<ResponseData<DeviceDTO>>(response.Content);
                        if (!returnObj.Error)
                        {
                            devices = returnObj.Data;
                            devices.ForEach(s => s.Status = Constants.DeviceStatus.CannotConnect);
                            NetworkMonitor.Instance.DeviceList = devices;
                            List<Task<DeviceDTO>> tasks = new List<Task<DeviceDTO>>();
                            devices.ForEach(device =>
                            {
                                tasks.Add(Task.Run(() =>
                                {
                                    Invoke(new MethodInvoker(() =>
                                    {
                                        zkDevices.Add(device.DeviceID, new ZKTecoDevice(RaiseDeviceEvent) { DeviceID = device.DeviceID });
                                    }));
                                    return device;
                                }));

                            });

                            var data = await Task.WhenAll(tasks);

                            foreach (var d in data)
                            {
                                 
                                litem = new ListViewItem();
                                litem.Tag = d;
                                litem.Text = d.DeviceName; 
                                litem.ImageKey = "redicon";
                                litem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = d.Status });
                                Invoke(new MethodInvoker(() =>
                                {
                                    lvDevices.Items.Add(litem); 
                                }));

                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void listView1_BeforeLabelEdit(object sender, LabelEditEventArgs e)
        {
            e.CancelEdit = true;
        }

        private void DeviceManager_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

            NetworkMonitor.Instance.Destroy();
            zkDevices.ToList().ForEach(z => z.Value.Disconnect());
            zkDevices.Clear();
            m_IsAlive = false;
            lvDevices.Items.Clear();
            buttonGetAndConnect.Enabled = true;
            buttonStop.Enabled = false;
        }

        private void DeviceManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsAlive)
            {
                e.Cancel = true;
                AddLog("Application cannot be closed when processor is running.", "WARNING");
            }
        }
        private void AddLog(string text, string type = "INFO")
        {
            ListViewItem litem;
            this.Invoke(new MethodInvoker(delegate ()
            {
                litem = logList.Items.Add($"{DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss")} - {text}");
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

        private void logList_SelectedIndexChanged(object sender, EventArgs e)
        {
            detailText.Text = logList.SelectedItems.Count > 0 ? logList.SelectedItems[0].Text : "";
        }

        private void buttonClearLogs_Click(object sender, EventArgs e)
        {
            logList.Items.Clear();
        }
    }
}
