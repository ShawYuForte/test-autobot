using forte.devices.models;

namespace forte.devices.services
{
    public interface IConfigurationManager
    {
        /// <summary>
        ///     Retrieve device configuration
        /// </summary>
        /// <returns></returns>
        StreamingDeviceConfig GetDeviceConfig();

        /// <summary>
        /// Update a specific setting, and retrieve updated configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setting"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        StreamingDeviceConfig UpdateSetting<T>(string setting, T value);
    }
}