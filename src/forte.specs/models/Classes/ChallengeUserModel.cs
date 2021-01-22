using System;

namespace forte.models.classes
{
    public class ChallengeUserModel : ChallengeUserDataModel
    {
        public Guid Id { get; set; }

        public Guid ChallengeId { get; set; }

        public DateTime Created { get; set; }
    }
}
