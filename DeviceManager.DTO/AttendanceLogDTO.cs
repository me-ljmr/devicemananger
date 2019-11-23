using System;

namespace DeviceManager.DTO
{
    public class AttendanceLogDTO
    {
        public int ID { get; set; }
        public int EmployeeId { get; set; }
        public int InVerifyMode { get; set; }
        public int OutVerifyMode { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime? LogOutDate { get; set; }
        public string LogTime { get; set; }
        public string LogOutTime { get; set; }
        public int InMode { get; set; }
        public int OutMode { get; set; }
        public int InDeviceId { get; set; }
        public int OutDeviceId { get; set; }
    }
}
