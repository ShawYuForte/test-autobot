using System;

namespace forte.models.classes.events
{
    public class UserSessionChatMessageAttachmentsUpdatedEvent : UserSessionEventBase
    {
        public UserSessionChatMessageAttachmentsUpdatedEvent()
        {
            EventType = UserSessionEventType.ChatMessageAttachmentsUpdated;
        }

        public Guid MessageId { get; set; }

        public string Url { get; set; }

        public MessageAttachmentType MessageAttachmentType { get; set; }

        public int MessageAttachmentTypeId
        {
            get
            {
                return (int)MessageAttachmentType;
            }

            set
            {
                MessageAttachmentType = (MessageAttachmentType)Enum.ToObject(typeof(MessageAttachmentType), value);
            }
        }

        public string AspectRatio { get; set; }
    }
}
