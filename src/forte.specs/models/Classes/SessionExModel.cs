using System;
using forte.models.trainers;

namespace forte.models.classes
{
    public class SessionExModel : SessionModel
    {
        /// <summary>
        ///     Created by userId
        /// </summary>
        public string CreatedByUserId { get; set; }

        public new ClassExModel Class { get; set; }

        /// <summary>
        ///     Is session marked as featured
        /// </summary>
        public bool? Featured { get; set; }

        /// <summary>
        ///     Class rating
        /// </summary>
        public decimal? Rating { get; set; }

        /// <summary>
        ///     Streaming device identifier
        /// </summary>
        public Guid? StreamingDeviceId { get; set; }

        public new TrainerExModel Trainer { get; set; }

        /// <summary>
        ///     Is session marked as trending
        /// </summary>
        public bool? Trending { get; set; }

        public byte[] Version { get; set; }

        /// <summary>
        ///     Pre scheduling buffer, used to prevent overlapping schedules
        /// </summary>
        public int PreStartSchedulingBuffer { get; set; }

        /// <summary>
        ///     Post scheduling buffer, used to prevent overlapping schedules
        /// </summary>
        public int PostEndSchedulingBuffer { get; set; }

        public string[] FavoriteUserIds { get; set; }
    }
}
