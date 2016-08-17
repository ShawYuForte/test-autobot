#region

using System.Collections.Generic;
using forte.models;

#endregion

namespace device.web.models
{
    public class SettingsModel
    {
        public SettingsModel()
        {
            Settings = new Dictionary<string, DataValue>();
        }

        public Dictionary<string, DataValue> Settings { get; set; }
    }
}