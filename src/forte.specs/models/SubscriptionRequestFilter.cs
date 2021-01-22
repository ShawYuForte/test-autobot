using System;

namespace forte.models
{
    public class SubscriptionRequestFilter : RequestFilter
    {
        /// <summary>
        ///     User identifier filter
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Include subscription related data (e.g. plan)
        /// </summary>
        public bool? IncludeRelatedData { get; set; }

        /// <summary>
        ///     Subscription identifier
        /// </summary>
        public Guid? SubscriptionId { get; set; }
    }

    public class SubscriptionPatchRequest : RequestFilter
    {
        public string Token { get; set; }

        public Guid? SubscriptionPlanId { get; set; }

        public string PromotionCode { get; set; }

        public bool CreateNewCard { get; set; }
    }
}
