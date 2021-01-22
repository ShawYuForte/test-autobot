using System;

namespace forte.models.devices
{
    public enum StreamingDeviceStatuses
    {
        /// <summary>
        ///     Device is idle, ready to take on commands
        /// </summary>
        Idle,

        /// <summary>
        ///     Device is streaming, can queue commands
        /// </summary>
        Streaming,

        /// <summary>
        ///     Device is streaming, can queue commands
        /// </summary>
        StreamingProgram,

        /// <summary>
        ///     Device is streaming and recording, can queue commands
        /// </summary>
        StreamingAndRecording,

        /// <summary>
        ///     Device is streaming and recording, can queue commands
        /// </summary>
        StreamingAndRecordingProgram,

        /// <summary>
        ///     Device is recording, can queue commands
        /// </summary>
        Recording,

        /// <summary>
        ///     Device is recording, can queue commands
        /// </summary>
        RecordingProgram,

        /// <summary>
        ///     Device is offline, cannot process commands
        /// </summary>
        Offline,

        /// <summary>
        ///     An error occurred with the device workflow
        /// </summary>
        Error,
    }

    /// <summary>
    ///     Represents a streaming device
    /// </summary>
    public class StreamingDeviceModel
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
        ///     Device status
        /// </summary>
        public StreamingDeviceStatuses Status { get; set; }

        /// <summary>
        ///     Active session video stream identifier, if streaming
        /// </summary>
        public Guid? ActiveVideoStreamId { get; set; }
    }
}
