using System;

namespace forte.exceptions
{
    public class DuplicateRecordException : BusinessRuleException
    {
        public DuplicateRecordException()
            : this(null)
        {
        }

        public DuplicateRecordException(string message)
            : this(message, null)
        {
        }

        public DuplicateRecordException(string message, Exception exception)
            : base(message, exception)
        {
            ErrorCode = Codes.ERR_DEFAULT;
        }

        public static class Codes
        {
            /// <summary>
            /// Default duplicate record exception type
            /// </summary>
            public const long ERR_DEFAULT = 0x002000000000000001;
        }
    }
}
