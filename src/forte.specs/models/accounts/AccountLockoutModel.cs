using System;

namespace forte.models.accounts
{
    public enum AccountLockoutReasons
    {
        /// <summary>
        /// Account is not locked
        /// </summary>
        None = 0,

        /// <summary>
        /// Account locked due to too many invalid attempts
        /// </summary>
        [CharCode('L')]
        InvalidLoginAttempts,

        /// <summary>
        /// Account has been suspended
        /// </summary>
        [CharCode('S')]
        AccountSuspended,
    }

    public class AccountLockoutModel
    {
        public DateTime? LockoutEndDateUtc { get; set; }

        public AccountLockoutReasons LockoutReason { get; set; }

        public string UserId { get; set; }
    }
}
