using System;
using forte.models.events;

namespace forte.domains.payments.models
{
    public class ReferralProcessedEventModel : EventModel
    {
        public static Guid EventId => Guid.Parse("FEAED3F5-CA1F-4255-8117-E9A42FEF1C47");

        public override string Event => "Referral Processed";
    }
}
