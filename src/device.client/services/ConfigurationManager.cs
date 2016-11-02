using System;
using AutoMapper;
using forte.devices.data;
using forte.devices.extensions;
using forte.devices.models;
using forte.models;
using forte.services;

namespace forte.devices.services
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IDeviceRepository _deviceRepository;
        private StreamingDeviceConfig _deviceConfig;
        private readonly ILogger _logger;

        public ConfigurationManager(IDeviceRepository deviceRepository, ILogger logger)
        {
            _deviceRepository = deviceRepository;
            _logger = logger;
        }

        public StreamingDeviceConfig GetDeviceConfig()
        {
            if (_deviceConfig != null) return _deviceConfig;
            var settings = _deviceRepository.GetSettings();
            _deviceConfig = new StreamingDeviceConfig();
            foreach (var setting in settings)
            {
                _deviceConfig[setting.Name] = setting.ToValue();
            }

            return _deviceConfig;
        }

        public StreamingDeviceConfig UpdateSetting<T>(string setting, T value)
        {
            var config = GetDeviceConfig();
            config[setting] = new DataValue(value);
            _deviceRepository.SaveSetting(setting, value);
            _deviceConfig = null;
            return GetDeviceConfig();
        }
    }
}