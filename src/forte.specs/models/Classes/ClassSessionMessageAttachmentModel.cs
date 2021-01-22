using System;

namespace forte.models.classes
{
    public class ClassSessionMessageAttachmentModel
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }

        public string Url { get; set; }

        public MessageAttachmentType MessageAttachmentType { get; set; }

        public decimal? AspectRatio { get; set; }

        public DateTime Created { get; set; }
    }
}
