using AutoMapper;
using forte.devices.data;
using forte.devices.models;

namespace forte.devices.services
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IDeviceRepository _deviceRepository;
        private StreamingDeviceConfig _deviceConfig;

        public ConfigurationManager(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public StreamingDeviceConfig GetDeviceConfig()
        {
            if (_deviceConfig != null) return _deviceConfig;
            var settings = _deviceRepository.GetSettings();
            _deviceConfig = new StreamingDeviceConfig();
            foreach (var setting in settings)
            {
                _deviceConfig[setting.Name] = Mapper.Map<DataValue>(setting);
            }
            return _deviceConfig;
        }
    }
}