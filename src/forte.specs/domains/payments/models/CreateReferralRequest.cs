namespace forte.domains.payments.models
{
    public class CreateReferralRequest : CreatePromotionRequest
    {
        public CreateReferralRequest()
        {
            PromotionType = PromotionTypes.Referral;
        }

        /// <summary>
        ///     Amount off, if applying a fixed value discount
        /// </summary>
        public int? AmountOff { get; set; }

        /// <summary>
        ///     User identifier for the referer
        /// </summary>
        public string ReferredByUserId { get; set; }

        /// <summary>
        ///     If set, the promotion code can only be used by the specified user
        /// </summary>
        public string ReservedForEmail { get; set; }
    }

    public abstract class CreatePromotionRequest
    {
        public string Description { get; set; }

        public PromotionTypes PromotionType { get; protected set; }

        public string Title { get; set; }
    }
}
