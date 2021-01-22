using System;

namespace forte.models.trainers
{
    /// <summary>
    /// Patch actions the user can request
    /// </summary>
    public enum TrainerPatchActions
    {
        UserId = 1,
        Status = 2,
    }

    public class TrainerPatchRequestModel
    {
        /// <summary>
        /// Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// Patch operation to perform
        /// </summary>
        public TrainerPatchActions Action { get; set; }

        /// <summary>
        /// Patched UserId of trainer
        /// </summary>
        public string UserId { get; set; }

        public TrainerStatus? Status { get; set; }
    }
}
