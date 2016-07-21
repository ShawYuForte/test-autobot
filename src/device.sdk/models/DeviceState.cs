using System;

namespace forte.devices.models
{
    public class DeviceState
    {
        /// <summary>
        ///     Unique identifier for this device
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Identifier for studio where the device is located
        /// </summary>
        public Guid StudioId { get; set; }

        /// <summary>
        ///     UTC date and time when the device state was last captured
        /// </summary>
        public DateTime StateCapturedOn { get; set; }

        /// <summary>
        ///     Active session identifier, if streaming
        /// </summary>
        public Guid? ActiveSessionId { get; set; }

        /// <summary>
        ///     Is the client recording
        /// </summary>
        public bool Recording { get; set; }

        /// <summary>
        ///     Is the client streaming
        /// </summary>
        public bool Streaming { get; set; }
    }
}