using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using forte.domains.payments.models;
using forte.models;

namespace forte.domains.payments.services
{
    public interface IPaymentService
    {
        /// <summary>
        ///     Apply a promotion to the specified plan
        /// </summary>
        /// <param name="planId">Plan identifier</param>
        /// <param name="planVersion">Plan version</param>
        /// <param name="promoCode">Promotion code</param>
        /// <returns></returns>
        Task<SubscriptionPlan> ApplyPlanPromotionAsync(Guid planId, byte[] planVersion, string promoCode);

        /// <summary>
        ///     Get expiring annual subscription plans
        /// </summary>
        /// <param name="daysUntilExpiration">Interval until expiration</param>
        /// <returns></returns>
        Task<List<string>> GetExpiringAnnualSubscriptionsAsync(int daysUntilExpiration);

        /// <summary>
        ///     Get expiring Daily subscription plans
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetExpiringDailySubscriptionsAsync();

        /// <summary>
        ///     Get expiring Yearly subscription plans
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetExpiringYearlySubscriptionsAsync();

        /// <summary>
        ///     Get expiring Daily subscription plans
        /// </summary>
        /// <returns></returns>
        Task<List<string>> RemoveClaimforDailySubscriptionUsersAsync(string userId);

        /// <summary>
        ///     Cancel subscription trial
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        Task<Subscription> CancelSubscriptionTrialAsync(Guid subscriptionId);

        /// <summary>
        ///     Cancel subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        Task<Subscription> CancelSubscriptionAsync(Guid subscriptionId);

        /// <summary>
        ///     Create a new subscription plan
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task<SubscriptionPlan> CreatePlanAsync(SubscriptionPlan plan);

        /// <summary>
        ///     Creates promotion
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        Task<Promotion> CreatePromotionAsync(Promotion promotion);

        /// <summary>
        ///     Creates promotions
        /// </summary>
        /// <param name="promotions"></param>
        /// <returns></returns>
        Task<List<string>> CreatePromotionsAsync(List<Promotion> promotions, Guid planId, string prefix);

        /// <summary>
        ///     Create a new promotion of referral type, for a specified email and payee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isItInvitationForClassVideo"></param>
        /// <param name="createdByStudioManager"></param>
        /// <returns></returns>
        Task<Promotion> CreateReferralAsync(CreateReferralRequest request, bool isItInvitationForClassVideo, bool createdByStudioManager);

        /// <summary>
        ///     Create a new subscription for the user and plan
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="subscriptionPlanId"></param>
        /// <param name="paymentToken"></param>
        /// <param name="email"></param>
        /// <param name="promotionCode"></param>
        /// <param name="referralCode"></param>
        /// <param name="sharedVideoUrl"></param>
        /// <returns></returns>
        Task<Subscription> CreateSubscriptionAsync(string userId, string email, string name, Guid subscriptionPlanId, string paymentToken, string promotionCode, string referralCode, string sharedVideoUrl);

        /// <summary>
        /// Create a new partner subscription
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreatePartnerSubscriptionAsync(string userId);

        Task TerminatePartnerSubscriptionAsync(string userId);

        /// <summary>
        ///     Delete an existing subscription plan
        /// </summary>
        /// <param name="planId"></param>
        //// <param name="version">Record version, must match that in the data store</param>
        /// <returns></returns>
        Task DeletePlanAsync(Guid planId /*, byte[] version*/);

        /// <summary>
        ///     Fetch all existing plans (at their most recent revision)
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResultPage<SubscriptionPlan>> FetchPlansAsync(RequestFilter filter);

        /// <summary>
        ///     Fetch public pricing plans
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResultPage<PricingPlan>> FetchPricingPlansAsync(PricingPlanFilter filter);

        /// <summary>
        ///     Fetch all promotions
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResultPage<Promotion>> FetchPromotionsAsync(PromotionRequestFilter filter);

        /// <summary>
        ///     Fetch paginated users invoices
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResultPage<Invoice>> FetchUserInvoicesAsync(InvoiceRequestFilter filter);

        /// <summary>
        ///     Find a valid/active plan promotion.
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="code"></param>
        /// <returns>Promotion, or null if not found</returns>
        Task<PromotionSummary> FindPlanPromotionSummaryAsync(Guid planId, string code);

        /// <summary>
        ///     Find a promotion using a promo code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Promotion> FindPromotionAsync(string code);

