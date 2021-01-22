using System;
using System.Threading.Tasks;
using forte.models.accounts;

namespace forte.services
{
    public interface IPartnerService
    {
        /// <summary>
        /// The method creates user account with Source Partner Identifier and sends invitation email for fill form and set up password
        /// </summary>
        /// <param name="email">email of new user</param>
        /// <param name="partnerId">identifier of partner, which be used as partner id</param>
        /// <returns></returns>
        Task<string> RegisterUserFromPartner(CreateUserFromPartnerRequest createUserRequest, Guid partnerId);

        /// <summary>
        /// The method sends email of successful registration with request of enter the password to forte.fit
        /// </summary>
        /// <param name="userId">identity of the user</param>
        /// <param name="resetToken">generated reset token</param>
        /// <returns></returns>
        Task SendPasswordEnterAsync(string userId, string resetToken);

        Task<bool> IsUserManagedByPartner(string userId, Guid partnerId);

        Task UpdateUserClaims(string userId);

        Task<AccountUtilization> GetAccountUtilization(string userId, DateTime startDate);
    }
}
