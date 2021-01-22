using System;

namespace forte.models.classes
{
    public class SessionAttendanceModel
    {
        public string Status { get; set; }

        public Guid Id { get; set; }

        public string AttendanceType { get; set; }

        public Guid? SessionId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Created { get; set; }

        public byte[] Version { get; set; }
    }

    public class SessionAttendanceFilter : RequestFilter
    {
        public bool AllUsers { get; set; }

        public Guid? ClassId { get; set; }

        public Guid? SessionId { get; set; }
    }
}
