using System;
using forte.devices.models;

namespace forte.devices.entities
{
    public class VideoStream : Entity
    {
        /// <summary>
        ///     Identifier for class the session is a part of
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        ///     Identifier for session this video stream represents
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        ///     Stream start time, must match session start time, minus the
        ///     any time needed for setup
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     Stream end time, must match the session start time, plus duration and any time needed for setup
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        ///     Status of the stream
        /// </summary>
        public StreamStatuses Status { get; set; }

        /// <summary>
        ///     Program type
        /// </summary>
        public ProgramTypes Type { get; set; }
    }
}