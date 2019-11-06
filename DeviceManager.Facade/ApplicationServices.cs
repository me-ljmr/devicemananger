using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.Facade
{
    public class ApplicationServices : IApplicationServices
    {
        public AppSettingsDTO AppSettings { get; set; }

        public ApplicationServices() {
           
        }
        public ApplicationServices(IOptions<AppSettingsDTO> appSettings)
        {
            AppSettings = appSettings.Value;
        }
    }
}
