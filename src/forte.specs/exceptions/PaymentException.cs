using System.Net;
using Newtonsoft.Json;

namespace forte.exceptions
{
    public class PaymentException : BusinessRuleException, IStatusCodeException
    {
        public PaymentException(HttpStatusCode httpStatusCode, PaymentError paymentError, string message)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            PaymentError = paymentError;
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public PaymentError PaymentError { get; set; }
    }

    public class PaymentError
    {
        [JsonProperty("type")]
        public string ErrorType { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("param")]
        public string Parameter { get; set; }
    }
}
