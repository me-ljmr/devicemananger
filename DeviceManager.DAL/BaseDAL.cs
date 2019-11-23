using DeviceManager.FacadeRepository;

namespace DeviceManager.DAL
{
    public class BaseDAL
    {
        protected readonly IApplicationServices _appServices;
        public BaseDAL(IApplicationServices appServices)
        {
            _appServices = appServices;
        }

    }
}
