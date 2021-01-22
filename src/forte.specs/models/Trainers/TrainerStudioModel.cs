using System;

namespace forte.models.trainers
{
    public class TrainerStudioModel
    {
        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Studio name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  City
        /// </summary>
        public string City { get; set; }
    }
}
