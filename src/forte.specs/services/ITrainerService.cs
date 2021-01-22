using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.trainers;

namespace forte.services
{
    public interface ITrainerService
    {
        /// <summary>
        ///     Retrieve the trainer public profile
        /// </summary>
        /// <param name="trainerId">The trainer ID.</param>
        /// <returns>The trainer model.</returns>
        TrainerModel GetTrainerPublicProfile(Guid trainerId);

        /// <summary>
        ///     Retrieve the trainer public profile
        /// </summary>
        /// <param name="trainerPermalink">The trainer permalink.</param>
        /// <returns>The trainer model.</returns>
        TrainerModel GetTrainerPublicProfile(string trainerPermalink);

        /// <summary>
        ///     Retrieve Trainer by UserID
        /// </summary>
        /// <param name="trainerUserId"></param>
        /// <returns></returns>
        TrainerModel GetTrainerPublicProfileFromUserId(string trainerUserId);

        /// <summary>
        ///     Gets trainers list with optional filter and pagination parameters.
        /// </summary>
        /// <param name="request">optional filter and pagination params</param>
        /// <returns>Paged list of trainers</returns>
        TrainerPageModel GetTrainers(TrainerRequest request);

        /// <summary>
        ///     Gets trainer.
        /// </summary>
        /// <param name="trainerId">The trainer identifier.</param>
        /// <returns>EditableTrainerModel.</returns>
        TrainerExModel GetEditableTrainer(Guid trainerId);

        /// <summary>
        ///     Gets trainer.
        /// </summary>
        /// <param name="trainerPermalink">The trainer permalink.</param>
        /// <returns>EditableTrainerModel.</returns>
        TrainerExModel GetEditableTrainer(string trainerPermalink);

        /// <summary>
        ///     Updates the trainer.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <returns>The extended model of updated trainer. </returns>
        Task<TrainerExModel> UpdateTrainerAsync(EditTrainerRequestModel request);

        /// <summary>
        ///     Creates a new trainer.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <returns>The extended model of created trainer. </returns>
        Task<TrainerExModel> CreateTrainerAsync(EditTrainerRequestModel request);

        /// <summary>
        ///     Delete trainer
        /// </summary>
        /// <param name="trainerId"></param>
        Task DeleteTrainerAsync(Guid trainerId);

        /// <summary>
        ///     Upload a trainer image
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<string> UploadTrainerImageAsync(string localFileName, string type);

        /// <summary>
        ///     Updates the user Id associated with a trainer
        /// </summary>
        /// <param name="trainerId">Trainer to update</param>
        /// <param name="userId">User Id to update</param>
        /// <param name="trainerVersion">The version concurrency field.</param>
        /// <returns>The updated trainer</returns>
        Task<TrainerExModel> UpdateTrainerUserIdAsync(Guid trainerId, string userId, byte[] trainerVersion);

        /// <summary>
        ///     Updates the trainer image url fields.
        /// </summary>
        /// <param name="trainerId">The trainer ID.</param>
        /// <param name="headerImageUrl">The trainer header image url.</param>
        /// <param name="thumbnailImageUrl">The trainer thumbnail image url.</param>
        /// <param name="trainerVersion">The version concurrency field.</param>
        /// <returns>The updated trainer.</returns>
        TrainerExModel UpdateTrainerImageUrls(
            Guid trainerId,
            string headerImageUrl,
            string thumbnailImageUrl,
            byte[] trainerVersion);

        /// <summary>
        ///     Update trainer status based on business rules
        /// </summary>
        /// <param name="trainerId"></param>
        /// <param name="status"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<TrainerExModel> UpdateTrainerStatusAsync(Guid trainerId, TrainerStatus status, byte[] version);
    }
}
