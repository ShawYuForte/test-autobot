#region

using System.Web.Http;
using AutoMapper;
using device.web.models;
using forte.devices.services;
using forte.models;
using forte.services;

#endregion

namespace device.web.controllers
{
	[RoutePrefix("api/device")]
	public class DeviceController : ApiController
	{
		private readonly IConfigurationManager _configManager;
		private readonly IDeviceDaemon _deviceDaemon;
		private readonly ILogger _logger;

		public DeviceController(IConfigurationManager configManager, IDeviceDaemon deviceDaemon, ILogger logger)
		{
			_configManager = configManager;
			_deviceDaemon = deviceDaemon;
			_logger = logger;
		}

		[Route, HttpGet]
		public IHttpActionResult GetState()
		{
			return Ok(new { model = _deviceDaemon.GetState()});
		}

		[Route("shutdown"), HttpPost]
		public IHttpActionResult Shutdown()
		{
			_deviceDaemon.Shutdown();
			return Ok();
		}

		[Route("fetch"), HttpPost]
		public IHttpActionResult FetchCommand()
		{
			//_deviceDaemon.QueryServer();
			return Ok();
		}

		[Route("publish"), HttpPost]
		public IHttpActionResult PublishState()
		{
			//_deviceDaemon.PublishState();
			return Ok();
		}

		[Route("reset"), HttpPost]
		public IHttpActionResult Reset()
		{
			//_deviceDaemon.ForceResetToIdle();
			return Ok();
		}

		[Route("settings"), HttpGet]
		public IHttpActionResult GetSettings()
		{
			_logger.Debug("Fetching settings");
			var config = _configManager.GetDeviceConfig();
			return Ok(Mapper.Map<SettingsModel>(config));
		}

		[Route("settings/{setting}"), HttpPost]
		public IHttpActionResult UpdateSetting([FromUri]string setting, [FromBody]DataValue dataValue)
		{
			var config = _configManager.GetDeviceConfig();
			if(!config.Contains(setting))
			{
				return BadRequest("Setting does not exist");
			}

			config = _configManager.UpdateSetting(setting, dataValue.Get());
			return Ok(Mapper.Map<SettingsModel>(config));
		}
	}
}