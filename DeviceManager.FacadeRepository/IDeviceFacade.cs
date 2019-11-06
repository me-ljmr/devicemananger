using DeviceManager.DTO;
using System;
using System.Collections.Generic;
using System.Text;
 
namespace DeviceManager.FacadeRepository
{
    public interface IDeviceFacade
    {
        public IEnumerable<DeviceDTO> GetDevices();
    }
}
