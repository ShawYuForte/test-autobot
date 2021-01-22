using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;

namespace forte.services
{
    public interface IClassSchedulingService : IPublicSessionProviderService
    {
        /// <summary>
        ///     Cancels the reservation spot in the session for the current user.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns><c>true</c> if operation succeeded.</returns>
        Task<bool> CancelReservationSpotAsync(Guid sessionId);

        /// <summary>
        ///     Creates a new session.
        /// </summary>
        /// <param name="sessionModel">The extended session model.</param>
        /// <returns> The extended model of created session. </returns>
        Task<SessionExModel> CreateSessionAsync(SessionExModel sessionModel);

        /// <summary>
        ///     Deletes the class session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="forceDelete">Bypass validation if forceDelete is true.</param>
        Task DeleteSessionAsync(Guid sessionId, bool forceDelete);

        Task ReCreateSessionsAsync();

        /// <summary>
        ///     Deletes the session attendance.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="customerUserId">The attendee customer user ID.</param>
        Task DeleteSessionAttendanceAsync(Guid sessionId, string customerUserId);

        /// <summary>
        ///     Fetch featured sessions. The featured concept changes over time, and includes currently live sessions
        /// </summary>
        /// <returns></returns>
        Task<FeaturedSessionSummaryPage> FetchFeaturedSessionsAsync(bool extended, bool hasVideoUrl = true, bool useAppleTV = true);

        /// <summary>
        ///     Fetch session attendances, including future (reserved) attendances.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResultPage<SessionAttendanceModel>> FetchSessionAttendanceAsync(SessionAttendanceFilter filter);

        /// <summary>
        ///     Get the extended session model by ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The extended model of session. </returns>
        Task<SessionExModel> GetEditableSessionAsync(Guid sessionId);

        /// <summary>
        ///     Get the extended session model by ID for indexing.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The extended model of session. </returns>
        Task<SessionExModel> GetIndexableSessionAsync(Guid sessionId);

        /// <summary>
        ///     Get the extended session model by permalink.
        /// </summary>
        /// <param name="sessionPermalink">The session permalink.</param>
        /// <returns>The extended model of session. </returns>
        Task<SessionExModel> GetEditableSessionAsync(string sessionPermalink);

        /// <summary>
        ///     Get sessions marked as freemium
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SessionPageModel> GetFreeSessionsAsync(SessionRequest request);

        /// <summary>
        ///     Gets the list of sessions with optional filter criteria
        /// </summary>
        /// <param name="request">The request filters.</param>
        /// <returns>The paged session model collection.</returns>
        Task<SessionPageModel> GetSessionsAsync(SessionRequest request);

        Task<Guid?> GetSessionDeviceId(Guid id);

        Task<SessionType?> GetSessionType(Guid id);

        /// <summary>
        ///     Get list of session summaries
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SessionSummaryPage> GetSessionSummariesAsync(
            SessionSummaryResponseType responseType,
            SessionSummariesRequest request);

        /// <summary>
        ///     Gets the list of sessions with optional filter criteria, and caches value
        /// </summary>
        /// <param name="request">The request filters.</param>
        /// <returns>The paged session model collection.</returns>
        Task<SessionPageModel> GetSessionsWithCachingAsync(SessionRequest request);

        /// <summary>
        ///     Gets the sessions list watched by the current user.
        /// </summary>
        /// <returns>The paged watched sessions model collection. </returns>
        Task<WatchedHistoryPageModel> GetWatchedHistoryAsync(WatchedHistoryRequestFilter requestFilter);

        /// <summary>
        ///     Marks the session video as watched.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The watched session model. </returns>
        Task<WatchedSessionModel> MarkAsWatchedAsync(Guid sessionId);

        /// <summary>
        ///     Marks a certain session as featured
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task MarkSessionFeaturedAsync(Guid sessionId);

        /// <summary>
        ///     Reserves a spot in the session for the current user.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns><c>true</c> if operation succeeded.</returns>
        Task<bool> ReserveSpotAsync(Guid sessionId);

        /// <summary>
        ///     Sets free session
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<bool> SetFreeSessionAsync(FreeSessionRequest request);

        /// <summary>
        ///     Set the session to specified status, not to be used by user driven requests, only system
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="sessionStatus"></param>
        /// <param name="changeReason"></param>
        /// <returns></returns>
        Task SetSessionStatusAsync(Guid sessionId, SessionStatus sessionStatus, ChangeTypeDetails changeReason = ChangeTypeDetails.DataUpdated);

        /// <summary>
        ///     Swap two featured request ordinal positions
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task SwapFeaturedSessionOrdinalsAsync(SwapFeaturedSessionsRequest request);

        /// <summary>
        ///     Switches the session attendance for the current user.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <param name="requestModel">The request model.</param>
        /// <returns></returns>
        Task<bool> SwitchSessionAttendanceAsync(Guid sessionId, SwitchAttendanceStatusRequestModel requestModel);

        /// <summary>
        ///     Unmarks a featured session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task UnmarkSessionFeaturedAsync(Guid sessionId);

        /// <summary>
        ///     Updates the session.
        /// </summary>
        /// <param name="sessionModel">The extended session model.</param>
        /// <returns>The extended model of session. </returns>
        Task<SessionExModel> UpdateSessionAsync(SessionExModel sessionModel);

        /// <summary>
        ///     Uploads a session cover file.
        /// </summary>
        /// <param name="localFilePath">The local file path to upload from.</param>
        /// <param name="type">The cover file type (image or video)</param>
        /// <returns>The uploaded file URL.</returns>
        Task<string> UploadSessionCoverFileAsync(string localFilePath, string type, string uid = null);
    }
}
