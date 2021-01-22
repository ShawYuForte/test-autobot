using System.Threading.Tasks;

namespace forte.services
{
    /// <summary>
    /// The IUserManager interface provides
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Hash a password
        /// </summary>
        /// <param name="password">Password to hash</param>
        /// <returns>Hashed password</returns>
        string HashPassword(string password);

        /// <summary>
        /// Generate a password reset token for the user
        /// </summary>
        /// <param name="userId">User id for which to generate a reset token</param>
        /// <returns>Reset token string</returns>
        Task<string> GeneratePasswordResetTokenAsync(string userId);
    }
}
