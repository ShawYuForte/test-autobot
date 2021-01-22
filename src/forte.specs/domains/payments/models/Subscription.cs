using System;
using forte.domains.core.models;
using forte.models;

namespace forte.domains.payments.models
{
    public enum SubscriptionStatuses
    {
        [CharCode('T')]
        Trialing,
        [CharCode('A')]
        Active,
        [CharCode('O')]
        PastDue,
        [CharCode('C')]
        Canceled,
    }

    public enum SubscriptionChangeReasons
    {
        /// <summary>
        ///     User or system canceled a trial
        /// </summary>
        [CharCode('T')]
        TrialCanceled,

        /// <summary>
        ///     Payment failed
        /// </summary>
        [CharCode('F')]
        PaymentFailed,

        /// <summary>
        ///     Provider driven update (either systematic, or customer service driven)
        /// </summary>
        [CharCode('P')]
        ProviderUpdate,

        /// <summary>
        /// Current subscription canceled because of an upgrade
        /// </summary>
        [CharCode('U')]
        Upgrade,

        /// <summary>
        /// Current subscription canceled because of a downgrade
        /// </summary>
        [CharCode('D')]
        Downgrade,

        /// <summary>
        /// Current subscription canceled by the administrator
        /// </summary>
        [CharCode('M')]
        ManuallyCancelled,
    }

    public class Subscription : Model
    {
        /// <summary>
        ///     The subscription plan associated with this subscription
        /// </summary>
        public SubscriptionPlan Plan { get; set; }

        /// <summary>
        ///     The promotion associated with this subscription
        /// </summary>
        public Promotion Promotion { get; set; }

        /// <summary>
        ///     User identifier associated with this subscription
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Determines whether the subscription cancels at the end of the period
        /// </summary>
        public bool CancelsAtPeriodEnd { get; set; }

        /// <summary>
        ///     If canceled, specifies the date and time when it was canceled
        /// </summary>
        public DateTime? CanceledOn { get; set; }

        /// <summary>
        ///     Date and time when the current period ends
        /// </summary>
        public DateTime CurrentPeriodEnd { get; set; }

        /// <summary>
        ///     Date and time when the current period started
        /// </summary>
        public DateTime CurrentPeriodStart { get; set; }

        /// <summary>
        ///     If no longer active, specifies the date and time when subscription ended
        /// </summary>
        public DateTime? EndedOn { get; set; }

        /// <summary>
        ///     Current status for the subscription
        /// </summary>
        public SubscriptionStatuses Status { get; set; }

        /// <summary>
        ///     Currently selected payment method
        /// </summary>
        public PaymentAccount ActivePaymentMethod { get; set; }

        /// <summary>
        ///     Subscription plan type
        /// </summary>
        public SubscriptionPlanTypes PlanType { get; set; }

        /// <summary>
        ///     Registration referral code
        /// </summary>
        public string ReferralCode { get; set; }

        /// <summary>
        ///     If trial subscription, date when trial started
        /// </summary>
        public DateTime? TrialStart { get; set; }

        /// <summary>
        ///     If trial subscription, date when trial ended
        /// </summary>
        public DateTime? TrialEnd { get; set; }

        /// <summary>
        /// Only for subscriptions where used promo code with RequireCreditCardAfterTrial = true should be used
        /// </summary>
        public bool NoPayCardTrialEndWarningSent { get; set; }

        /// <summary>
        /// shared video URL (if user was invited by sharing a class video)
        /// </summary>
        public string SharedVideoUrl { get; set; }
    }
}
