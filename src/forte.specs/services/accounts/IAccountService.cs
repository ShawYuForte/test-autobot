using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.models;
using forte.models.accounts;
using forte.models.auth;
using forte.models.classes;
using Forte.Svc.Services.Models.Accounts;

namespace forte.services.accounts
{
    public interface IAccountService
    {
        /// <summary>
        ///     Creates a new claim group.
        /// </summary>
        /// <param name="groupModel">The claim group model.</param>
        /// <param name="extended">Return extended claim group model if set to <c>true</c>.</param>
        /// <param name="addedClaims">The list of claims to add.</param>
        /// <returns>The created claim group model.</returns>
        GroupModel CreateGroup(GroupExModel groupModel, bool? extended, List<ClaimDefinitionModel> addedClaims);

        /// <summary>
        ///     Creates a new user and send a password reset email.
        /// </summary>
        /// <param name="userManager">User manager implementation instance.</param>
        /// <param name="userModel">The user model.</param>
        /// <param name="extended">Return extended model if set to <c>true</c>.</param>
        /// <param name="addedGroups">The list of claim group IDs to add.</param>
        /// <returns>The created user model.</returns>
        Task<UserModel> CreateUserAsync(
            IUserManager userManager,
            UserExModel userModel,
            bool? extended,
            List<GroupExModel> addedGroups);

        /// <summary>
        ///     Deletes the exsiting claim group.
        /// </summary>
        /// <param name="groupId">The claim group identifier.</param>
        void DeleteGroup(Guid groupId);

        /// <summary>
        ///     Ensure the user account is a freemium account. If the account is already a freemium account, no action is taken.
        ///     Otherwise user groups are changed and the user is notified (if configured to do so)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task EnsureFreemiumUserAccount(string userId);

        /// <summary>
        ///     Ensure the user account is a premium account. If the account is already a premium account, no action is taken.
        ///     Otherwise user groups are changed and the user is notified (if configured to do so)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task EnsurePremiumUserAccount(string userId);

        /// <summary>
        ///     Find a specific user based on specified criteria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserModel> FindUserAsync(FindUserRequest request);

        /// <summary>
        ///     Retrieve account lockout info for user with specified email. Lockout object will be returned even if account not
        ///     locked
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<AccountLockoutModel> GetAccountLockoutInfoAsync(string email);

        /// <summary>
        ///     Get an individual application user record
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IApplicationUser> GetApplicationUserAsync(string userId);

        /// <summary>
        ///     Get an individual application user record by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Application user</returns>
        Task<IApplicationUser> GetApplicationUserByEmailAsync(string email);

        /// <summary>
        ///     Gets the claims definitions.
        /// </summary>
        /// <param name="skip">The number of items to skip.</param>
        /// <param name="take">The number of items to take.</param>
        /// <param name="groupId">The claim group identifier.</param>
        /// <returns> The claims page result. </returns>
        ResultPage<ClaimDefinitionModel> GetClaimDefinitions(int skip, int take, Guid? groupId);

        /// <summary>
        ///     Gets the claim group by ID.
        /// </summary>
        /// <param name="groupId">The claim group identifier.</param>
        /// <param name="extended">Return extended claim group model if set to <c>true</c>.</param>
        /// <returns>The existing claim group model. </returns>
        GroupModel GetGroup(Guid groupId, bool? extended);

        /// <summary>
        ///     Gets the claim groups.
        /// </summary>
        /// <param name="skip">The number of items to skip.</param>
        /// <param name="take">The number of items to take.</param>
        /// <returns>The claim groups page result.</returns>
        ResultPage<GroupExModel> GetGroups(int skip, int take);

        /// <summary>
        ///     Retrieve array of login provider names for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ResultPage<string>> GetUserLoginProvidersAsync(string userId);

        /// <summary>
        ///     Get an individual application user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="extended">Return extended model if set to <c>true</c>.</param>
        /// <returns>The user.</returns>
        Task<UserModel> GetUserProfileAsync(string userId, bool? extended);

