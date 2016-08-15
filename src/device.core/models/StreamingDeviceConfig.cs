using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using forte.models;

namespace forte.devices.models
{
    public class StreamingDeviceConfig
    {
        /// <summary>
        ///     Device unique identifier
        /// </summary>
        public Guid DeviceId => Get<Guid>(nameof(DeviceId));

        /// <summary>
        /// Information on the operating system this device is running with
        /// </summary>
        public string OperatingSystem => Get<string>(nameof(OperatingSystem));

        /// <summary>
        /// Information on the processor the device is running with
        /// </summary>
        public string Processor => Get<string>(nameof(Processor));

        /// <summary>
        /// Information on the configured machine memory for the device
        /// </summary>
        public int Memory => Get<int>(nameof(Memory));

        private readonly Dictionary<string, DataValue> _settings = new Dictionary<string, DataValue>();

        //TODO
        // 1. add the collection of cameras to allow querying for health, turning them off, etc.
        public DataValue this[string setting]
        {
            get
            {
                if (!_settings.ContainsKey(setting)) _settings.Add(setting, new DataValue());
                return _settings[setting];
            }
            set
            {
                if (!_settings.ContainsKey(setting))
                    _settings.Add(setting, value);
                else
                    _settings[setting] = value;
            }
        }

        public T Get<T>(string setting, T defaultValue = default(T))
        {
            var value = this[setting].Get<T>();
            return Equals(value, default(T)) ? defaultValue : value;
        }

        public IDictionary<string, DataValue> ToDictionary()
        {
            return new ReadOnlyDictionary<string, DataValue>(_settings);
        }
    }
}