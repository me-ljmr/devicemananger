using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviceManager.DataEntity.Models
{
    [Table("Tbl_DeviceAttenLog", Schema = "dbo")]
    public class RawLog
    {
        [Required, Key]
        public Int32 LogId { get; set; }
        public Int32 Emp_ID { get; set; }
        public Int32 VerifyMode { get; set; }
        public Int32 InOutMode { get; set; }
        public DateTime LogDate { get; set; }
        [MaxLength(20)]
        public String LogTime { get; set; }
        public Int32 ExtractedFrom { get; set; }
        public Int32 indx { get; set; }
        public Int32 mstindx { get; set; }
        public String workcode { get; set; }
    }

}
