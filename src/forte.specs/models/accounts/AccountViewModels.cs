using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using forte.domains.payments.models;
using forte.web.Infrastructure;

namespace Forte.Web.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Display(Name = "Age Group")]
        public string AgeGroup { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Fitness Level")]
        public string FitnessLevel { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip Code *")]
        public string ZipCode { get; set; }

        [Display(Name = "Terms and Conditions")]
        [MustBeTrue(ErrorMessage = "You must accept the terms to register.")]
        public bool TermsAccepted { get; set; }

        [Display(Name = "Sign me up for the mailing list")]
        public bool MailingList { get; set; }

        /// <summary>
        /// Invitation code
        /// </summary>
        [Required(ErrorMessage = "A registration code is required to register")]
        [Display(Name = "Registration Code *")]
        public string RegistrationCode { get; set; }

        public string HearAbout { get; set; }

        public Guid? StudioId { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }

        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        private string _email;

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value?.Trim();
            }
        }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class SharedVideoModel
    {
        public string SharedVideoUrl { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            // Default values
            NewsletterSubscribed = true;
        }

        [Required(ErrorMessage = "A subscription plan must be selected before registering")]
        [Display(Name = "Subscription Plan identifier")]
        public Guid SubscriptionPlanId { get; set; }

        public bool IsPremiumPlan { get; set; }

        public PricingPlan PricingPlan { get; set; }

        [Display(Name = "Subscription Plan")]
        public string SubscriptionPlanSummary { get; set; }

        [Required(ErrorMessage = "Your First Name is required.")]
        [Display(Name = "First Name *")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your Last Name is required.")]
        [Display(Name = "Last Name *")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Your Email is required.")]
        [EmailAddress]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A Password is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password *")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password *")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Age Group")]
        public string AgeGroup { get; set; }

        [Display(Name = "Fitness Level")]
        public string FitnessLevel { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        //[Required(ErrorMessage = "Your Zip Code is required.")]
        [Display(Name = "Zip Code *")]
        public string ZipCode { get; set; }

        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }

        [Display(Name = "Terms and Conditions")]
        [MustBeTrue(ErrorMessage = "You must accept the terms to register.")]
        public bool TermsAccepted { get; set; }

        [Display(Name = "Sign me up for the FORTË newsletter")]
        public bool NewsletterSubscribed { get; set; }

        public string SubscriptionPlanJson { get; set; }

        public int SubscriptionPlanPrice { get; set; }

        /// <summary>
        ///     Label sent to inform user about the plan (e.g. $15 / week)
        /// </summary>
        public string SubscriptionPlanLabel { get; set; }

        //[Required(ErrorMessage = "Payment method is required.")]
        [Display(Name = "Payment Method *")]
        public string PaymentToken { get; set; }

        /// <summary>
        ///     External login provider, if linked to external provider, null otherwise
        /// </summary>
        public string ExternalLoginProvider { get; set; }

        public string ReferralCode { get; set; }

        [Display(Name = "How did you hear about us?")]
        public string HearAbout { get; set; }

        public string HearAboutOther { get; set; }

        public Guid? StudioId { get; set; }
    }

    public class RegisterViewModelApi : RegisterViewModel
    {
        public string CouponCode { get; set; }

        public string ShareVideoUrl { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
