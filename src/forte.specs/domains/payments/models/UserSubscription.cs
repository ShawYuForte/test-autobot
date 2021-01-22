using System;

namespace forte.domains.payments.models
{
    public class UserSubscription
    {
        /// <summary>
        ///     Model global identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     The subscription plan associated with this subscription
        /// </summary>
        public PricingPlan Plan { get; set; }

        /// <summary>
        ///     Determines whether the subscription cancels at the end of the priod
        /// </summary>
        public bool CancelsAtPeriodEnd { get; set; }

        /// <summary>
        ///     If canceled, specifies the date and time when it was cancelled
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
        ///     If trial subscription, date when trial started
        /// </summary>
        public DateTime? TrialStart { get; set; }

        /// <summary>
        ///     If trial subscription, date when trial ended
        /// </summary>
        public DateTime? TrialEnd { get; set; }

        /// <summary>
        /// Only for subscriptions where used promocode with RequireCreditCardAfterTrial = true should be used
        /// </summary>
        public bool NoPayCardTrialEndWarningSent { get; set; }

        /// <summary>
        /// shared video url (if user was invited by sharing a class video)
        /// </summary>
        public string SharedVideoUrl { get; set; }
    }
}
