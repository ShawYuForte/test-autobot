#region

using forte.devices.entities;
using forte.models;

#endregion

namespace forte.devices.extensions
{
    public static class DataExtensions
    {
        public static DataValue ToValue(this DeviceSetting setting)
        {
            if (setting == null) return null;
            var value = new DataValue();

            if (setting.BoolValue.HasValue) value.BoolValue = setting.BoolValue;
            if (setting.ByteArrayValue != null && setting.ByteArrayValue.Length > 0)
                value.ByteArrayValue = setting.ByteArrayValue;
            if (setting.DateTimeValue.HasValue) value.DateTimeValue = setting.DateTimeValue;
            if (setting.GuidValue.HasValue) value.GuidValue = setting.GuidValue;
            if (setting.IntValue.HasValue) value.IntValue = setting.IntValue;
            if (!string.IsNullOrWhiteSpace(setting.StringValue)) value.StringValue = setting.StringValue;

            return value;
        }
    }
}