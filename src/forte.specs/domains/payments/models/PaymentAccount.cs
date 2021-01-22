using forte.models;

namespace forte.domains.payments.models
{
    public enum PaymentAccountTypes
    {
        [CharCode('C')]
        Card,
    }

    public class PaymentAccount
    {
        public PaymentAccountTypes Type { get; set; }

        /// <summary>
        ///     Billing address line 1
        /// </summary>
        public string BillingAddressLine1 { get; set; }

        /// <summary>
        ///     Billing address line 2
        /// </summary>
        public string BillingAddressLine2 { get; set; }

        /// <summary>
        ///     Billing address city
        /// </summary>
        public string BillingAddressCity { get; set; }

        /// <summary>
        ///     Billing address state
        /// </summary>
        public string BillingAddressState { get; set; }

        /// <summary>
        ///     Billing address zip code
        /// </summary>
        public string BillingAddressZip { get; set; }

        /// <summary>
        ///     Billing address country
        /// </summary>
        public string BillingAddressCountry { get; set; }

        /// <summary>
        ///     If address_zip was provided, results of the check: pass, fail, unavailable, or unchecked.
        /// </summary>
        public string BillingAddressVerification { get; set; }

        /// <summary>
        ///     Card brand. Can be Visa, American Express, MasterCard, Discover, JCB, Diners Club, or Unknown.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        ///     Two-letter ISO code representing the country of the card.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///     Currency ISO code
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        ///     User this account is associated with
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     If a CVC was provided, results of the check: pass, fail, unavailable, or unchecked
        /// </summary>
        public string CvcVerification { get; set; }

        /// <summary>
        ///     The last four digits of the device account number.
        /// </summary>
        public string LastFourDigits { get; set; }

        /// <summary>
        ///     Month when this account expires (if applicable, e.g. credit card)
        /// </summary>
        public int? ExpirationMonth { get; set; }

        /// <summary>
        ///     Year when this account expires (if applicable, e.g. credit card)
        /// </summary>
        public int? ExpirationYear { get; set; }

        /// <summary>
        ///     Uniquely identifies this particular card number. You can use this attribute to check whether two customers who’ve
        ///     signed up with you are using the same card number, for example.
        /// </summary>
        public string Fingerprint { get; set; }

        /// <summary>
        ///     Card funding type. Can be credit, debit, prepaid, or unknown
        /// </summary>
        public string Funding { get; set; }

        /// <summary>
        ///     Payment method owner name (e.g. cardholder name if credit card)
        /// </summary>
        public string AccountHolderName { get; set; }

        /// <summary>
        ///     The type of entity that holds the account. This can be either individual or company.
        /// </summary>
        public string AccountHolderType { get; set; }

        /// <summary>
        ///     The name of the bank associated with payment method
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        ///     Bank routing number
        /// </summary>
        public string RoutingNumber { get; set; }
    }
}
