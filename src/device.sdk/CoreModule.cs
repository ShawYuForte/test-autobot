using forte.devices.extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace forte.devices
{
    public static class CoreModule
    {
        public static JsonSerializerSettings GetSerializerSettings(JsonSerializerSettings existingSerializerSettings = null)
        {
            var settings = existingSerializerSettings ?? new JsonSerializerSettings();

            settings.Formatting = Formatting.None;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.ContractResolver = new DictionarySafeCamelCasePropertyNamesContractResolver();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }

        public static void SetDefaultSerializerSettings(JsonSerializerSettings existingSerializerSettings = null)
        {
            var settings = existingSerializerSettings ??
                           (JsonConvert.DefaultSettings != null ? JsonConvert.DefaultSettings() : null);
            settings = GetSerializerSettings(settings);
            JsonConvert.DefaultSettings = () => settings;
        }
    }
}