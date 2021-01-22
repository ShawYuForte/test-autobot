using Newtonsoft.Json;

namespace forte.domains.payments.models
{
    public class PromotionSummary
    {
        /// <summary>
        ///     Amount off, if applying a fixed value discount
        /// </summary>
        public int? AmountOff { get; set; }

        /// <summary>
        ///     Promotion unique code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Promotion description
        /// </summary>
        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        ///     Promotion disclaimers, to be appended to plan, subscription, or customer account
        /// </summary>
        public string Disclaimers { get; set; }

        /// <summary>
        ///     Duration, if duration type is "recurring"
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        ///     Duration type
        /// </summary>
        public DurationTypes DurationType { get; set; }

        /// <summary>
        ///     Percent off, if percent based coupon
        /// </summary>
        public int? PercentOff { get; set; }

        /// <summary>
        ///     Promotion title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Promotion type
        /// </summary>
        public PromotionTypes CodeType { get; set; }

        /// <summary>
        ///     Trial period of promo code
        /// </summary>
        public int? TrialPeriodLength { get; set; }

        /// <summary>
        ///     Unlimited Days - flag that determines does this code has unlimited validation period
        /// </summary>
        public bool UnlimitedDays { get; set; }

        /// <summary>
        ///     RequireCreditCardAfterTrial - flag which determines shouldn't we ask user credit card info during registration or
        ///     not
        /// </summary>
        public bool RequireCreditCardAfterTrial { get; set; }

        /// <summary>
        ///     ReservedForEmail
        /// </summary>
        public string ReservedForEmail { get; set; }
    }
}
