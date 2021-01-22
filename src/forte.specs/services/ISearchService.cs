using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models.search;

namespace forte.services
{
    /// <summary>
    ///     Describes the service to access the Forte app search functionality.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        ///     Gets the list of search suggestions.
        /// </summary>
        /// <param name="request">The suggestions request.</param>
        /// <returns>The list of search suggestions.</returns>
        Task<IEnumerable<string>> FetchSuggestionsAsync(SearchSuggestionRequest request);

        /// <summary>
        ///     Index a class entity
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task IndexClassAsync(Guid classId);

        /// <summary>
        ///     Index a session entity
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task IndexSessionAsync(Guid sessionId);

        /// <summary>
        ///     Index a studio entity
        /// </summary>
        /// <param name="studioId"></param>
        /// <returns></returns>
        Task IndexStudioAsync(Guid studioId);

        /// <summary>
        ///     Index a trainer entity
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        Task IndexTrainerAsync(Guid trainerId);

        /// <summary>
        ///     Re-indexes all content by deleting current index, querying the database for the latest, and indexing the latest
        ///     content
        /// </summary>
        /// <returns></returns>
        Task ReindexAllAsync();

        /// <summary>
        ///     Remove class entity index
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        Task RemoveClassIndexAsync(Guid classId);

        /// <summary>
        ///     Remove session entity index
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        Task RemoveSessionIndexAsync(Guid sessionId);

        /// <summary>
        ///     Remove studio entity index
        /// </summary>
        /// <param name="studioId"></param>
        /// <returns></returns>
        Task RemoveStudioIndexAsync(Guid studioId);

        /// <summary>
        ///     Remove trainer entity index
        /// </summary>
        /// <param name="trainerId"></param>
        /// <returns></returns>
        Task RemoveTrainerIndexAsync(Guid trainerId);

        /// <summary>
        ///     Performs the search by specified request parameters.
        /// </summary>
        /// <param name="request">The search request.</param>
        /// <returns>Paged list of search results.</returns>
        Task<SearchResultPage> SearchAsync(SearchRequest request);
    }
}
