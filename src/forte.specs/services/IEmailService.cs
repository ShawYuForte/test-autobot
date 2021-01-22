using System.Collections.Generic;
using System.Threading.Tasks;
using Forte.Svc.Services.Models.Email;

namespace forte.services
{
    public interface IEmailService
    {
        /// <summary>
        ///     Send an email
        /// </summary>
        /// <param name="email"></param>
        Task SendAsync(EmailModel email);

        /// <summary>
        ///     Send an email template to specified single recipient, using an optional object tokenProvider and BCC recipient
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="templateUrl"></param>
        /// <param name="recipient"></param>
        /// <param name="tokenProvider"></param>
        /// <param name="bccRecipient"></param>
        /// <returns></returns>
        Task SendTemplateAsync(string recipient, string subject, string templateUrl, object tokenProvider = null, string bccRecipient = null);

        Task SendTemplateAsync(
            List<string> recipient,
            string subject,
            string templateUrl,
            object tokenProvider = null,
            List<string> bccRecipient = null);

        /// <summary>
        ///     Send an email template to specified recipients and BCC recipients, using an optional object tokenProvider
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="bccRecipients"></param>
        /// <param name="subject"></param>
        /// <param name="templateFile"></param>
        /// <param name="tokenProvider"></param>
        /// <returns></returns>
        Task SendTemplateAsync(List<string> recipients, List<string> bccRecipients, string subject, string templateFile, object tokenProvider = null);
    }
}
