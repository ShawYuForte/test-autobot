using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;
using forte.models.studios;
using Forte.Svc.Services.Models.Studios;

namespace forte.services
{
    /// <summary>
    /// Interface IStudioService
    /// </summary>
    public interface IStudioService
    {
        /// <summary>
        /// Gets the list of studios with optional filter criteria.
        /// </summary>
        /// <param name="request">Request filter and paging parameters.</param>
        /// <returns>The paged list of studios.</returns>
        ResultPage<StudioModel> GetStudios(StudioRequest request);

        /// <summary>
        /// Gets the public studio.
        /// </summary>
        /// <param name="studioId">The studio identifier.</param>
        /// <returns>The studio model.</returns>
        StudioModel GetPublicStudio(Guid studioId);

        /// <summary>
        /// Gets the public studio.
        /// </summary>
        /// <param name="studioPermalink">The studio permalink.</param>
        /// <returns>The studio model.</returns>
        StudioModel GetPublicStudio(string studioPermalink);

        /// <summary>
        /// Gets studio.
        /// </summary>
        /// <param name="studioId">The studio identifier.</param>
        /// <returns>EditableStudioModel.</returns>
        StudioExModel GetEditableStudio(Guid studioId);

        /// <summary>
        /// Gets studio.
        /// </summary>
        /// <param name="studioPermalink">The studio permalink.</param>
        /// <returns>EditableStudioModel.</returns>
        StudioExModel GetEditableStudio(string studioPermalink);

        /// <summary>
        /// Updates the studio.
        /// </summary>
        /// <param name="request">Editable Class Model.</param>
        /// <returns>The updated studio.</returns>
        Task<StudioExModel> UpdateStudioAsync(EditStudioRequestModel request);

        /// <summary>
        /// Suspends the specified studio.
        /// </summary>
        /// <param name="studioId">The studio ID.</param>
        /// <param name="studioVersion">The version concurrency field.</param>
        /// <returns>The updated studio.</returns>
        Task<StudioExModel> SuspendStudioAsync(Guid studioId, byte[] studioVersion);

        /// <summary>
        /// Unsuspends the specified studio.
        /// </summary>
        /// <param name="studioId">The studio ID.</param>
        /// <param name="studioVersion">The version concurrency field.</param>
        /// <returns>The updated studio.</returns>
        Task<StudioExModel> UnsuspendStudioAsync(Guid studioId, byte[] studioVersion);

        /// <summary>
        /// Updates the studio image url fields.
        /// </summary>
        /// <param name="studioId">The studio ID.</param>
        /// <param name="headerImageUrl">The studio header image url.</param>
        /// <param name="thumbnailImageUrl">The studio thumbnail image url.</param>
        /// <param name="studioVersion">The version concurrency field.</param>
        /// <returns>The updated studio.</returns>
        StudioExModel UpdateStudioImageUrls(Guid studioId, string headerImageUrl, string thumbnailImageUrl, byte[] studioVersion);

        /// <summary>
        /// Creates the studio.
        /// </summary>
        /// <param name="request">Editable Class Model.</param>
        /// <returns>The created studio.</returns>
        Task<StudioExModel> CreateStudioAsync(EditStudioRequestModel request);

        /// <summary>
        /// Deletes the studio.
        /// </summary>
        /// <param name="studioId">The studio identifier.</param>
        Task DeleteStudioAsync(Guid studioId);

        /// <summary>
        /// Gets sessions for Studio
        /// </summary>
        /// <param name="studioId">The studio identifier.</param>
        /// <returns>PublicSessionPageModel.</returns>
        PublicClassTrainerPageModel GetPublicClassesForStudio(Guid studioId);

        /// <summary>
        /// Upload a studio image
        /// </summary>
        /// <param name="localFileName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<string> UploadStudioImageAsync(string localFileName, string type);

        /// <summary>
        /// Approves the draft studio.
        /// </summary>
        /// <param name="studioId">The studio ID.</param>
        /// <returns>The approved studio.</returns>
        Task<StudioModel> ApproveStudioAsync(Guid studioId);

        /// <summary>
        /// Rejects the draft studio.
        /// </summary>
        /// <param name="studioId">The studio ID.</param>
        /// <param name="reason">The rejection reason.</param>
        /// <returns>The rejected studio.</returns>
        Task<StudioModel> RejectStudioAsync(Guid studioId, string reason);

        /// <summary>
        /// Gets a list of city names
        /// </summary>
        /// <param name="request">The request filter</param>
        /// <returns>Paged list of city names</returns>
        ResultPage<string> GetCities(RequestFilter request);

        /// <summary>
        /// Updates the studio's next live session info. One of the Id parameters needs to have a value.
        /// </summary>
        /// <param name="studioId">Studio Id to update</param>
        /// <param name="classId">Class Id related to studio to update</param>
        /// <param name="sessionId">Session Id related to studio to update</param>
        void UpdateStudioNextLiveSessionInfo(Guid? studioId, Guid? classId, Guid? sessionId);
    }
}
