using System;

namespace forte.devices.entities
{
    public class DeviceConfig : Entity
    {
        public Guid DeviceId { get; set; }
        public string OperatingSystem { get; set; }
        public string Processor { get; set; }
        public int Memory { get; set; }
    }
}