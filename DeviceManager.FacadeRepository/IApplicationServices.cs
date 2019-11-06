using DeviceManager.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.FacadeRepository
{
    public interface IApplicationServices
    {
        AppSettingsDTO AppSettings { get; set; }
    }
}
