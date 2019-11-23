using DeviceManager.DTO;
using System;

namespace DeviceManager.DALRepository
{
    public interface IDeviceLogDAL
    {
        public bool SaveRawDeviceLog(RawLogDTO data);
        public bool CheckIn(AttendanceLogDTO data);
        public bool CheckOut(int attId, AttendanceLogDTO data);
        public AttendanceLogDTO GetAttendanceLog(DateTime date, int EmployeeId);
    }
}
