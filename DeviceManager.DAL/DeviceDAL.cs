using DeviceManager.DALRepository;
using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DeviceManager.DAL
{
    public class DeviceDAL:BaseDAL, IDeviceDAL
    {
        private IApplicationServices appServices;
        public DeviceDAL(IApplicationServices appServices) : base(appServices)//(IOptions<AppSettingsDTO> appServices)
        {
            this.appServices = appServices;
        }
        public IEnumerable<DeviceDTO> GetAllIPDevices() {
            var devices = new List<DeviceDTO>();
            using (var db = new AttendanceContext(appServices)) {
                devices.AddRange(
                    db.Devices.Where(x => x.ConMode == 0).Select(
                        x=>
                        new DeviceDTO() { 
                            ConnectionMode = x.ConMode,
                            DeviceID = x.DeviceID,
                            DeviceName = x.DeviceDescription,
                            IPAddress = x.IPAddress,
                            PortNo = x.PortNo,
                            MachineNumber= x.machnumber
                        }) 
                    );
                return devices;
            }
        }

        public void SaveDevice(DeviceDTO device)
        {
            throw new NotImplementedException();
        }

        public void UpdateDevice(int deviceId, DeviceDTO device)
        {
            throw new NotImplementedException();
        }
    }
}
