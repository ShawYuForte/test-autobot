using System;

namespace forte.models.accounts
{
    public enum InvitationStatuses
    {
        /// <summary>
        ///     Invitation has been created, but not sent
        /// </summary>
        [CharCode('N')]
        Created,

        /// <summary>
        ///     Invitation has been sent, referral created
        /// </summary>
        [CharCode('S')]
        Sent,

        /// <summary>
        ///     Invitation has been accepted
        /// </summary>
        [CharCode('A')]
        Accepted,

        /// <summary>
        ///     Invitation is complete, benefit has been obtained (if one was eligible)
        /// </summary>
        [CharCode('C')]
        Complete,

        /// <summary>
        ///     Invitation is incomplete for several reasons
        /// </summary>
        [CharCode('I')]
        Incomplete,
    }

    public class InvitationModel
    {
        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Email address of recipient. If no email address specified, any email address can use it
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Invitation unique code
        /// </summary>
        public string RegistrationCode { get; set; }

        /// <summary>
        ///     Recipient's name
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        ///     Resend invitation, if the code already exists for the specified email
        /// </summary>
        public bool? ResendIfExisting { get; set; }

        /// <summary>
        ///     Invitation status code
        /// </summary>
        public InvitationStatuses Status { get; set; }

        /// <summary>
        ///     The reason behind the current status
        /// </summary>
        public string StatusReason { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Is Studio Beneficiar
        /// </summary>
        public bool IsStudioBeneficiar { get; set; }

        /// <summary>
        /// Studio Manager user that sent invitation
        /// </summary>
        public string StudiManagerUserId { get; set; }

        /// <summary>
        /// SessionId for which was invitation for a video
        /// </summary>
        public Guid? SessionId { get; set; }
    }
}