        /// <summary>
        ///     Returns a promotion for public consumption, no authorization constraints
        /// </summary>
        /// <param name="promoCode"></param>
        /// <returns></returns>
        Task<PromotionSummary> FindPromotionSummaryAsync(string promoCode);

        /// <summary>
        ///     Get customer record for specified user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Customer> GetCustomerAsync(string userId);

        /// <summary>
        ///     Get a subscription plan for editing, using identifier
        /// </summary>
        /// <param name="planId"></param>
        /// <returns></returns>
        Task<SubscriptionPlan> GetPlanAsync(Guid planId);

        /// <summary>
        ///     Retrieve a pricing plan, using a subscription plan identifier, and apply either the passed in promotion code
        ///     (optional), or the active promotion code (if available)
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        Task<PricingPlan> GetPricingPlan(Guid planId, string promotionCode);

        /// <summary>
        ///     Retrieve the first premium annual plan
        /// </summary>
        /// <returns></returns>
        Task<PricingPlan> GetAnnualPlan();

        Task<Promotion> GetPromotionAsync(string promotionCode);

        /// <summary>
        ///     Get promotion by promotion Id
        /// </summary>
        /// <param name="promotionId"></param>
        /// <returns></returns>
        Task<Promotion> GetPromotionAsync(Guid promotionId);

        /// <summary>
        ///     Retrieve subscription using identifier
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <exception cref="ActionNotAllowedException">
        ///     Thrown if subscription belongs to another user, and user doesn't have manage subscription permissions
        /// </exception>
        /// <returns></returns>
        Task<Subscription> GetSubscriptionAsync(Guid subscriptionId);

        /// <summary>
        ///     Retrieve subscriptions as specified in the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<ResultPage<Subscription>> GetSubscriptionsAsync(SubscriptionRequestFilter filter);

        /// <summary>
        ///     Retrieve the current users's active subscription for current user
        /// </summary>
        /// <returns></returns>
        Task<UserSubscription> GetUserActiveSubscriptionAsync();

        /// <summary>
        ///     Retrieve the current users's active subscription
        /// </summary>
        /// <returns></returns>
        Task<UserSubscription> GetUserActiveSubscriptionAsync(string userId);

        /// <summary>
        ///     Handle provider notification event
        /// </summary>
        /// <param name="providerEvent"></param>
        /// <param name="providerEntityId">Payment provider entity record identifier</param>
        /// <param name="customerId">Payment provider customer identifier, if applicable</param>
        /// <returns></returns>
        Task HandleProviderEventAsync(PaymentProviderEvents providerEvent, string providerEntityId, string customerId = null);

        /// <summary>
        ///     Link an existing promotion to a plan
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task LinkPromotionToPlanAsync(Guid planId, string code);

        /// <summary>
        ///     Process a referral based on business rules
        /// </summary>
        /// <param name="referredUserId"></param>
        /// <param name="promotionCode"></param>
        /// <param name="isStudioBeneficiar"></param>
        /// <returns></returns>
        Task ProcessReferralAsync(string referredUserId, string promotionCode, bool isStudioBeneficiar = false);

        /// <summary>
        ///     Unlink promotion from plan
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="promoId"></param>
        /// <returns></returns>
        Task UnlinkPromotionToPlanAsync(Guid planId, Guid promoId);

        /// <summary>
        ///     Update subscription payment method using provider specific authorization token
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="token"></param>
        /// <param name="createNewCard"></param>
        /// <returns></returns>
        Task<PaymentAccount> UpdatePaymentMethodAsync(Guid subscriptionId, string token, bool createNewCard);

        /// <summary>
        ///     Update an existing subscription plan. If the plan is in use, a new revision of the plan will be created
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task<SubscriptionPlan> UpdatePlanAsync(SubscriptionPlan plan);

        /// <summary>
        ///     Updates promotion
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns></returns>
        Task<Promotion> UpdatePromotionAsync(Promotion promotion);

        /// <summary>
        ///     Upgrade an existing subscription
        /// </summary>
        /// <returns></returns>
        Task<Subscription> UpgradeSubscriptionAsync(Guid subscriptionId, Guid planId, string paymentToken, string promoCode);

        Task<bool> UpdateSubscriptionCouponAsync(string userId);

        Task<bool> UpdateDailySubscriptionAsync(string userId);
    }
}
