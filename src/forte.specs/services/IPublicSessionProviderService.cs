using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models.classes;
using forte.models.search;

namespace forte.services
{
    public interface IPublicSessionProviderService
    {
        /// <summary>
        ///     Gets the public session by ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The session model. </returns>
        Task<SessionModel> GetPublicSessionAsync(Guid sessionId);

        /// <summary>
        ///     Gets the public session by permalink.
        /// </summary>
        /// <param name="sessionPermalink">The session permalink.</param>
        /// <returns>The session model. </returns>
        Task<SessionModel> GetPublicSessionAsync(string sessionPermalink);

        Task<List<SearchDocumentEx>> RefineIsFavoriteAsync(List<SearchDocumentEx> sessions);

        Task<List<SearchDocumentEx>> RefineIsReservedAsync(List<SearchDocumentEx> sessions);
    }
}
