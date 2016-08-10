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


        /// <summary>
        /// Unique identifier for the preset load instance
        /// </summary>
        public string StreamingPresetLoadHash { get; set; }
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
        Idle,

        /// <summary>
        ///     Device has been prepared for streaming, and is allocated to a particular video stream
        /// </summary>
        ReadyToStream,

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
        ///     Device is busy (setting up or tearing down stream), can queue commands
        /// </summary>
        Busy,

        /// <summary>
        ///     Device is offline, cannot process commands
        /// </summary>
        Offline,

        /// <summary>
        ///     An error occurred with the device workflow
        /// </summary>
        Error
    }
}