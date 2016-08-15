using System;
using forte.models.devices;

namespace forte.devices.models
{
    public class DeviceCommandModelEx : DeviceCommandModel
    {
        public ExecutionStatus Status { get; set; }
        public int RetryCount { get; set; }
        public DateTime PublishedOn { get; set; }
    }
}