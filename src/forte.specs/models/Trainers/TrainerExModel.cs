using System;

namespace forte.models.trainers
{
    public class TrainerExModel : TrainerModel
    {
        /// <summary>
        /// UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// System user identifier
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Admin user id that created the trainer
        /// </summary>
        public string CreatedByUserId { get; set; }
    }
}
