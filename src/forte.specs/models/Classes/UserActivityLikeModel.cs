using System;

namespace forte.models.classes
{
    public class UserActivityLikeModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string ProfileImgUrl { get; set; }

        public string UserName { get; set; }

        public Guid UserActivityId { get; set; }

        public DateTime Created { get; set; }
    }
}
