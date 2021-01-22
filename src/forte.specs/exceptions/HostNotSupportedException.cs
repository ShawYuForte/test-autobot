using System;

namespace Forte.Exceptions
{
    public class HostNotSupportedException : Exception
    {
        public HostNotSupportedException()
            : base("Host not supported")
        {
        }

        public HostNotSupportedException(string message)
            : base(message)
        {
        }

        public HostNotSupportedException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
