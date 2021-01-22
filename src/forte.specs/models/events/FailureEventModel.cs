using System;
using forte.models.events;

namespace Forte.Domains.Events.Models
{
    public class FailureEventModel : EventModel
    {
        public FailureEventModel()
        {
            TypeId = Guid.Parse("01CDB952-8B46-488A-8DDE-A229D0CCF9AF");
        }

        public override string Event => "Business process failure event";

        /// <summary>
        /// Failure message
        /// </summary>
        public string FailureMessage { get; set; }

        /// <summary>
        /// Failure details, e.g. for technical failure, stack trace
        /// </summary>
        public string FailureDetails { get; set; }

        /// <summary>
        /// Source system or service where the failure occurred
        /// </summary>
        public string FailureSource { get; set; }

        /// <summary>
        /// Unique identifier of the source type
        /// </summary>
        public Guid FailureSourceTypeId { get; set; }

        /// <summary>
        /// Unique idenfitier of the source instance
        /// </summary>
        public Guid FailureSourceInstanceId { get; set; }
    }
}
