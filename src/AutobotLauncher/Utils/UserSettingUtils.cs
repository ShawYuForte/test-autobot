using AutobotLauncher.Forms.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutobotLauncher.Utils
{
    public static class UserSettingUtils
    {
        public static async Task InitUserSettingsAfterInstallation()
        {
            var userSettingsPath = $"{FileUtils.Dir.FullName}\\userSettings.xml";

            if (File.Exists(userSettingsPath))
            {
                try
                {
                    var userSettingsXml = XDocument.Load(userSettingsPath);
                    XElement forteSettings = userSettingsXml.Root.Elements("forteSettings").FirstOrDefault();

                    var customDeviceIdValue = forteSettings.Attribute("CustomDeviceId").Value;
                    var studioUrlValue = forteSettings.Attribute("StudioUrl").Value;
                    var videoUrlValue = forteSettings.Attribute("VideoUrl").Value;

                    if (!string.IsNullOrEmpty(customDeviceIdValue))
                    {
                        await ClientApiInteractor.SettingSave("CustomDeviceId", customDeviceIdValue);
                        await ClientApiInteractor.SettingSave("DeviceId", customDeviceIdValue);
                        await ClientApiInteractor.SettingSave("CustomDeviceIdPresent", (!string.IsNullOrEmpty("True")).ToString());
                    }
                }
                catch { }
            }
        }

        public static async Task UpdateUserSettings(BaseConfigModel model)
        {
            var userSettingsPath = $"{FileUtils.Dir.FullName}\\userSettings.xml";

            if (File.Exists(userSettingsPath))
            {
                try
                {
                    var userSettingsXml = XDocument.Load(userSettingsPath);
                    XElement forteSettings = userSettingsXml.Root.Elements("forteSettings").FirstOrDefault();

                    forteSettings.Attribute("CustomDeviceId").SetValue(model.CustomDeviceId);
                    userSettingsXml.Save(userSettingsPath);
                }
                catch { }
            }

            await ClientApiInteractor.SettingSave("CustomDeviceId", "");
        }
    }
}
