using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DeviceManager.DataEntity.Models
{
    [Table("Tbl_DeviceMaster", Schema ="dbo")]
    public class DeviceMaster
    {
        [Required, Key]
        public Int32 DeviceID { get; set; }
        [MaxLength(4)]
        public String ComNo { get; set; }
        [MaxLength(255)]
        public String DeviceDescription { get; set; }
        [MaxLength(10)]
        public String PortNo { get; set; }
        [MaxLength(18)]
        public String IPAddress { get; set; }
        [MaxLength(10)]
        public String BaudRate { get; set; }
        public Int32 ConMode { get; set; }
        public Int32 log_mode { get; set; }
        public Int32 machnumber { get; set; }
        public String CommKey { get; set; }
        [MaxLength(50)]
        public String dproductcode { get; set; }
        [MaxLength(50)]
        public String dplatform { get; set; }
        [MaxLength(50)]
        public String dfversion { get; set; }
    }
    

     
}
