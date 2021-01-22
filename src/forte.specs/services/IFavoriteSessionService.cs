using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;

namespace forte.services
{
    public interface IFavoriteSessionService
    {
        Task MarkSessionAsFavoriteForCurrentUserAsync(Guid sessionId);

        Task UnmarkSessionAsFavoriteForCurrentUserAsync(Guid sessionId);

        Task<FavoriteSessionPageModel> GetFavoriteSessionsForCurrentUserAsync(RequestFilter filter);
    }
}
