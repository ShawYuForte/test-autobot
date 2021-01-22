using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace forte.models.classes
{
    public class UpdateUserSessionScoreModel
    {
        public DateTime Created { get; set; }

        [Required]
        public Guid? Id { get; set; }

        public string UserId { get; set; }

        public int ScoreValue { get; set; }

        [Required]
        public Guid? SessionId { get; set; }

        [Required]
        public Guid? WatchedSessionId { get; set; }

        public bool IsUserOnline { get; set; }

        public byte[] Version { get; set; }

        public int AllSessionsTotalScores { get; set; }
    }

    public class UserSessionScoreModel : UserSessionScoreInfo
    {
        public DateTime Created { get; set; }

        public Guid Id { get; set; }

        public int ScoreValue { get; set; }

        public Guid SessionId { get; set; }

        public bool IsUserOnline { get; set; }

        public byte[] Version { get; set; }

        public int AllSessionsTotalScores { get; set; }

        public string AgoraUserId { get; set; }

        public int AttendeeVideoShareModeId { get; set; }

        public List<UserSessionInstructorSettingModel> UserSessionInstructorSettings { get; set; }
    }

    public class UserSessionScoreInfo
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Nickname { get; set; }

        public string LastName { get; set; }

        public string Location { get; set; }

        public string ImgUrl { get; set; }
    }
}
