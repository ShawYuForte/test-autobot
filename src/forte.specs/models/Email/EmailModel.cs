using System.Collections.Generic;

namespace Forte.Svc.Services.Models.Email
{
    public enum MessageContentType
    {
        Text,
        Html,
    }

    public class EmailModel
    {
        public EmailModel()
        {
        }

        public EmailModel(string to, string subject, string message)
        {
            Recipients = new List<string> { to };
            BccRecipients = new List<string>();
            Subject = subject;
            Message = message;
            MessageContentType = MessageContentType.Text;
        }

        public List<string> Recipients { get; set; }

        public List<string> BccRecipients { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public MessageContentType MessageContentType { get; set; }
    }
}
