using System;

namespace forte.models.streaming
{
    public enum StreamStatuses
    {
        /// <summary>
        ///     The stream is not created
        /// </summary>
        Unknown = -2,

        /// <summary>
        ///     The stream is disonnected, it exists in the database, but no associated resources
        /// </summary>
        Disconnected = -1,

        /// <summary>
        ///     Video stream is going through configuration
        /// </summary>
        Configuring = 0,

        /// <summary>
        ///     The stream is offline
        /// </summary>
        Offline = 1,

        /// <summary>
        ///     The stream is not broadcasting, but can be previewed
        /// </summary>
        Preview = 2,

        /// <summary>
        ///     The stream is broadcasting live
        /// </summary>
        Live = 3,

        /// <summary>
        ///     The stream is being processed and cannot be edited
        /// </summary>
        Processing = 4,

        /// <summary>
        ///     The stream is on-demand
        /// </summary>
        OnDemand = 5,
    }

    public enum ProgramTypes
    {
        /// <summary>
        ///     Program starts and ends manually
        /// </summary>
        Manual = 0,

        /// <summary>
        ///     Program starts and ends based on the start / end dates
        /// </summary>
        Scheduled = 1,
    }

    public class VideoStreamModel
    {
        /// <summary>
        ///     Stream identifier
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        ///     Video stream media url
        /// </summary>
        public string MediaUrl { get; set; }

        /// <summary>
        ///     Video stream preview media url
        /// </summary>
        public string PreviewMediaUrl { get; set; }
    }

    public class VideoStreamModelEx : VideoStreamModel
    {
        /// <summary>
        ///     Primary ingest url used for streaming
        /// </summary>
        public string PrimaryIngestUrl { get; set; }

        /// <summary>
        ///     Secondary ingest url used for streaming
        /// </summary>
        public string SecondaryIngestUrl { get; set; }
    }
}