        /// <summary>
        ///     Gets page of system users.
        /// </summary>
        /// <returns>The users page result.</returns>
        Task<ResultPage<UserModel>> GetUsersAsync(UserRequest request);

        /// <summary>
        ///     Sends the password reset email to the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="resetToken">The password reset token.</param>
        /// <param name="subject">The email subject to override the default.</param>
        Task SendPasswordResetAsync(string userId, string resetToken, bool simplifiedEmail, string subject = null);

        /// <summary>
        ///     Send welcome email.
        /// </summary>
        /// <returns></returns>
        Task SendWelcomeEmailAsync(string userId);

        /// <summary>
        ///     Suspends the specified user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="userVersion">The version concurrency field.</param>
        /// <returns>The updated user.</returns>
        Task<UserExModel> SuspendUserAsync(string userId, byte[] userVersion);

        /// <summary>
        ///     Unsuspends the specified user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="userVersion">The version concurrency field.</param>
        /// <returns>The updated user.</returns>
        Task<UserExModel> UnsuspendUserAsync(string userId, byte[] userVersion);

        /// <summary>
        ///     Updates the exsiting claim group.
        /// </summary>
        /// <param name="groupModel">The claim group model.</param>
        /// <param name="extended">Return extended model if set to <c>true</c>.</param>
        /// <param name="addedClaims">The list of claims to add.</param>
        /// <param name="removedClaims">The list of claims to remove.</param>
        /// <returns>The updated claim group model.</returns>
        GroupModel UpdateGroup(
            GroupExModel groupModel,
            bool? extended,
            List<ClaimDefinitionModel> addedClaims,
            List<ClaimDefinitionModel> removedClaims);

        /// <summary>
        ///     Updates the exsiting user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <param name="extended">Return extended model if set to <c>true</c>.</param>
        /// <param name="addedGroups">The list of claim group IDs to add.</param>
        /// <param name="removedGroups">The list of claim group IDs to remove.</param>
        /// <param name="addedClassTypes">The list of class types IDs to add</param>
        /// <param name="removedClassTypes">The list of class types IDs to remove</param>
        /// <returns>The updated user model.</returns>
        Task<UserModel> UpdateUserAsync(
            UserExModel userModel,
            bool? extended,
            List<GroupExModel> addedGroups,
            List<GroupExModel> removedGroups,
            List<ClassTypeModel> addedClassTypes,
            List<ClassTypeModel> removedClassTypes);

        /// <summary>
        ///     Updates the user image url fields.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="headerImageUrl">The user header image url.</param>
        /// <param name="thumbnailImageUrl">The user thumbnail image url.</param>
        /// <param name="userVersion">The version concurrency field.</param>
        /// <returns>The updated user.</returns>
        UserExModel UpdateUserImageUrls(string userId, string headerImageUrl, string thumbnailImageUrl, byte[] userVersion);

        /// <summary>
        ///     Updates the user profile.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <param name="addedClassTypes">The list of class types IDs to add</param>
        /// <param name="removedClassTypes">The list of class types IDs to remove</param>
        /// <returns>The updated user.</returns>
        Task<UserExModel> UpdateUserProfileAsync(UserExModel userModel, List<ClassTypeModel> addedClassTypes, List<ClassTypeModel> removedClassTypes);

        /// <summary>
        ///     Upload a profile image file.
        /// </summary>
        /// <param name="localFilePath">The local image file path.</param>
        /// <param name="type">The image file type (header or profile)</param>
        /// <returns></returns>
        Task<string> UploadAccountImageAsync(string localFilePath, string type);

        Task<ValidicUserInfoEx> SaveValidicUser(ValidicUserInfoEx validicUserInfoEx);

        Task UpdateStravaAuthToken(string userId, string token);

        Task UpdateStravaAutoPosting(string userId, bool stravaAutoPosting);

        Task UpdateLastLoginDateAsync(string id);

        Task<ResultPage<UserLite>> GetLinkToInstructorUsersAsync(int skip, int take);
    }
}
