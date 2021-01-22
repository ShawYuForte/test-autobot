using System;

namespace forte.exceptions
{
    public class ContextNotAuthorizedException : BusinessRuleException
    {
        public ContextNotAuthorizedException()
        {
        }

        public ContextNotAuthorizedException(string message)
            : base(message)
        {
        }

        public ContextNotAuthorizedException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
