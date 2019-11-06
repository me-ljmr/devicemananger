using DeviceManager.FacadeRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceManager.DAL
{
    public class BaseDAL
    {
        protected readonly IApplicationServices _appServices;
        public BaseDAL(IApplicationServices appServices) {
            _appServices = appServices; 
        }

    }
}
