using System;

namespace forte.models.trainers
{
    public class TrainerRequest : RequestFilter
    {
        /// <summary>
        /// When specified, returns trainers associated with class
        /// </summary>
        public Guid? ClassId { get; set; }

        /// <summary>
        /// When specified, returns trainers associated with studio
        /// </summary>
        public Guid? StudioId { get; set; }

        /// <summary>
        /// Get Studios located in specified city
        /// </summary>
        public string City { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }
    }
}
