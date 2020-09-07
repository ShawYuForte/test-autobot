using forte.devices.models;

namespace forte.devices.services
{
    public interface IConfigurationManager
    {
        StreamingDeviceConfig GetDeviceConfig();
        StreamingDeviceConfig UpdateSetting<T>(string setting, T value);
		void DeleteSetting(string setting);
	}
}