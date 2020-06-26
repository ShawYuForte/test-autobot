using System.Collections.Generic;
using System.Collections.ObjectModel;
using forte.models;

namespace forte.devices.models
{
	public class StreamingDeviceConfig
    {
        private readonly Dictionary<string, DataValue> _settings = new Dictionary<string, DataValue>();

        //TODO
        // 1. add the collection of cameras to allow querying for health, turning them off, etc.
        public DataValue this[string setting]
        {
            get
            {
				if(!_settings.ContainsKey(setting))
				{
					_settings.Add(setting, new DataValue(string.Empty));
				}
                return _settings[setting];
            }
			set
			{
				if(!_settings.ContainsKey(setting))
				{
					_settings.Add(setting, value);
				}
				else
				{
					_settings[setting] = value;
				}
            }
        }

        public T Get<T>(string setting, T defaultValue = default(T))
        {
            var value = this[setting].Get<T>();
            return Equals(value, default(T)) ? defaultValue : value;
        }

        public object Get(string setting)
        {
            return _settings.ContainsKey(setting) ? _settings[setting].Get() : null;
        }

        public bool Contains(string setting)
        {
            return _settings.ContainsKey(setting);
        }

        public IDictionary<string, DataValue> ToDictionary()
        {
            return new ReadOnlyDictionary<string, DataValue>(_settings);
        }
    }
}