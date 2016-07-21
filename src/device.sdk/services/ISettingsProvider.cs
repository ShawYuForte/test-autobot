using forte.devices.models;

namespace forte.devices.services
{
    public interface ISettingsProvider
    {
        /// <summary>
        /// Get app settings
        /// </summary>
        /// <returns></returns>
        Settings GetSettings();
    }
}