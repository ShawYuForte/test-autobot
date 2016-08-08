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
    }
}