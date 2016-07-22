using System;

namespace forte.devices.models
{
    public class DeviceConfig
    {
        public Guid DeviceId { get; set; }
        public string OperatingSystem { get; set; }
        public string Processor { get; set; }
        public int Memory { get; set; }
    }
}