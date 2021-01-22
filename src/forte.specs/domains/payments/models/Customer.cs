using forte.domains.core.models;

namespace forte.domains.payments.models
{
    public class Customer : Model
    {
        /// <summary>
        ///     Current balance, if any, being stored on the customer’s account. If negative, the customer has credit to apply to
        ///     the next invoice. If positive, the customer has an amount owed that will be added to the next invoice. The balance
        ///     does not refer to any unpaid invoices; it solely takes into account amounts that have yet to be successfully
        ///     applied to any invoice. This balance is only taken into account for recurring billing purposes (i.e.,
        ///     subscriptions, invoices, invoice items).
        /// </summary>
        public int AccountBalance { get; set; }

        /// <summary>
        ///     Whether or not the latest charge for the customer’s latest invoice has failed.
        /// </summary>
        public bool Delinquent { get; set; }

        /// <summary>
        ///     Record description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Billing email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Customer's system user identifier
        /// </summary>
        public string UserId { get; set; }
    }
}
