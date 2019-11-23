using DeviceManager.DTO;
using DeviceManager.FacadeRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;

namespace DeviceManager.API.Controllers
{

    [ApiController]
    public class DeviceActivityController : ControllerBase
    {
        private readonly IDeviceFacade device;
        private readonly IDeviceLogFacade deviceLog;

        private readonly IApplicationServices appServices;
        public DeviceActivityController(IDeviceFacade device, IDeviceLogFacade deviceLog, IApplicationServices appServices)
        {
            this.device = device;
            this.appServices = appServices;
            this.deviceLog = deviceLog;
        }
        private void ValidateToken(HttpRequest request)
        {
            if (!Request.Headers.ContainsKey("token"))
            {
                throw new NotSupportedException("Not authorized");
            }
            StringValues values = new StringValues();
            Request.Headers.TryGetValue("token", out values);
            var tokenFromRequest = values.FirstOrDefault();
            if (tokenFromRequest != appServices.AppSettings.Token)
            {
                throw new NotSupportedException("Invalid token");

            }
        }
        [Route("api/devices/all")]
        [HttpPost]
        public JsonResult GetIPDevices()
        {
            try
            {
                ValidateToken(Request);
                return new JsonResult(new { error = false, data = device.GetDevices() });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { error = true, messages = new string[] { ex.Message } });
            }

        }
        [Route("api/devicelog/save")]
        [HttpPost]
        public JsonResult SaveDeviceLog([FromBody]  RawLogDTO logData)
        {
            try
            {
                ValidateToken(Request);
                deviceLog.SaveLogData(logData);
                return new JsonResult(new { error = false });
            }
            catch (Exception ex)
            {

                return new JsonResult(new { error = true, messages = new string[] { ex.Message } });
            }
        }

    }
}