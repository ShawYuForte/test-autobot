using System;

namespace forte.exceptions
{
    public class RecordNotFoundException : BusinessRuleException
    {
        public RecordNotFoundException()
            : this("Record not found")
        {
        }

        public RecordNotFoundException(string message)
            : this(message, null)
        {
        }

        public RecordNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
            ErrorCode = Codes.ERR_DEFAULT;
        }

        public static class Codes
        {
            /// <summary>
            ///     Default 'record not found' exception type
            /// </summary>
            public const long ERR_DEFAULT = 0x001000000000000001;
        }
    }
}
