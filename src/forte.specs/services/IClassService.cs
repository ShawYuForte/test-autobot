using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;
using forte.models.streaming;

namespace forte.services
{
    /// <summary>
    ///     Describes the service for operations related to classes and sessions.
    /// </summary>
    public interface IClassService
    {
        /// <summary>
        ///     Creates a new class.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <returns>The extended model of created class. </returns>
        Task<ClassExModel> CreateClassAsync(EditClassRequestModel request);

        /// <summary>
        ///     Creates a manual video stream. The request is executed asynchronously, look for events to determine
        ///     the outcome of this request.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        VideoStreamModel CreateManualVideoStream(Guid classId, Guid sessionId);

        /// <summary>
        ///     Soft deletes the class.
        /// </summary>
        /// <param name="classId">The class ID.</param>
        Task DeleteClassAsync(Guid classId);

        /// <summary>
        ///     Deletes the class permanently.
        /// </summary>
        /// <param name="classId">The class ID.</param>
        Task PurgeClassAsync(Guid classId);

        /// <summary>
        ///     Get individual class
        /// </summary>
        /// <param name="classRequest"></param>
        /// <returns></returns>
        ClassModel GetClass(ClassRequest classRequest);

        /// <summary>
        ///     Gets the class by ID.
        /// </summary>
        /// <param name="classId">The class ID.</param>
        /// <returns>The class model. </returns>
        [Obsolete("Use GetClass(ClassRequest)")]
        ClassModel GetClass(Guid classId);

        /// <summary>
        ///     Gets the class by permalink.
        /// </summary>
        /// <param name="classPermalink">The class permalink.</param>
        /// <returns>The class model. </returns>
        [Obsolete("Use GetClass(ClassRequest)")]
        ClassModel GetClass(string classPermalink);

        /// <summary>
        ///     Gets the public classes.
        /// </summary>
        /// <returns> The paged class model collection. </returns>
        Task<ClassPageModel> GetClassesAsync(ClassRequestFilter filter);

        /// <summary>
        ///     Restores the deleted class.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns>The extended model of restored class. </returns>
        Task<ClassExModel> RestoreClassAsync(Guid classId);

        /// <summary>
        ///     Updates the class.
        /// </summary>
        /// <param name="request">The request model.</param>
        /// <returns>The extended model of updated class. </returns>
        Task<ClassExModel> UpdateClassAsync(EditClassRequestModel request);

        /// <summary>
        ///     Updates the class image url fields.
        /// </summary>
        /// <param name="classId">The class ID.</param>
        /// <param name="headerImageUrl">The class header image url.</param>
        /// <param name="thumbnailImageUrl">The class thumbnail image url.</param>
        /// <param name="coverImageUrl">The class cover image url.</param>
        /// <param name="classVersion">The version concurrency field.</param>
        /// <returns>The updated class.</returns>
        ClassExModel UpdateClassImageUrls(
            Guid classId,
            string headerImageUrl,
            string thumbnailImageUrl,
            string coverImageUrl,
            byte[] classVersion);

        /// <summary>
        ///     Uploads a class image.
        /// </summary>
        /// <param name="localFilePath">The local file path to upload from.</param>
        /// <param name="type">The class image type (thumbnails or headers).</param>
        /// <returns>The uploaded file URL.</returns>
        Task<string> UploadClassImageAsync(string localFilePath, string type);

        /// <summary>
        ///     Fetch class ratings
        /// </summary>
        /// <exception cref="ActionNotAllowedException">If user requests all and does not have permissions</exception>
        /// <exception cref="RecordNotFoundException">If class or session is not found</exception>
        /// <returns></returns>
        Task<ResultPage<ClassRating>> FetchClassRatingsAsync(ClassRatingFilter filter);

        /// <summary>
        ///     Create new class rating
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        Task<ClassRating> CreateClassRatingAsync(ClassRating rating);

        /// <summary>
        ///     Update existing class rating
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        Task<ClassRating> UpdateClassRatingAsync(ClassRating rating);

        /// <summary>
        ///     Upsert Class Radio Station
        /// </summary>
        /// <param name="classRadioStation"></param>
        /// <returns>Result model</returns>
        Task<ClassRadioStationModel> UpsertClassRadioStationAsync(ClassRadioStationModel classRadioStation);

        /// <summary>
        ///     Delete Class Radio Station
        /// </summary>
        /// <param name="classRadioStationId"></param>
        Task DeleteClassRadioStationAsync(Guid classRadioStationId);

        Task<IReadOnlyCollection<ClassTypeModel>> GetUserRecentlyWatchedClassTypesAsync(string userId, int takeCount);

        Task<string> UploadClassChatImageAsync(string localFilePath);
    }
}
