using System.Collections.Generic;

namespace ZkemConnector.NET
{
    public class ResponseData<T>
    {
        public bool Error { get; set; }
        public List<T> Data { get; set; }
        public string[] Messages { get; set; }
    }
    public class DeviceDTO
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public string PortNo { get; set; }
        public int ConnectionMode { get; set; }
        public int MachineNumber { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }
    }


}
