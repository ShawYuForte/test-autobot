using System;

namespace forte.extensions
{
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Checks whether exception or any of the inner exceptions contain the provided message
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool MessageContains(this Exception exception, string message)
        {
            var innerException = exception;
            while (innerException != null)
            {
                if (innerException.Message.Contains(message))
                {
                    return true;
                }

                innerException = innerException.InnerException;
            }
            return false;
        }

        public static bool IsOfType<T>(this Exception exception)
            where T : Exception
        {
            if (exception == null)
            {
                return false;
            }

            var innerException = exception;
            while (innerException != null)
            {
                if (innerException is T)
                {
                    return true;
                }

                innerException = innerException.InnerException;
            }
            return false;
        }
    }
}
