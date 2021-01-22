using System;

namespace forte.models.classes
{
    public class ClassSessionMessageLikeModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public Guid MessageId { get; set; }

        public string UserImgUrl { get; set; }

        public string NameOfUser { get; set; }

        public string NicknameOfUser { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }
}
