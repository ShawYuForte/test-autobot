using System;

namespace forte.models.search
{
    /// <summary>
    /// The global search request filters
    /// </summary>
    public class SearchRequest : RequestFilter
    {
        /// <summary>
        /// Initializes the search request
        /// </summary>
        public SearchRequest()
        {
            Take = 5;
        }

        /// <summary>
        /// The text to search
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// The class session status to filter
        /// </summary>
        public string[] Status { get; set; }

        /// <summary>
        /// The city names to filter
        /// </summary>
        public string[] City { get; set; }

        /// <summary>
        /// The session trainer ids to filter
        /// </summary>
        public string[] TrainerId { get; set; }

        /// <summary>
        /// The session studio ids to filter
        /// </summary>
        public string[] StudioId { get; set; }

        /// <summary>
        /// The class equipment for class session type documents.
        /// </summary>
        public string[] Equipment { get; set; }

        /// <summary>
        /// Filter for sessions that need equipment
        /// </summary>
        public bool? NeedsEquipment { get; set; }

        /// <summary>
        /// The session class difficulties to filter
        /// </summary>
        public int[] Difficulty { get; set; }

        /// <summary>
        /// The class category ids to filter
        /// </summary>
        public string[] Category { get; set; }

        /// <summary>
        /// The class session duration ranges to filter
        /// </summary>
        public string[] Duration { get; set; }

        /// <summary>
        /// The class type ids to filter
        /// </summary>
        public string[] ClassType { get; set; }

        /// <summary>
        /// Filter for sessions without a class type
        /// </summary>
        public bool? NoClassType { get; set; }

        /// <summary>
        /// Minimum date/time for session time
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Maximum date/time for session time
        /// </summary>
        public DateTime? ToDate { get; set; }

        public bool? HasVideoUrl { get; set; }

        public bool? UseAppleTV { get; set; }

        public bool? SetFavorite { get; set; }

        public bool? SetReserved { get; set; }
    }
}
