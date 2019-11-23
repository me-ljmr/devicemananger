using System.Drawing;

namespace ZkemConnector.NET
{
    public class Constants
    {
        // resources
        public static string GetAllDevices = "api/devices/all";
        public static string SaveDeviceLog = "api/devicelog/save";
        public class Status
        {
            public string Code { get; set; }
            public Color Color { get; set; }
            public string StatusText { get; set; }

        }
        public static class DeviceStatus
        {
            public static string Connected = "CONNECTED";
            public static string Active = "ACTIVE";
            public static string NotInNetwork = "NOT_IN_NETWORK";
            public static string CannotConnect = "DISCONNECTED";
            public static string NotBiometric = "NOT_BIOMETRIC_DEVICE";
        }

    }
}
