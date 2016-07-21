using System;

namespace forte.devices.entities
{
    public class DeviceState : Entity
    {
        /// <summary>
        ///     Current video stream identifier, if streaming
        /// </summary>
        public Guid? CurrentVideoStreamId { get; set; }

        /// <summary>
        ///     Identifier for session associated with the current video stream, if streaming
        /// </summary>
        public Guid? CurrentSessionId { get; set; }
    }
}