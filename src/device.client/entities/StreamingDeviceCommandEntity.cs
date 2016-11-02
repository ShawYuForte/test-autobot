#region

using System;
using forte.devices.models;
using forte.models.devices;

#endregion

namespace forte.devices.entities
{
    public class StreamingDeviceCommandEntity : Entity
    {
        /// <summary>
        ///     If command is canceled
        /// </summary>
        public DateTime? CanceledOn { get; set; }

        /// <summary>
        ///     Command to send to device
        /// </summary>
        public StreamingDeviceCommands Command { get; set; }

        /// <summary>
        ///     Command data, if any
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///     If command is executed (i.e. complete)
        /// </summary>
        public DateTime? ExecutedOn { get; set; }

        /// <summary>
        ///     How many times execution was attempted until now
        /// </summary>
        public int ExecutionAttempts { get; set; }

        /// <summary>
        ///     Any execution messages
        /// </summary>
        public string ExecutionMessages { get; set; }

        /// <summary>
        ///     If execution succeeded
        /// </summary>
        public bool ExecutionSucceeded { get; set; }

        /// <summary>
        ///     When was this command issued
        /// </summary>
        public DateTime IssuedOn { get; set; }

        /// <summary>
        ///     How many times should the device retry until giving up
        /// </summary>
        public int MaxAttemptsAllowed { get; set; }

        /// <summary>
        ///     Streaming device identifier
        /// </summary>
        public Guid StreamingDeviceId { get; set; }

        /// <summary>
        ///     Video stream identifier, if applicable
        /// </summary>
        public Guid? VideoStreamId { get; set; }
    }
}