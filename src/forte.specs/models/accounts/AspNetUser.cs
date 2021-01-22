using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace forte.domains.identity.entities
{
    public class AspNetUser : IdentityUser<string, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>
    {
        public string AgeGroup { get; set; }

        public string City { get; set; }

        // This attribute is a work arounds for EF issue: http://patrickdesjardins.com/blog/entity-framework-and-conversion-of-a-datetime2
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public string FirstName { get; set; }

        public string FitnessLevel { get; set; }

        public string Gender { get; set; }

        public string LastName { get; set; }

        public string RegistrationCode { get; set; }

        public string State { get; set; }

        public bool TermsAccepted { get; set; }

        public string ZipCode { get; set; }

        public bool NewsletterSubscribed { get; set; }

        public string HearAbout { get; set; }

        public Guid? StudioId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AspNetUser, string> manager)
        {
            return await GenerateUserIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<AspNetUser, string> manager,
            string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            var claims = await manager.GetClaimsAsync(userIdentity.GetUserId());
            userIdentity.AddClaims(claims);
            return userIdentity;
        }
    }
}
