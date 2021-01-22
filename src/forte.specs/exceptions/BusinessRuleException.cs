using System;

namespace forte.exceptions
{
    /// <summary>
    /// The base business logic exception to throw for general business logic error, or to inherit the specific exceptions from.
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException()
        {
        }

        public BusinessRuleException(string message)
            : base(message)
        {
        }

        public BusinessRuleException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public BusinessRuleException(string message, Exception exception, long errorCode)
            : base(message, exception)
        {
            ErrorCode = errorCode;
        }

        public long ErrorCode { get; protected set; }
    }
}
