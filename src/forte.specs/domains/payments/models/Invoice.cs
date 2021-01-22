using System;
using forte.domains.core.models;

namespace forte.domains.payments.models
{
    public class Invoice : Model
    {
        public int AmountDue { get; set; }

        /// <summary>
        ///     Discount amount, if any
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        ///     Total after applying discounts
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        ///     Number of payment attempts made for this invoice, from the perspective of the payment retry schedule. Any payment
        ///     attempt counts as the first attempt, and subsequently only automatic retries increment the attempt count. In other
        ///     words, manual payment attempts after the first attempt do not affect the retry schedule.
        /// </summary>
        public int AttemptCount { get; set; }

        /// <summary>
        ///     Whether or not an attempt has been made to pay the invoice.
        /// </summary>
        public bool Attempted { get; set; }

        /// <summary>
        ///     Whether or not the invoice is still trying to collect payment. An invoice is closed if it’s either paid or it has
        ///     been marked closed. A closed invoice will no longer attempt to collect payment.
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        ///     Customer identifier
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        ///     Description as the customer sees it
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The time at which payment will next be attempted.
        /// </summary>
        public DateTime? NextPaymentAttempt { get; set; }

        /// <summary>
        ///     Whether or not payment was successfully collected for this invoice. An invoice can be paid (most commonly) with a
        ///     charge or with credit from the customer’s account balance.
        /// </summary>
        public bool Paid { get; set; }

        /// <summary>
        ///     Billing period end date
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        ///     Billing period start date
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        ///     Subscription this invoice is for
        /// </summary>
        public Subscription Subscription { get; set; }

        /// <summary>
        ///     Subscription identifier
        /// </summary>
        public Guid SubscriptionId { get; set; }

        /// <summary>
        ///     Customer's system user identifier
        /// </summary>
        public string UserId { get; set; }
    }
}
