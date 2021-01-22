using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models.classes;

namespace forte.services
{
    public interface IUserClassLeaderBoardService
    {
        Task<IReadOnlyCollection<UserSessionScoreModel>> GetActiveUserSessionScoresAsync(Guid sessionId, string[] includeUserIds);

        Task<UserSessionScoreModel> UpsertUserSessionScoreAsync(UpdateUserSessionScoreModel userSessionScore);

        Task<UserSessionOnlineStatusModel> GetUserSessionOnlineStatusAsync(string userId, Guid sessionId);

        Task<UserSessionOnlineStatusModel> UpsertUserSessionOnlineStatusAsync(UserSessionOnlineStatusModel userSessionOnlineStatus);

        Task<IReadOnlyCollection<string>> GetUserCrewMembersAsync(string userId);

        Task<UserClassCrewModel> AddUserClassCrewMemberAsync(string userId, string crewUserId);

        Task RemoveUserClassCrewMemberAsync(string userId, string crewUserId);

        Task<IReadOnlyCollection<UserSessionScoreInfo>> GetUserSessionScoreInfoAsync(string[] userIds);

        Task<IReadOnlyCollection<UserProfileCrewMember>> GetUserCrewMembersExtendedAsync(string userId);

        Task<bool> IsUserOnlineAsync(string userId, Guid sessionId);

        Task UpdateUserSessionOnlineStatusAsync(Guid sessionId, string userId, bool status);

        Task<IReadOnlyCollection<string>> GetLinkedCrewUsersAsync(string userId);

        Task<IReadOnlyCollection<SessionInstructorSettingModel>> GetSessionInstructorSettingsAsync(Guid sessionId);

        Task<SessionInstructorSettingModel> UpsertSessionInstructorSettingAsync(SessionInstructorSettingModel sessionInstructorSetting);

        Task<UserSessionInstructorSettingModel> UpsertUserSessionInstructorSettingAsync(UserSessionInstructorSettingModel userSessionInstructorSetting);
    }
}
