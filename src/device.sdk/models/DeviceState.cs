using System;

namespace forte.devices.models
{
    public class StreamingDeviceState
    {
        /// <summary>
        ///     Device unique identifier
        /// </summary>
        public Guid DeviceId { get; set; }

        /// <summary>
        ///     UTC date and time when the device state was last captured
        /// </summary>
        public DateTime StateCapturedOn { get; set; }

        /// <summary>
        ///     Active session identifier, if streaming
        /// </summary>
        public Guid? ActiveVideoStreamId { get; set; }

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