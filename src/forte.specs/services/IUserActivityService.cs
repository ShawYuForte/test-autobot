using System;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;

namespace forte.services
{
    public interface IUserActivityService
    {
        Task<UserActivityModel> GetUserActivityByIdAsync(Guid userActivityId);

        Task<UserActivityModel> GetUserActivityByLikeIdAsync(Guid userActivityLikeId);

        Task<UserActivityModel> GetUserActivityByCommentIdAsync(Guid userActivityCommentId);

        Task<ResultPage<UserActivityModel>> GetUserActivityPagedAsync(string userId, int skip, int take);

        Task<ResultPage<UserActivityModel>> GetUserCrewMembersActivityPagedAsync(string userId, int skip, int take);

        Task<UserActivityModel> CreateUserActivityAsync(CreateUserActivityModel model);

        Task MarkUserActivitiesAsReadAsync(Guid[] ids);

        Task<UserActivityLikeModel> CreateUserActivityLikeAsync(Guid userActivityId, string userId);

        Task RemoveUserActivityLikeAsync(Guid userActivityLikeId);

        Task RemoveUserActivityCommentAsync(Guid userActivityCommentId);

        Task<UserActivityCommentModel> UpsertUserActivityCommentAsync(UserActivityCommentModel model);
    }
}
