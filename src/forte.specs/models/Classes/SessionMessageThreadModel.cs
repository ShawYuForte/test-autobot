using System;

namespace forte.models.classes
{
    public class SessionMessageThreadModel
    {
        public string ChatName { get; set; }

        public string ChatImgUrl { get; set; }

        public string ChatWithUserId { get; set; }

        public Guid? ChatSessionId { get; set; }

        public string LastMessage { get; set; }

        public DateTime LastMessageDate { get; set; }

        public int NumberOfUnreadMessages { get; set; }
    }
}
