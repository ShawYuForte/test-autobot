using System;

namespace forte.domains.payments.models
{
    public class PricingPlan
    {
        /// <summary>
        ///     Plan description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Plan disclaimers
        /// </summary>
        public string Disclaimers { get; set; }

        /// <summary>
        ///     Discounted price, if plan is discounted
        /// </summary>
        public int? DiscountedPrice { get; set; }

        /// <summary>
        ///     Discount title, if a discount has been applied
        /// </summary>
        public string DiscountTitle { get; set; }

        /// <summary>
        /// If there is a discount, with a duration, this field represents the duration
        /// </summary>
        public int? DiscountDuration { get; set; }

        /// <summary>
        /// If there is  adiscount, this field represents the duration type
        /// </summary>
        public DurationTypes? DiscountDurationType { get; set; }

        /// <summary>
        ///     Plan global identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Payment interval
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        ///     Payment interval type
        /// </summary>
        public IntervalTypes? IntervalType { get; set; }

        /// <summary>
        ///     Plan price
        /// </summary>
        public int? Price { get; set; }

        /// <summary>
        ///     Plan subtitle
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        ///     Plan title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     If specified, the plan supports a trial for the specified number of days
        /// </summary>
        public int? TrialLength { get; set; }

        /// <summary>
        ///     Subscription plan type
        /// </summary>
        public SubscriptionPlanTypes Type { get; set; }

        /// <summary>
        ///    Discount Description
        /// </summary>
        public string DiscountDescription { get; set; }

        /// <summary>
        ///    Discount Description
        /// </summary>
        public int? DiscountTrialPeriodLength { get; set; }

        /// <summary>
        ///    Discount Description
        /// </summary>
        public int? AmountOff { get; set; }

        /// <summary>
        ///    Discount Description
        /// </summary>
        public int? PercentOff { get; set; }

        public bool AutoRenewal { get; set; }
    }
}
