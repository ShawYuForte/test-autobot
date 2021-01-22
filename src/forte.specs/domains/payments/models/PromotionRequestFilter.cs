using System;
using forte.models;

namespace forte.domains.payments.models
{
    public class PromotionRequestFilter : RequestFilter
    {
        /// <summary>
        ///     Filter by plan identifier
        /// </summary>
        public Guid? PlanId { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }
    }
}
