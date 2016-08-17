#region

using System.Web.Http;
using AutoMapper;
using device.web.models;
using forte.devices.services;
using forte.models;

#endregion

namespace device.web.controllers
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        private readonly IConfigurationManager _configManager;
        private readonly IDeviceDaemon _deviceDaemon;

        public DeviceController(IConfigurationManager configManager, IDeviceDaemon deviceDaemon)
        {
            _configManager = configManager;
            _deviceDaemon = deviceDaemon;
        }

        [Route("settings"), HttpGet]
        public IHttpActionResult GetSettings()
        {
            var config = _configManager.GetDeviceConfig();
            return Ok(Mapper.Map<SettingsModel>(config));
        }

        [Route("settings/{setting}"), HttpPost]
        public IHttpActionResult UpdateSetting([FromUri]string setting, [FromBody]DataValue dataValue)
        {
            var config = _configManager.GetDeviceConfig();
            if (!config.Contains(setting))
                return BadRequest("Setting does not exist");

            config = _configManager.UpdateSetting(setting, dataValue.Get());
            return Ok(Mapper.Map<SettingsModel>(config));
        }
    }
}