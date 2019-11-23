using DeviceManager.DataEntity.Models;
using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using Microsoft.EntityFrameworkCore;

namespace DeviceManager.DAL
{
    public class AttendanceContext : DbContext
    {

        public IApplicationServices _appServices;
        public readonly AppSettingsDTO appSettings;
        public AttendanceContext(IApplicationServices appServices)
        {
            _appServices = appServices;
            appSettings = appServices.AppSettings;
        }
        public DbSet<DeviceMaster> Devices { get; set; }
        public DbSet<RawLog> RawLogs { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(appSettings.ConnectionString);
    }
}
