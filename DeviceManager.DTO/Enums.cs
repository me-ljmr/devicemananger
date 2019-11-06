using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.DTO
{
    
    public enum LogModeEnum
    {
        CheckIn=0,
        CheckOut=1
    }
    public enum VerifyModeEnum { 
        FingerPrint = 1,
        Card = 2
    }

    public class SingleShiftExtractionModes {
        public static string FirstInLastOut = "FILO";
        public static string FirstInFirstOut = "FIFO";
        public static string UseInOutModesFromDevice= "DEVICE";
    }
    public class MultiShiftExtractionModes {
        public static string UseModesFromDevice = "DEVICE";
    }
}
