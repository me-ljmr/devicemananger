using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DeviceManager.DataEntity.Models
{
    [Table("tbl_emp_attn_log", Schema = "dbo")]
    public class AttendanceLog
    {
        [Required, Key]
        public Int32 AttnLogId { get; set; }
        public Int32 EMP_ID { get; set; }
        public String WORKHOUR_ID { get; set; }
        public DateTime TDATE { get; set; }
        public DateTime? TDATE_OUT { get; set; }
        [MaxLength(15)]
        public String INTIME { get; set; }
        [MaxLength(40)]
        public String INMODE { get; set; }
        [MaxLength(510)]
        public String INREMARKS { get; set; }
        [MaxLength(15)]
        public String OUTTIME { get; set; }
        [MaxLength(40)]
        public String OUTMODE { get; set; }
        [MaxLength(510)]
        public String OUTREMARKS { get; set; }
        public String DT_LIN { get; set; }
        [MaxLength(15)]
        public String LUNCHIN { get; set; }
        [MaxLength(40)]
        public String LUNCHINMODE { get; set; }
        [MaxLength(100)]
        public String LUNCHINREMARKS { get; set; }
        public String DT_LOUT { get; set; }
        [MaxLength(15)]
        public String LUNCHOUT { get; set; }
        [MaxLength(40)]
        public String LUNCHOUTMODE { get; set; }
        [MaxLength(100)]
        public String LUNCHOUTREMARKS { get; set; }
        public String DT_TIN { get; set; }
        [MaxLength(15)]
        public String TIFFININ { get; set; }
        [MaxLength(40)]
        public String TIFFININMODE { get; set; }
        [MaxLength(100)]
        public String TIFFININREMARKS { get; set; }
        public String DT_TOUT { get; set; }
        [MaxLength(15)]
        public String TIFFINOUT { get; set; }
        [MaxLength(40)]
        public String TIFFINOUTMODE { get; set; }
        [MaxLength(100)]
        public String TIFFINOUTREMARKS { get; set; }
        public String ATT_STATUS { get; set; }
        [MaxLength(20)]
        public String OUT_VNO { get; set; }
        public String IS_HALTED { get; set; }
        public Boolean FLGIN { get; set; }
        public Boolean FLGOUT { get; set; }
        public Int32 REMARK_FLAG { get; set; }
        public Int32 SignInBranch { get; set; }
        public Int32 SignOutBranch { get; set; }
        [MaxLength(15)]
        public String breakouttime { get; set; }
        [MaxLength(15)]
        public String breakoutmode { get; set; }
        [MaxLength(200)]
        public String breakoutremarks { get; set; }
        [MaxLength(200)]
        public String breakinremarks { get; set; }
        public DateTime? dt_bin { get; set; }
        [MaxLength(15)]
        public String breakintime { get; set; }
        [MaxLength(40)]
        public String breakinmode { get; set; }
        public DateTime? dt_bout { get; set; }
        public DateTime? tiffinindate { get; set; }
        public DateTime? tiffinoutdate { get; set; }
        [MaxLength(20)]
        public String tiffininTime { get; set; }
        [MaxLength(20)]
        public String tiffinOuTime { get; set; }
        public DateTime? tiffindate { get; set; }
        public Int32 counter { get; set; }
    }
}
