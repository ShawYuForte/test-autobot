using System;

namespace forte.models.accounts
{
    public enum InvitationTypes
    {
        [CharCode('U')]
        UserReferral,
    }

    public class InvitationExModel : InvitationModel
    {
        /// <summary>
        ///     Invitation unique code identifier
        /// </summary>
        public Guid? RegistrationCodeId { get; set; }

        /// <summary>
        ///     Full name of the sender
        /// </summary>
        public string SenderFullName { get; set; }

        /// <summary>
        ///     User Id that sent the invitation
        /// </summary>
        public string SenderUserId { get; set; }

        /// <summary>
        ///     Invitation type
        /// </summary>
        public InvitationTypes Type { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }
    }
}
