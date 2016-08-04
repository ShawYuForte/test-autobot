using System;
using forte.devices.models;

namespace forte.devices.entities
{
    public class DeviceCommandEntity : Entity
    {
        public Guid DeviceId { get; set; }
        public DeviceCommands Command { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime? ExecutedOn { get; set; }
        public ExecutionStatus Status { get; set; }

        /// <summary>
        ///     Date / time when the command confirmation published on the server, after local execution or failure
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        public string ExecutionMessages { get; set; }
    }

    public enum ExecutionStatus
    {
        /// <summary>
        ///     Command has been received
        /// </summary>
        Received = 0,

        /// <summary>
        ///     Command has been executed successfully
        /// </summary>
        Executed = 1,

        /// <summary>
        ///     Command execution failed
        /// </summary>
        Failed = 2
    }
}