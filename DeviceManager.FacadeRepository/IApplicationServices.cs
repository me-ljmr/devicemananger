using DeviceManager.DTO;

namespace DeviceManager.FacadeRepository
{
    public interface IApplicationServices
    {
        AppSettingsDTO AppSettings { get; set; }
    }
}
