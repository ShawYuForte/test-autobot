#region

using System;
using forte.devices.models;
using forte.models.devices;

#endregion

namespace forte.devices.entities
{
    public class DeviceCommandEntity : Entity
    {
        public DeviceCommands Command { get; set; }
        public string Data { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime? ExecutedOn { get; set; }

        public string ExecutionMessages { get; set; }
        public DateTime IssuedOn { get; set; }

        /// <summary>
        ///     Date / time when the command confirmation published on the server, after local execution or failure
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        public ExecutionStatus Status { get; set; }
    }
}