using System;

namespace forte.models.classes
{
    public class ChallengeSessionModel
    {
        public Guid Id { get; set; }

        public int Duration { get; set; }

        public string ImgUrl { get; set; }

        public string ClassName { get; set; }

        public string CoachName { get; set; }

        public Guid ClassTypeId { get; set; }

        public string ClassType { get; set; }
    }
}
