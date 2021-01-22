using System;
using forte.domains.core.models;
using forte.models;

namespace forte.domains.payments.models
{
    public enum PromotionTypes
    {
        /// <summary>
        ///     Code is simply used to track affiliate traffice
        /// </summary>
        [CharCode('A')]
        Affiliate,

        /// <summary>
        ///     Code is used to offer a discount
        /// </summary>
        [CharCode('C')]
        Coupon,

        /// <summary>
        ///     Code is used to track referrals (implies payment)
        /// </summary>
        [CharCode('R')]
        Referral,
    }

    public enum DurationTypes
    {
        /// <summary>
        ///     Code is applicable to one iteration (for coupon types)
        /// </summary>
        [CharCode('O')]
        Once,

        /// <summary>
        ///     Code is applicable to multiple iterations (for coupon types)
        /// </summary>
        [CharCode('R')]
        Recurring,

        /// <summary>
        ///     Code is applicable until subscription canceled (for coupon types)
        /// </summary>
        [CharCode('U')]
        Unlimited,
    }

    public class Promotion : Model
    {
        /// <summary>
        ///     Determines whether the promotion is active, based on business rules
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        ///     Amount off, if applying a fixed value discount
        /// </summary>
        public int? AmountOff { get; set; }

        /// <summary>
        ///     Promotion unique code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///     Currency (default "US")
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        ///     Promotion description
        /// </summary>
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
        ///     Maximum times this code can be redeemed. Null means infinite
        /// </summary>
        public int? MaxRedemptions { get; set; }

        /// <summary>
        ///     Percent off (1-100), if percent based coupon
        /// </summary>
        public int? PercentOff { get; set; }

        /// <summary>
        ///     Expiration date for this code
        /// </summary>
        public DateTime? RedeemBy { get; set; }

        /// <summary>
        ///     If set, the promotion code can only be used by the specified user
        /// </summary>
        public string ReservedForEmail { get; set; }

        /// <summary>
        ///     If set, the promotion code can only be used by the specified user
        /// </summary>
        public string ReservedForUserId { get; set; }

        /// <summary>
        ///     User identifier for the referer
        /// </summary>
        public string ReferredByUserId { get; set; }

        /// <summary>
        ///     How many times this code was redeemed
        /// </summary>
        public int TimesRedeemed { get; set; }

        /// <summary>
        ///     Revision number, keeps track of the number of times the entity gets edited
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        ///     Promotion title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Promotion type
        /// </summary>
        public PromotionTypes CodeType { get; set; }

        /// <summary>
        /// Trial period of promocode
        /// </summary>
        public int? TrialPeriodLength { get; set; }

        /// <summary>
        /// Unlimited Days - flag that determines does this code has unlimited validation period
        /// </summary>
        public bool UnlimitedDays { get; set; }

        /// <summary>
        /// RequireCreditCardAfterTrial - flag which determines shouldn't we ask user credit card info during registration or not
        /// </summary>
        public bool RequireCreditCardAfterTrial { get; set; }

        /// <summary>
        ///     Specifies whether this promotion is valid, based on business rules
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        ///     If Promotion is not valid, validation errors
        /// </summary>
        public string[] ValidationMessages { get; set; }
    }
}
