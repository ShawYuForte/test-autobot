using System;
using System.Text;

namespace forte.models.classes
{
    /// <summary>
    ///     The public session filters request model.
    /// </summary>
    public class SessionSummariesRequest : RequestFilter
    {
        /// <summary>
        ///     Trainer ID filter
        /// </summary>
        public Guid? TrainerId { get; set; }

        /// <summary>
        ///     Studio ID filter
        /// </summary>
        public Guid? StudioId { get; set; }

        /// <summary>
        ///     Class IDs filter
        /// </summary>
        public Guid? ClassId { get; set; }

        /// <summary>
        ///     Customer User ID filter
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Class type filter
        /// </summary>
        public Guid? ClassTypeId { get; set; }

        /// <summary>
        ///     When true, returns sessions without a class type associated
        /// </summary>
        public bool? NoClassType { get; set; }

        /// <summary>
        ///     Include scheduled sessions?
        /// </summary>
        public bool? Scheduled { get; set; }

        /// <summary>
        ///     Include live sessions?
        /// </summary>
        public bool? Live { get; set; }

        /// <summary>
        ///     Include On-demand past sessions
        /// </summary>
        public bool? Ondemand { get; set; }

        /// <summary>
        ///     Include only Trending sessions
        /// </summary>
        public bool? Trending { get; set; }

        public bool? SetFavorite { get; set; }

        public bool? SetReserved { get; set; }

        /// <summary>
        ///     Specifies how results should be ordered
        /// </summary>
        public SessionRequestOrderOptions? OrderBy { get; set; }

        /// <summary>
        ///     Builds and returns a unique cache key for this object based on all the various properties
        /// </summary>
        /// <returns>A cache key which represents all the various values that make up this object</returns>
        public string GetCacheKey()
        {
            var key = new StringBuilder();

            //Tedious...but for each property add label and value to the stringbuilder.
            //We need label as otherwise with nulls the values may not be enough to uniquely identify and overlap with other objects with different properties (but same values)
            key.Append("ti:");
            key.Append(TrainerId);

            key.Append("si:");
            key.Append(StudioId);

            key.Append("ci:");
            key.Append(ClassId);

            key.Append("cti:");
            key.Append(ClassTypeId);

            key.Append("nct:");
            key.Append(NoClassType);

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

            return key.ToString();
        }
    }
}
