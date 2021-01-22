using System;

namespace forte.models.classes
{
    public class UserSessionOnlineStatusModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public bool IsUserOnline { get; set; }

        public Guid SessionId { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }
}
