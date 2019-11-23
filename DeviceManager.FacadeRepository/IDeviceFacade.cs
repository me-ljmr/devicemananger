using DeviceManager.DTO;
using System.Collections.Generic;

namespace DeviceManager.FacadeRepository
{
    public interface IDeviceFacade
    {
        public IEnumerable<DeviceDTO> GetDevices();
    }
}
