using System;

namespace forte.models.classes
{
    public class UserActivityCommentModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string ProfileImgUrl { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }

        public Guid UserActivityId { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }
}
