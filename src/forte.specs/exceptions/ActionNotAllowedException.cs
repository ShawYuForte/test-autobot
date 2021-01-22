using System;
using forte.exceptions;

namespace Forte.Exceptions
{
    public class ActionNotAllowedException : BusinessRuleException
    {
        public ActionNotAllowedException()
            : base("Action not allowed")
        {
            ErrorCode = ErrorCodes.Security.ActionNotAllowed;
        }

        public ActionNotAllowedException(string message)
            : base(message)
        {
            ErrorCode = ErrorCodes.Security.ActionNotAllowed;
        }

        public ActionNotAllowedException(string message, Exception exception)
            : base(message, exception)
        {
            ErrorCode = ErrorCodes.Security.ActionNotAllowed;
        }
    }
}
