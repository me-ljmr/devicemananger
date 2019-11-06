using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZkemConnector.NET
{
    public class LogData
    {
        public string EnrollNumber { get; set; }
        public int IsInValid { get; set; }
        public int AttState { get; set; } 
        public int VerifyMethod { get; set; }
        public DateTime Tdate { get; set; }
    }
}
