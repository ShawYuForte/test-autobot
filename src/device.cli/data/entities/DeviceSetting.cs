using System;

namespace forte.devices.entities
{
    public class DeviceSetting : Entity
    {
        public string Name { get; set; }

        public Guid? GuidValue { get; set; }

        public string StringValue { get; set; }

        public int? IntValue { get; set; }

        public DateTime? DateTimeValue { get; set; }

        public bool? BoolValue { get; set; }

        public byte[] ByteArrayValue { get; set; }
    }
}