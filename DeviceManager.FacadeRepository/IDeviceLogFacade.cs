using DeviceManager.DTO;
using System;

namespace DeviceManager.FacadeRepository
{
    public interface IDeviceLogFacade
    {
        bool SaveLogData(RawLogDTO logData);
        AttendanceLogDTO GetAttendanceLog(DateTime date, int EmployeeId);
    }
}
