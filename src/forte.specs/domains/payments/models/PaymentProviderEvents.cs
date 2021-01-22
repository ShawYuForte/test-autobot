namespace forte.domains.payments.models
{
    public enum PaymentProviderEvents
    {
        /// <summary>
        ///     A charge against customer payment method was attempted and failed
        /// </summary>
        ChargeFailed,

        /// <summary>
        ///     A charge against customer payment method was attempted and succeeded
        /// </summary>
        ChargeSucceeded,

        /// <summary>
        ///     A refund against customer payment method was attempted and succeeded
        /// </summary>
        ChargeRefunded,

        /// <summary>
        ///     A new invoice was created for customer subscription, for the current cycle
        /// </summary>
        InvoiceCreated,

        /// <summary>
        ///     Current invoice payment succeeded
        /// </summary>
        InvoicePaymentSucceeded,

        /// <summary>
        ///     Current invoice payment failed
        /// </summary>
        InvoicePaymentFailed,

        /// <summary>
        ///     Customer payment account (method) created
        /// </summary>
        PaymentAccountCreated,

        /// <summary>
        ///     Customer subscription deleted
        /// </summary>
        SubscriptionDeleted,

        /// <summary>
        ///     Occurs whenever a subscription changes. Examples would include switching from one plan to another, or switching
        ///     status from trial to active.
        /// </summary>
        SubscriptionUpdated,

        /// <summary>
        ///     Trial period is ending soon
        /// </summary>
        TrialEnding,

        /// <summary>
        ///     Occurs whenever any property of a customer changes.
        /// </summary>
        CustomerUpdated,
    }
}
