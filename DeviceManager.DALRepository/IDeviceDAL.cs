using DeviceManager.DTO;
using System.Collections.Generic;

namespace DeviceManager.DALRepository
{
    public interface IDeviceDAL
    {
        public IEnumerable<DeviceDTO> GetAllIPDevices();
        public void SaveDevice(DeviceDTO device);
        public void UpdateDevice(int deviceId, DeviceDTO device);

    }
}
