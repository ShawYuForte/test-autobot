using System;
using System.Collections;
using System.Text;

namespace forte.models.classes
{
    /// <summary>
    /// The public session filters request model.
    /// </summary>
    public class SessionRequest : RequestFilter
    {
        /// <summary>
        /// Trainer IDs filter
        /// </summary>
        public Guid[] TrainerId { get; set; }

        /// <summary>
        /// Studio IDs filter
        /// </summary>
        public Guid[] StudioId { get; set; }

        /// <summary>
        /// Class IDs filter
        /// </summary>
        public Guid[] ClassId { get; set; }

        /// <summary>
        /// Customer User ID filter
        /// </summary>
        public string CustomerUserId { get; set; }

        /// <summary>
        /// Searches for any of the keywords in class name
        /// </summary>
        public string[] Keyword { get; set; }

        /// <summary>
        /// Minimum date for session start time
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Maximum date for session start time
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Minimum date for session start time
        /// </summary>
        public DateTime? FromTime { get; set; }

        /// <summary>
        /// Minimum date for session start time
        /// </summary>
        public DateTime? ToTime { get; set; }

        /// <summary>
        /// Minimum session duration in minutes
        /// </summary>
        public int? MinDuration { get; set; }

        /// <summary>
        /// Maximum session duration in minutes
        /// </summary>
        public int? MaxDuration { get; set; }

        /// <summary>
        /// Searches for sessions held in any of the cities specified
        /// </summary>
        public string[] City { get; set; }

        /// <summary>
        /// Searches for sessions with any of the class types specified
        /// </summary>
        public Guid[] ClassTypeId { get; set; }

        /// <summary>
        /// When true, returns sessions without a class type associated
        /// </summary>
        public bool? NoClassType { get; set; }

        /// <summary>
        /// Searches for sessions with any of the categories specified
        /// </summary>
        public Guid[] CategoryId { get; set; }

        /// <summary>
        /// Include scheduled sessions?
        /// </summary>
        public bool? Scheduled { get; set; }

        /// <summary>
        /// Include live sessions?
        /// </summary>
        public bool? Live { get; set; }

        /// <summary>
        /// Include On-demand past sessions
        /// </summary>
        public bool? Ondemand { get; set; }

        /// <summary>
        /// Include only Featured sessions
        /// </summary>
        public bool? Featured { get; set; }

        /// <summary>
        /// Include only Trending sessions
        /// </summary>
        public bool? Trending { get; set; }

        /// <summary>
        /// </summary>
        public bool? HasVideoUrl { get; set; }

        /// <summary>
        /// </summary>
        public bool? UseAppleTV { get; set; }

        /// <summary>
        /// Specifies how results should be ordered
        /// </summary>
        public SessionRequestOrderOptions? OrderBy { get; set; }

        /// <summary>
        /// need in manage content section of studio manager
        /// </summary>
        public bool ForStudioManager { get; set; }

        public bool? SetFavorite { get; set; }

        /// <summary>
        /// Builds and returns a unique cache key for this object based on all the various properties
        /// </summary>
        /// <returns>A cache key which represents all the various values that make up this object</returns>
        public string GetCacheKey()
        {
            var key = new StringBuilder();

            //Helper to loop through array and append to the StringBuilder as well as check for array null values
            Action<IEnumerable> appendArray = (array) =>
            {
                if (array != null)
                {
                    foreach (var arrayValue in array)
                    {
                        key.Append(arrayValue);
                    }
                }
            };

            //Tedious...but for each property add label and value to the stringbuilder.
            //We need label as otherwise with nulls the values may not be enough to uniquely identify and overlap with other objects with different properties (but same values)
            key.Append("ti:");
            appendArray(TrainerId);

            key.Append("si:");
            appendArray(StudioId);

            key.Append("ci:");
            appendArray(ClassId);

            key.Append("cui:");
            key.Append(CustomerUserId);

            key.Append("k:");
            appendArray(Keyword);

            key.Append("fd:");
            key.Append(FromDate);

            key.Append("td:");
            key.Append(ToDate);

            key.Append("ft:");
            key.Append(FromTime);

            key.Append("tt:");
            key.Append(ToTime);

            key.Append("mind:");
            key.Append(MinDuration);

            key.Append("maxd:");
            key.Append(MaxDuration);

            key.Append("c:");
            key.Append(City);

            key.Append("cti:");
            appendArray(ClassTypeId);

            key.Append("nct:");
            key.Append(NoClassType);

            key.Append("cati:");
            appendArray(CategoryId);

            key.Append("sch:");
            key.Append(Scheduled);

            key.Append("l:");
            key.Append(Live);

            key.Append("od:");
            key.Append(Ondemand);

            key.Append("t:");
            key.Append(Trending);

            key.Append("ob:");
            key.Append(OrderBy);

            key.Append("hvu:");
            key.Append(HasVideoUrl);

            key.Append("uatv:");
            key.Append(UseAppleTV);

            key.Append("setfav:");
            key.Append(SetFavorite);

            return key.ToString();
        }
    }
}
