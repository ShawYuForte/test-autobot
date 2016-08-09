using System;

namespace forte.devices.models
{
    public class StreamingDeviceState
    {
        public StreamingDeviceState()
        {
            Status = StreamingDeviceStatuses.Idle;
        }

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
        ///     Device status
        /// </summary>
        public StreamingDeviceStatuses Status { get; set; }
    }

    public enum StreamingDeviceStatuses
    {
        /// <summary>
        ///     Device status is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     Device is idle, ready to take on commands
        /// </summary>
        Idle = 1,

        /// <summary>
        ///     Device is streaming, can queue commands
        /// </summary>
        Streaming = 2,

        /// <summary>
        ///     Device is streaming and recording, can queue commands
        /// </summary>
        StreamingAndRecording = 3,

        /// <summary>
        ///     Device is streaming and recording, can queue commands
        /// </summary>
        Recording = 4,

        /// <summary>
        ///     Device is busy (setting up or tearing down stream), can queue commands
        /// </summary>
        Busy = 5,

        /// <summary>
        ///     Device is offline, cannot process commands
        /// </summary>
        Offline = 6,

        /// <summary>
        ///     An error occurred with the device workflow
        /// </summary>
        Error = 7
    }
}