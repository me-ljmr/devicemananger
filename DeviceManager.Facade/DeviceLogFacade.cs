using DeviceManager.DALRepository;
using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.Facade
{
    public class DeviceLogFacade : IDeviceLogFacade
    {
        private readonly IDeviceLogDAL deviceLog;
        //data extraction parameters
        private readonly bool isSingleShift;
        private readonly string extractionMode;
        private readonly bool isMultiShift;

        private readonly IApplicationServices appServices;
        public DeviceLogFacade(IDeviceLogDAL deviceLog,IApplicationServices appServices)
        {
            this.appServices = appServices;
            this.deviceLog = deviceLog;
            var dataExtractionMode= appServices.AppSettings.DataExtractionMode;
            isSingleShift = dataExtractionMode.Split(':')[0].ToUpper() =="SINGLE";
            isMultiShift = !isSingleShift;
            extractionMode = dataExtractionMode.Split(':')[1].ToUpper();
        }
        public AttendanceLogDTO GetAttendanceLog(DateTime date, int employeeId)
        {
            return deviceLog.GetAttendanceLog(date, employeeId);
        }

        public bool SaveLogData(RawLogDTO logData)
        {
            deviceLog.SaveRawDeviceLog(logData);
            var attLog = GetAttendanceLog(logData.LogDate, logData.EmployeeId);

            if (isSingleShift)
            {
                if (extractionMode == SingleShiftExtractionModes.FirstInLastOut 
                    || extractionMode == SingleShiftExtractionModes.FirstInFirstOut)
                {
                    if (attLog == null)
                        logData.InOutMode = (int)LogModeEnum.CheckIn;
                    else {
                        if (extractionMode == SingleShiftExtractionModes.FirstInFirstOut)
                        {
                            if (attLog.LogOutDate != null)
                                //in first in first out,  data exists so no need to update log out
                                return false;
                            else
                                logData.InOutMode = (int)LogModeEnum.CheckOut;
                        }
                        else if (extractionMode == SingleShiftExtractionModes.FirstInLastOut)
                        {
                            logData.InOutMode = (int)LogModeEnum.CheckOut;
                        } 
                    } 
                }
                 
                if ((LogModeEnum)logData.InOutMode == LogModeEnum.CheckOut)
                {
                    // for this to work ./ there should already be a record

                    if (attLog != null)
                    {
                        attLog.OutMode = logData.InOutMode;
                        attLog.OutVerifyMode = logData.VerifyMode;
                        attLog.LogOutDate = logData.LogDate;
                        attLog.LogOutTime = logData.LogTime; 
                        deviceLog.CheckOut(attLog.ID, attLog);
                        return true;
                    }
                    else
                    {
                        throw new Exception("No in data found. ");
                    }
                }
                else if ((LogModeEnum)logData.InOutMode == LogModeEnum.CheckIn)
                {
                    // checking if we have record already available for the day

                    var attlog = deviceLog.GetAttendanceLog(logData.LogDate, logData.EmployeeId);
                    if (attlog != null && attlog.ID != 0)
                    {
                        throw new Exception("Employee already checked in for today. Only raw log will be saved.");
                    }

                    attlog = new AttendanceLogDTO();
                    attlog.EmployeeId = logData.EmployeeId;
                    attlog.InMode = logData.InOutMode;
                    attlog.InVerifyMode = logData.VerifyMode;
                    attlog.LogDate = logData.LogDate;
                    attlog.LogTime = logData.LogTime;

                    deviceLog.CheckIn(attlog);
                    return true;
                }
            }
            return false;
        }
    }
}
