using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using forte.domains.payments.models;
using forte.models;
using forte.models.accounts;
using Forte.Web.Models;

namespace forte.services.accounts
{
    public interface IRegistrationService
    {
        /// <summary>
        ///     Create invitation
        /// </summary>
        /// <param name="createInvitationRequest"></param>
        /// <returns></returns>
        Task CreateInvitationsAsync(CreateInvitationsRequest createInvitationRequest);

        /// <summary>
        ///     Create studio invitation
        /// </summary>
        /// <param name="createStudioInvitationRequest"></param>
        /// <returns></returns>
        Task CreateStudioInvitationsAsync(CreateStudioInvitationsRequest createStudioInvitationRequest);

        /// <summary>
        ///     Create a new subscription for user with specified email, and selected plan
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subscriptionPlanId"></param>
        /// <param name="promotionCode"></param>
        /// <param name="paymentCode"></param>
        /// <param name="referralCode"></param>
        /// <param name="sharedVideoUrl"></param>
        /// <returns></returns>
        Task CreateSubscriptionAsync(
            string email,
            Guid subscriptionPlanId,
            string paymentCode,
            string promotionCode,
            string referralCode,
            string sharedVideoUrl);

        /// <summary>
        ///     Fetch invitation by code
        /// </summary>
        /// <param name="invitationCode"></param>
        /// <returns></returns>
        Task<InvitationExModel> FetchInvitationAsync(string invitationCode);

        /// <summary>
        ///     Fetch all invitation
        /// </summary>
        /// <returns></returns>
        Task<InvitationPageModel> FetchInvitationsAsync(InvitationFilter request);

        /// <summary>
        ///     Finalizes the decision on an invitation
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <param name="success"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        Task FinalizeInvitationAsync(string promotionCode, bool success, string reason);

        /// <summary>
        ///     Process an invitation (create referral, send email, etc.) based on business rules
        /// </summary>
        /// <param name="invitationId"></param>
        /// <param name="sharedVideoUrl">shared video url</param>
        /// <returns></returns>
        Task ProcessInvitationAsync(Guid invitationId, string sharedVideoUrl);

        /// <summary>
        ///     Processes referral that initiated the subscription, if there was a referral
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        Task ProcessReferralAsync(Guid subscriptionId);

        Task<RegisterResult> RegisterExternal(RegisterViewModelApi model, HttpContextBase ctx, List<string> outErrors);

        Task<RegisterResult> Register(RegisterViewModelApi model, HttpContextBase ctx, List<string> outErrors);

        Task<PricingPlan> GetSelectedSubscriptionPlanAsync(Guid planId, string promotionCode);
    }
}
