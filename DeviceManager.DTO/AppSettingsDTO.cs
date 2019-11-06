using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.DTO
{
    public class AppSettingsDTO
    {
        public string ConnectionString { get; set; }
        public string[] VerifyMode { get; set; }
        public string[] LogMode { get;set; }
        public string DataExtractionMode { get; set; }
        public string Token { get; set; }
    }
}
