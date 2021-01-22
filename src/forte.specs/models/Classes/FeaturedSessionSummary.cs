using System;

namespace forte.models.classes
{
    public class FeaturedSessionSummary : SessionSummary
    {
        /// <summary>
        ///     When was this assignment created
        /// </summary>
        public DateTime? FeaturedCreated { get; set; }

        /// <summary>
        ///     Who was this assignment created by
        /// </summary>
        public string FeaturedCreatedBy { get; set; }

        /// <summary>
        ///     When was this assignment last modified
        /// </summary>
        public DateTime? FeaturedLastModified { get; set; }

        /// <summary>
        ///     Who was this assignment last modified by
        /// </summary>
        public string FeaturedLastModifiedBy { get; set; }

        /// <summary>
        ///     Ordinal used for ordering featured sessions
        /// </summary>
        public decimal FeaturedOrdinal { get; set; }

        /// <summary>
        ///     Row version of the featured mapping
        /// </summary>
        public byte[] FeaturedVersion { get; set; }

        public string VideoUrl { get; set; }
    }
}
