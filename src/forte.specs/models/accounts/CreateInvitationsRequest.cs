using System;
using System.Collections.Generic;

namespace forte.models.accounts
{
    public class CreateInvitationsRequest
    {
        /// <summary>
        /// Email addresses of the recipients
        /// </summary>
        public List<string> Emails { get; set; }

        /// <summary>
        /// Specifies whether the invitation will go out anonymously, or from the user
        /// </summary>
        public bool Anonymous { get; set; }

        /// <summary>
        /// For sharing video to friends
        /// </summary>
        public string SharedVideoUrl { get; set; }

        /// <summary>
        /// SessionId for which was invitation for a video
        /// </summary>
        public Guid? SessionId { get; set; }
    }
}
