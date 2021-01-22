using forte.domains.core.models;
using forte.models;

namespace forte.domains.payments.models
{
    public enum IntervalTypes
    {
        [CharCode('D')]
        Day,
        [CharCode('W')]
        Week,
        [CharCode('M')]
        Month,
        [CharCode('Y')]
        Year,
    }

    public enum SubscriptionPlanTypes
    {
        /// <summary>
        ///     Free subscription plan
        /// </summary>
        [CharCode('F')]
        Free,

        /// <summary>
        ///     Premium subscription plan
        /// </summary>
        [CharCode('P')]
        Premium,
    }

    public class SubscriptionPlan : Model
    {
        /// <summary>
        ///     Currently active promotion, if any
        /// </summary>
        public Promotion ActivePromotion { get; set; }

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
        /// Discount description, if a discount has been applied
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
        ///     Plan revision
        /// </summary>
        public int Revision { get; set; }

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
        /// Subscription Auto Renewal option
        /// </summary>
        public bool AutoRenewal { get; set; }

        /// <summary>
        ///     Subscription plan type
        /// </summary>
        public SubscriptionPlanTypes Type { get; set; }
    }
}
