#region

using forte.devices.models;

#endregion

namespace forte.devices.services
{
    public class SimulatorConfigManager : IConfigurationManager
    {
        private readonly StreamingDeviceConfig _streamingDeviceConfig = new StreamingDeviceConfig();

        public StreamingDeviceConfig GetDeviceConfig()
        {
            return _streamingDeviceConfig;
        }

        public StreamingDeviceConfig UpdateSetting<T>(string setting, T value)
        {
            _streamingDeviceConfig[setting] = new forte.models.DataValue(value);
            return _streamingDeviceConfig;
        }
    }
}