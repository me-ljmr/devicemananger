using DeviceManager.DALRepository;
using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using System;
using System.Collections.Generic; 

namespace DeviceManager.Facade
{
    public class DeviceFacade:IDeviceFacade
    {
        private IDeviceDAL _deviceDAL;
        public DeviceFacade(IDeviceDAL deviceDAL) {
            this._deviceDAL = deviceDAL;

        }
        public IEnumerable<DeviceDTO> GetDevices() {

            
            var allDevices = this._deviceDAL.GetAllIPDevices();
             
            return allDevices;
        }
         
    }
}
