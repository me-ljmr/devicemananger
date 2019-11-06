using DeviceManager.DALRepository;
using DeviceManager.DataEntity.Models;
using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using System;
using System.Linq;
namespace DeviceManager.DAL
{
    public class DeviceLogDAL : IDeviceLogDAL
    {
        private readonly IApplicationServices appServices;

        public DeviceLogDAL( IApplicationServices appServices) {
            this.appServices = appServices;
        }
        public string GetLogModeString(int mode)
        {
            if (appServices.AppSettings.LogMode.Length < (mode + 1))
            {
                throw new Exception($"Log mode not found for {mode}.");
            }
            return appServices.AppSettings.LogMode[mode];
        }
        public int GetLogModeIndex(string mode)
        {
            for (int ind = 0; ind < appServices.AppSettings.LogMode.Length; ind++)
            {
                if (appServices.AppSettings.LogMode[ind] == mode)
                {
                    return ind;
                }
            }
            return -1;
        }
        public string GetVerifyModeString(int mode)
        {
            if (appServices.AppSettings.VerifyMode.Length < (mode + 1))
            {
                throw new Exception($"Verify mode not found for {mode}.");
            }
            return appServices.AppSettings.VerifyMode[mode];
        }
        public int GetVerifyModeIndex(string mode)
        {
            for (int ind = 0; ind < appServices.AppSettings.VerifyMode.Length; ind++)
            {
                if (appServices.AppSettings.VerifyMode[ind] == mode)
                {
                    return ind;
                }
            }
            return -1;
        }
        public bool CheckIn(AttendanceLogDTO data)
        {
            try
            {
                using (var db = new AttendanceContext(appServices)) {

                    var attLog = new AttendanceLog()
                    {
                        EMP_ID = data.EmployeeId,
                        INMODE = appServices.AppSettings.LogMode[data.InMode],
                        INTIME = data.LogTime,
                        TDATE = data.LogDate,
                        TDATE_OUT = null,
                        dt_bin = null,
                        dt_bout  = null,
                        DT_LIN= null,
                        DT_LOUT = null,
                        DT_TOUT = null,
                        INREMARKS = appServices.AppSettings.VerifyMode[data.InVerifyMode]
                         
                    };
                    db.AttendanceLogs.Add(attLog);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return false;
        }
        public bool CheckOut(int attID, AttendanceLogDTO data) {
            try
            {
                using (var db = new AttendanceContext(appServices))
                {
                    var attLog = db.AttendanceLogs.Where(alog =>alog.AttnLogId == attID).FirstOrDefault();

                    attLog.OUTMODE = appServices.AppSettings.LogMode[data.OutMode];
                    attLog.OUTTIME = data.LogOutTime;
                    attLog.TDATE_OUT =  data.LogOutDate;
                    attLog.OUTREMARKS = appServices.AppSettings.VerifyMode[data.OutVerifyMode];
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AttendanceLogDTO GetAttendanceLog(DateTime date, int EmployeeId)
        {
            try
            {
                using (var db = new AttendanceContext(appServices))
                {

                    var s = db.AttendanceLogs.Where(e => e.TDATE.Date ==  date.Date
                        && e.EMP_ID == EmployeeId).OrderByDescending(r=>r.AttnLogId).FirstOrDefault();
                    if (s == null) {
                        return null;
                    }
                    return new AttendanceLogDTO()
                        {
                            ID = s.AttnLogId,
                            EmployeeId = s.EMP_ID,
                            LogDate = (s.TDATE),
                            LogTime = s.INTIME,
                            InMode = GetLogModeIndex(s.INMODE),
                            InVerifyMode = GetVerifyModeIndex(s.INREMARKS),

                            LogOutDate = (s.TDATE_OUT),
                            LogOutTime = s.OUTTIME,
                            OutMode = GetLogModeIndex(s.OUTMODE),
                            OutVerifyMode = GetVerifyModeIndex(s.OUTREMARKS)

                        };
                      
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool SaveRawDeviceLog(RawLogDTO data)
        {
            try
            { 
                using (var db = new AttendanceContext(appServices))
                {
                    db.RawLogs.Add(new DataEntity.Models.RawLog()
                    {
                        Emp_ID = data.EmployeeId,
                        ExtractedFrom = data.DeviceID,
                        InOutMode = data.InOutMode,
                        LogDate = data.LogDate,
                        LogTime = data.LogTime,
                        VerifyMode = data.VerifyMode

                    });
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex) {
                throw ex ;
            }
            
            
        }
        
    }
}
