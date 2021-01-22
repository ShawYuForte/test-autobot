using System;
using forte.models;

namespace Forte.Svc.Services.Models.Studios
{
    /// <summary>
    /// The studio request filters.
    /// </summary>
    public class StudioRequest : RequestFilter
    {
        /// <summary>
        /// The trainer filter to apply.
        /// </summary>
        public Guid? TrainerId { get; set; }

        /// <summary>
        /// The city filter to apply.
        /// </summary>
        public string City { get; set; }

        public string StudioName { get; set; }

        public string Website { get; set; }

        public string Status { get; set; }

        public bool? GetOnDemandCount { get; set; }
    }
}
