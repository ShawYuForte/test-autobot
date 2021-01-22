using System;
using System.Collections.Generic;

namespace forte.models.accounts
{
    public class CreateStudioInvitationsRequest
    {
        /// <summary>
        ///     Invited friends
        /// </summary>
        public IList<StudioInvitedFriendModel> Friends { get; set; }

        /// <summary>
        ///     Inviter First Name
        /// </summary>
        public string InviterFirstName { get; set; }

        /// <summary>
        ///     Inviter Last Name
        /// </summary>
        public string InviterLastName { get; set; }

        /// <summary>
        ///     Inviter Email
        /// </summary>
        public string InviterEmail { get; set; }

        /// <summary>
        ///     For sharing video to friends
        /// </summary>
        public string SharedVideoUrl { get; set; }

        /// <summary>
        /// SessionId for which was invitation for a video
        /// </summary>
        public Guid? SessionId { get; set; }
    }
}
