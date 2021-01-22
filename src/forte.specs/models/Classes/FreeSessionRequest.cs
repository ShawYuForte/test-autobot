using System;

namespace forte.models.classes
{
    public class FreeSessionRequest : RequestFilter
    {
        public Guid? SessionId { get; set; }
    }
}
