﻿using System;

namespace forte.models.classes
{
    public class SessionInstructorSettingModel
    {
        public Guid Id { get; set; }

        public SessionSetting Setting { get; set; }

        public bool Value { get; set; }

        public DateTime Created { get; set; }

        public Guid SessionId { get; set; }

        public byte[] Version { get; set; }
    }
}
