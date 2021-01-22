using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace forte.models
{
    public enum RegisterResult
    {
        None,
        ExternalInfoIsNull,
        Error,
        Success,
    }

    public enum SessionStatus
    {
        /// <summary>
        ///     Event owner has not yet published
        /// </summary>
        Draft = 0,

        /// <summary>
        ///     Event is published but has not occurred yet (for live events)
        /// </summary>
        Scheduled = 1,

        /// <summary>
        ///     Event is live
        /// </summary>
        Live = 2,

        /// <summary>
        ///     Event has occurred in the past and is available now on-demand
        /// </summary>
        OnDemand = 3,

        /// <summary>
        ///     Session is being processed for on-demand consumptions
        /// </summary>
        Processing = 4,

        /// <summary>
        ///     Session has been cancelled
        /// </summary>
        Cancelled = 5,

        /// <summary>
        ///     Event is no longer available on-demand and has been archived
        /// </summary>
        Archived = 6,

        /// <summary>
        ///     Streaming error, invalid session
        /// </summary>
        Error = 7,

        Linked = 100,

        Restarting = 101,
    }

    public enum SessionType
    {
        /// <summary>
        ///     Session is scheduled and is fully automated
        /// </summary>
        Scheduled = 0,

        /// <summary>
        /// Manually entered session (pre-recorded, or recorded through other means)
        /// </summary>
        Manual = 1,

        Portable = 2,
    }

    public enum ClassStatus
    {
        /// <summary>
        ///     Class owner has not yet published
        /// </summary>
        Draft = 0,

        /// <summary>
        ///     Class is published
        /// </summary>
        Published = 1,

        /// <summary>
        ///     Class is deleted
        /// </summary>
        Deleted = 2,
    }

    public enum TrainerStatus
    {
        /// <summary>
        ///     Trainer profile is offline, associated streaming videos will not be watchable by users
        /// </summary>
        Offline = 0,

        /// <summary>
        ///     Trainer profile is online
        /// </summary>
        Online = 1,
    }

    public enum AuditAction
    {
        UserLock = 0,
        UserUnlock = 1,
        UserLogin = 2,
        UserSiteVisit = 3,
    }

    public enum StudioStatus
    {
        /// <summary>
        ///     The studio is created but not approved yet by admin
        /// </summary>
        PendingApproval = 0,

        /// <summary>
        ///     The studio is approved by admin and published
        /// </summary>
        Published = 1,

        /// <summary>
        ///     The studio is rejected by admin
        /// </summary>
        Rejected = 2,

        /// <summary>
        ///     The studio is suspended
        /// </summary>
        Suspended = 3,
    }

    /// <summary>
    ///     Specifies what kind of model the resource API returns
    /// </summary>
    public enum ModelType
    {
        /// <summary>
        ///     Partial model, useful for public consumption, reads, and queries
        /// </summary>
        Partial,

        /// <summary>
        ///     Complete model, useful for editing
        /// </summary>
        Complete,
    }

    public enum GroupType
    {
        /// <summary>
        ///     The standard claims group
        /// </summary>
        Standard = 0,

        /// <summary>
        ///     The system claims group
        /// </summary>
        System = 1,
    }

    public enum EventAttendanceStatus
    {
        // These statuses are not involved to the app logic now as payments are not yet implemented. Might need to be reviewed and cleaned up.
        None = 0,
        Paid = 1,
        Attended = 2,
        Refunded = 3,
        // These statuses are used and related to customer attendance
        Attending = 4,
        SittingOut = 5,
    }

    public enum EventAttendanceType
    {
        /// <summary>
        ///     Attendance during the live stream
        /// </summary>
        Live = 0,

        /// <summary>
        ///     Attendance after the live event, on-demand
        /// </summary>
        OnDemand = 1,

        /// <summary>
        ///     A fan that has not attended the event, but has been following and/or sharing
        /// </summary>
        Follower = 2,
    }

    public enum DataChanges
    {
        Create,
        Update,
        Delete,
        Restore,
        VideoUploadFinalize,
    }

    public enum ChangeTypeDetails
    {
        DataUpdated,
        StreamStarted,
        StreamStopped,
        ManualStreamFlow,
    }

    public enum SessionSummaryResponseType
    {
        Live,
        Upcoming,
        Trending,
        OnDemand,
        Watched,
        Reserved,
        Completed,
        Library,
    }

    public enum UserActivityType
    {
        /// <summary>
        ///     Class Completed
        /// </summary>
        ClassCompleted,

        /// <summary>
        ///     Added To Crew
        /// </summary>
        AddedToCrew,

        /// <summary>
        ///     Class Shared
        /// </summary>
        ClassShared,

        /// <summary>
        ///     Badge Earned
        /// </summary>
        BadgeEarned,

        /// <summary>
        ///     Message Shared
        /// </summary>
        MessageShared,

        /// <summary>
        ///     User High Five
        /// </summary>
        UserHighFive,
    }

    public enum ChallengeType
    {
        /// <summary>
        ///     Running challenge
        /// </summary>
        [Description("Running")]
        Running,
    }

    public enum ChallengeInvitationStatus
    {
        /// <summary>
        ///     Invite sent
        /// </summary>
        Sent,

        /// <summary>
        ///     Invite accepted
        /// </summary>
        Accepted,

        /// <summary>
        ///     Invite declined
        /// </summary>
        Declined,
    }

    public enum MessageAttachmentType
    {
        /// <summary>
        ///     Image
        /// </summary>
        Image,
    }

    public enum UserSessionSetting
    {
        /// <summary>
        ///     User Audio Muted
        /// </summary>
        UserAudioMuted,

        /// <summary>
        ///     User Video Muted
        /// </summary>
        UserVideoMuted,

        /// <summary>
        ///     User Blocked
        /// </summary>
        UserBlocked,
    }

    public enum SessionSetting
    {
        /// <summary>
        ///     Class Audio Muted
        /// </summary>
        ClassAudioMuted,
    }

    public static class EnumExtensions
    {
        public static IReadOnlyCollection<EnumDescriptionModel> ConvertToEnumDescriptionItems<TEnum>()
            where TEnum : struct
        {
            if (typeof(TEnum).IsEnum)
            {
                var result = Enum.GetValues(typeof(TEnum))
                    .Cast<TEnum>()
                    .Select(v =>
                    {
                        var memberInfo = typeof(TEnum).GetMember(v.ToString());
                        var description = (DescriptionAttribute)memberInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
                        var item = new EnumDescriptionModel
                        {
                            Value = v.ToString(),
                            Description = description?.Description ?? v.ToString(),
                        };

                        return item;
                    })
                    .ToList().AsReadOnly();

                return result;
            }

            return new EnumDescriptionModel[0];
        }
    }
}
