using System;

namespace forte.models.classes.events
{
    public class UserSessionChatUpdatedEvent : UserSessionEventBase
    {
        public UserSessionChatUpdatedEvent()
        {
            EventType = UserSessionEventType.ChatUpdated;
        }

        //Id of user chat message (used for updates)
        public Guid Id { get; set; }

        public string Message { get; set; }

        public string SenderUserId { get; set; }

        public bool IsMessageRemoved { get; set; }
    }
}
