using System;

namespace forte.models.classes
{
    public class ChallengeUserInvitationModel
    {
        public Guid Id { get; set; }

        public Guid ChallengeId { get; set; }

        public string UserId { get; set; }

        public ChallengeInvitationStatus Status { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }
}
