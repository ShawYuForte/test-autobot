using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace forte.devices.models
{
    public class StreamingDeviceConfig
    {
        /// <summary>
        ///     Device unique identifier
        /// </summary>
        public Guid DeviceId
        {
            get { return Get<Guid>(nameof(DeviceId)); }
            set { Set(nameof(DeviceId), value); }
        }

        /// <summary>
        /// Information on the operating system this device is running with
        /// </summary>
        public string OperatingSystem
        {
            get { return Get<string>(nameof(OperatingSystem)); }
            set { Set(nameof(OperatingSystem), value); }
        }

        /// <summary>
        /// Information on the processor the device is running with
        /// </summary>
        public string Processor
        {
            get { return Get<string>(nameof(Processor)); }
            set { Set(nameof(Processor), value); }
        }

        /// <summary>
        /// Information on the configured machine memory for the device
        /// </summary>
        public int Memory
        {
            get { return Get<int>(nameof(Memory)); }
            set { Set(nameof(Memory), value); }
        }


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

        public void Set<T>(string setting, T value)
        {
            this[setting].Set(value);
        }

        public IDictionary<string, DataValue> ToDictionary()
        {
            return new ReadOnlyDictionary<string, DataValue>(_settings);
        }
    }
}