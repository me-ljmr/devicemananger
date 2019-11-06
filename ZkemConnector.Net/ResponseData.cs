using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkemConnector.NET
{
    public class ResponseData<T>
    {
        public bool Error { get; set; }
        public List<T> Data { get; set; }
        public string[] Messages { get; set; }
    }
    public class DeviceDTO {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public string PortNo { get; set; }
        public int ConnectionMode { get; set; }
        public int MachineNumber { get; set; }
    }
    
     
}
