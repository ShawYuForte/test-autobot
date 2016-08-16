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
            Settings = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Settings { get; set; }
    }
}