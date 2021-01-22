using System;
using System.Collections.Generic;

namespace forte.models.classes
{
    public class ChallengeModel
    {
        public ChallengeModel()
        {
            Sessions = new List<ChallengeSessionModel>();
            UserInvitations = new List<ChallengeUserInvitationModel>();
            Users = new List<ChallengeUserModel>();
        }

        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public ChallengeType ChallengeType { get; set; }

        public bool IsPublic { get; set; }

        public bool SendAlerts { get; set; }

        public string CreatedByUserId { get; set; }

        public List<ChallengeSessionModel> Sessions { get; set; }

        public List<ChallengeUserInvitationModel> UserInvitations { get; set; }

        public List<ChallengeUserModel> Users { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }
}
