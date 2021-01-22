using System;

namespace forte.models.classes
{
    public class UserSessionInstructorSettingModel
    {
        public string UserId { get; set; }

        public Guid Id { get; set; }

        public UserSessionSetting Setting { get; set; }

        public bool Value { get; set; }

        public DateTime Created { get; set; }

        public Guid SessionId { get; set; }

        public byte[] Version { get; set; }
    }
}
