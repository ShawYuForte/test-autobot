using System;
using System.ComponentModel.DataAnnotations;
using forte.models.trainers;

namespace forte.models.classes
{
    /// <summary>
    ///     The public session model
    /// </summary>
    public class SessionModel
    {
        /// <summary>
        ///     Class this session belongs to
        /// </summary>
        public ClassModel Class { get; set; }

        /// <summary>
        ///     UTC date and time when this entity record was created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Created by userId
        /// </summary>
        /// <summary>
        ///     Created by UserFullName. To avoid nested queries later?
        /// </summary>
        public string CreatedByUserFullName { get; set; }

        /// <summary>
        ///     Event duration in minutes
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        ///     Time when it was edited
        /// </summary>
        public DateTime? EditedOn { get; set; }

        /// <summary>
        ///     Event end time in UTC
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        ///     The stored version e-tag
        /// </summary>
        public string ETag { get; set; }

        /// <summary>
        ///     Entity record identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this session is reserved by the current user.
        /// </summary>
        public bool IsReserved { get; set; }

        /// <summary>
        ///     The session permalink
        /// </summary>
        public string Permalink { get; set; }

        /// <summary>
        ///     Time when it whas published
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        ///     Session type
        /// </summary>
        public SessionType Type { get; set; }

        /// <summary>
        ///     Event start time in UTC
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        ///  Free sessions start date
        /// </summary>
        public DateTime? FreeStartDate { get; set; }

        /// <summary>
        ///     Is feed fm radio enabled for the session
        /// </summary>
        public bool FeedFmEnabled { get; set; }

        /// <summary>
        ///     Is 2-way video disabled for the session
        /// </summary>
        public bool VideoSharingDisabled { get; set; }

        /// <summary>
        ///     Is 2-way video disabled for the session
        /// </summary>
        public bool DoNotConvertToOnDemand { get; set; }

        /// <summary>
        ///     Event status
        /// </summary>
        [Required]
        [Display(Name = "Session Status")]
        public SessionStatus Status { get; set; }

        /// <summary>
        ///     Session trainer
        /// </summary>
        public TrainerExModel Trainer { get; set; }

        /// <summary>
        ///     For manual sessions, this field stores the url to the streaming video
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        ///     Contains link to video, that was uploaded directly to media account, repeats video url field
        /// </summary>
        public string SessionUrl { get; set; }

        /// <summary>
        ///     count of Accepted Invitations
        /// </summary>
        public int Accepted { get; set; }

        /// <summary>
        ///     count of Sent Invitations
        /// </summary>
        public int Sent { get; set; }

        public bool IsFavorite { get; set; }

        public string PresetName { get; set; }

        public short? DeviceType { get; set; }

        public string PrimaryIngestUrl { get; set; }

        public string PrimaryIngestKey { get; set; }
    }
}
