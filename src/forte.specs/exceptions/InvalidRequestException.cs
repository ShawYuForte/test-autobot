using System;

namespace forte.exceptions
{
    public class InvalidRequestException : BusinessRuleException
    {
        public InvalidRequestException()
            : base("Invalid request")
        {
        }

        public InvalidRequestException(string message)
            : base(message)
        {
        }

        public InvalidRequestException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
