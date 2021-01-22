using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace forte.services.accounts
{
    public interface IBadgeService
    {
        /// <summary>
        /// The method for register action for userId
        /// </summary>
        /// <param name="actionName">Name of the action</param>
        /// <param name="userId">Identity of the user, who executed the action</param>
        /// <returns></returns>
        Task<List<string>> RegisterActionAndGetNewBadges(string actionName, string userId, DateTime startDate);

        /// <summary>
        /// The method for find all user's badges
        /// </summary>
        /// <param name="userId">Identity of users</param>
        /// <returns>All user's badges</returns>
        Task<List<string>> GetUserBadgeNames(string userId);

        /// <summary>
        /// The method for find badge definition by name
        /// </summary>
        /// <param name="name">Name of badge definition</param>
        /// <returns>Found badge definition or null, if badge definition is not found</returns>
        IBadgeDefinition FindDefinitionByName(string name);
    }
}
