using DeviceManager.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.FacadeRepository
{
    public interface IDeviceLogFacade
    {
        bool SaveLogData(RawLogDTO logData);
        AttendanceLogDTO GetAttendanceLog(DateTime date, int EmployeeId);
    }
}
