using System;
using forte.models;

namespace forte.domains.payments.models
{
    public class PricingPlanFilter : RequestFilter
    {
        public string PromotionCode { get; set; }

        public Guid PlanId { get; set; }
    }
}
