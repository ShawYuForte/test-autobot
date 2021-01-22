using System;

namespace forte.models.classes
{
    public class SessionSummary
    {
        /// <summary>
        ///     UTC date and time when this session record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Session unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Class required equipment
        /// </summary>
        public string Equipment { get; set; }

        /// <summary>
        ///     Specifies whether this video is flagged as a free video
        /// </summary>
        public bool Freemium { get; set; }

        /// <summary>
        ///     Unique session permalink
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        ///     Session status
        /// </summary>
        public SessionStatus Status { get; set; }

        /// <summary>
        ///     Session start time in UTC
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///     Session end time in UTC
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        ///     Session duration in minutes
        /// </summary>
        public int Duration { get; set; }

        public bool Trending { get; set; }

        /// <summary>
        ///     Specifies this session is reserved by the currently requesting user
        /// </summary>
        public bool IsReserved { get; set; }

        public bool IsFavorite { get; set; }

        public Guid ClassId { get; set; }

        public string ClassName { get; set; }

        public string CoverImageUrl { get; set; }

        public string CoverVideoUrl { get; set; }

        public Guid? ClassTypeId { get; set; }

        public string ClassType { get; set; }

        public string Description { get; set; }

        public string ClassPermalink { get; set; }

        public int Difficulty { get; set; }

        public string ThumbnailImageUrl { get; set; }

        public string HeaderImageUrl { get; set; }

        public Guid StudioId { get; set; }

        public string StudioName { get; set; }

        public string City { get; set; }

        public string StudioPermalink { get; set; }

        public Guid TrainerId { get; set; }

        public string TrainerName { get; set; }

        public string TrainerProfilePhotoUrl { get; set; }

        public string TrainerPermalink { get; set; }
    }
}
