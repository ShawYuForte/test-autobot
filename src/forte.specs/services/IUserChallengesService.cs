using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models;
using forte.models.classes;

namespace forte.services
{
    public interface IUserChallengesService
    {
        Task<ResultPage<ChallengeModel>> GetActiveChallengesPagedAsync(string userId, int skip, int take);

        Task<ResultPage<ChallengeModel>> GetUpcomingChallengesPagedAsync(string userId, int skip, int take);

        Task<ChallengeModel> UpsertChallengeAsync(ChallengeModel model);

        Task DeleteChallengeAsync(Guid challengeId);

        Task<IReadOnlyCollection<ChallengeUserDataModel>> GetSuggestedChallengeUsersAsync(string userId);

        Task<ChallengeUserInvitationModel> UpsertChallengeUserInvitationAsync(ChallengeUserInvitationModel model);

        Task UpdateChallengeUserInvitationStatusAsync(Guid invitationId, ChallengeInvitationStatus status);

        Task<ChallengeUserModel> CreateChallengeUserAsync(ChallengeUserModel model);

        Task<IReadOnlyCollection<ChallengeSessionModel>> GetSuggestedChallengeSessionsByChallengeTypeAsync(ChallengeType challengeType);
    }
}
