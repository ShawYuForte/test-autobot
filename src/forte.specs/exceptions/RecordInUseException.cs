using System;

namespace forte.exceptions
{
    /// <summary>
    ///     Record in use exception, typically thrown when attempting to delete a record that is being
    ///     used through referential integrity
    /// </summary>
    public class RecordInUseException : BusinessRuleException
    {
        public RecordInUseException()
            : this("Record in use")
        {
        }

        public RecordInUseException(string message)
            : this(message, null)
        {
        }

        public RecordInUseException(string message, Exception exception)
            : base(message, exception)
        {
            ErrorCode = Codes.ERR_DEFAULT;
        }

        public static class Codes
        {
            /// <summary>
            ///     Default 'record in use' exception type
            /// </summary>
            public const long ERR_DEFAULT = 0x003000000000000001;
        }
    }
}
