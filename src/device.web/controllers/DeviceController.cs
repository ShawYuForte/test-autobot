#region

using System.Web.Http;
using AutoMapper;
using device.web.models;
using forte.devices.services;

#endregion

namespace device.web.controllers
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        private readonly IConfigurationManager _configManager;

        public DeviceController(IConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        [Route("settings"), HttpGet]
        public IHttpActionResult GetSettings()
        {
            var config = _configManager.GetDeviceConfig();
            return Ok(Mapper.Map<SettingsModel>(config));
        }
    }
}