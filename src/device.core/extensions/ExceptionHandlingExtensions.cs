using System;
using System.Text;

namespace forte.devices.extensions
{
    public static class ExceptionHandlingExtensions
    {
        public static string InnerMessage(this object exceptionObject, bool append = false)
        {
            var exception = exceptionObject as Exception;
            if (exception == null) return string.Empty;
            return exception.InnerMessage(append);
        }

        public static string InnerMessage(this Exception exception, bool append = false)
        {
            if (exception == null) return null;
            var messageBuffer = new StringBuilder();
            if (append) messageBuffer.AppendLine(exception.Message);
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                if (append && exception != null) messageBuffer.AppendLine(exception.Message);
            }
            return append ? $"{messageBuffer.ToString()}{Environment.NewLine}{exception.Message}" : exception.Message;
        }
    }
}
