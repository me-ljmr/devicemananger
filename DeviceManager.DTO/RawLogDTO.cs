using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.DTO
{
    public class RawLogDTO
    { 
        public int EmployeeId { get; set; }
        public int VerifyMode { get; set; }
        public int InOutMode { get; set; }
        public DateTime LogDate { get; set; }
        public string LogTime { get; set; }
        public int DeviceID { get; set; }
    }
}
