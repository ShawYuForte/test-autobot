using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace forte.models.classes
{
    public class UpsertClassSessionMessageModel
    {
        public UpsertClassSessionMessageModel()
        {
            Attachments = new List<ClassSessionMessageAttachmentModel>();
        }

        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     The stored record version, used for optimistic concurrency
        /// </summary>
        public byte[] Version { get; set; }

        /// <summary>
        /// The sent message text
        /// </summary>
        [MaxLength(5000)]
        public string Message { get; set; }

        /// <summary>
        /// The sender`s User Id
        /// </summary>
        [Required]
        public string SenderUserId { get; set; }

        /// <summary>
        /// The receiver User Id
        /// </summary>
        public string SentToUserId { get; set; }

        /// <summary>
        /// The session id which this attribute belongs to
        /// </summary>
        public Guid? SessionId { get; set; }

        public List<ClassSessionMessageAttachmentModel> Attachments { get; set; }
    }

    public class ClassSessionMessageModel : UpsertClassSessionMessageModel
    {
        public ClassSessionMessageModel()
        {
            ClassSessionMessageLikes = new List<ClassSessionMessageLikeModel>();
            ReadClassSessionMessages = new List<ReadClassSessionMessageModel>();
        }

        /// <summary>
        /// The first/last names of sender
        /// </summary>
        public string SenderUserName { get; set; }

        /// <summary>
        /// The nick name of sender
        /// </summary>
        public string SenderNickname { get; set; }

        public List<ClassSessionMessageLikeModel> ClassSessionMessageLikes { get; set; }

        public List<ReadClassSessionMessageModel> ReadClassSessionMessages { get; set; }

        /// <summary>
        /// The avatar url of sender
        /// </summary>
        public string SenderImgUrl { get; set; }
    }
}
