using System;

namespace DeviceManager.DTO
{
    public class DeviceDTO
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public string PortNo { get; set; }
        public int ConnectionMode { get; set; }  
        public int MachineNumber { get; set; }

    }
}
