using System;

namespace forte.models.auth
{
    /// <summary>
    ///     Defines the structure of the application user. A user is identified as the authenticated
    ///     context within the application. The user may be a system, admin, customer, or other type
    ///     of context.
    /// </summary>
    public interface IApplicationUser
    {
        /// <summary>
        ///     Used to record failures for the purposes of lockout
        /// </summary>
        int AccessFailedCount { get; set; }

        string AgeGroup { get; set; }

        /// <summary>
        ///     The number of invitations this user is allowed to send
        /// </summary>
        int AllowedInvitations { get; set; }

        string City { get; set; }

        DateTime Created { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        string Email { get; set; }

        /// <summary>
        ///     True if the email is confirmed, default is false
        /// </summary>
        bool EmailConfirmed { get; set; }

        string FirstName { get; set; }

        string FitnessLevel { get; set; }

        string Gender { get; set; }

        /// <summary>
        ///     User ID (Primary Key)
        /// </summary>
        string Id { get; set; }

        string LastName { get; set; }

        /// <summary>
        ///     DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        ///     PhoneNumber for the user
        /// </summary>
        string PhoneNumber { get; set; }

        /// <summary>
        ///     True if the phone number is confirmed, default is false
        /// </summary>
        bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        ///     Registration code used by the user to create an account
        /// </summary>
        string RegistrationCode { get; set; }

        string State { get; set; }

        string LockoutReasonCode { get; set; }

        /// <summary>
        ///     User name
        /// </summary>
        string UserName { get; set; }

        string ZipCode { get; set; }
    }
}
