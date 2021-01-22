using System;

namespace forte.models.devices
{
    public class StreamingDeviceConfig
    {
        /// <summary>
        ///     Device unique identifier
        /// </summary>
        public Guid DeviceId { get; set; }

        /// <summary>
        /// Information on the operating system this device is running with
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Information on the processor the device is running with
        /// </summary>
        public string Processor { get; set; }

        /// <summary>
        /// Information on the configured machine memory for the device
        /// </summary>
        public int Memory { get; set; }
    }
}
