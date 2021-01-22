using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace forte.models.classes.events
{
    public enum UserSessionEventType
    {
        ScoresUpdated,
        ChatUpdated,
        UserDisconnected,
        ChatMessageAttachmentsUpdated,
    }

    public abstract class UserSessionEventBase : TableEntity
    {
        public DateTime Created { get; set; }

        public Guid SessionId { get; set; }

        public UserSessionEventType EventType { get; set; }

        public int UserSessionEventTypeId
        {
            get
            {
                return (int)EventType;
            }

            //need setter for storage
            set
            {
                EventType = (UserSessionEventType)Enum.ToObject(typeof(UserSessionEventType), value);
            }
        }
    }
}
