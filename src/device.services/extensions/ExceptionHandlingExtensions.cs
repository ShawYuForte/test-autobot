using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forte.device.extensions
{
    public static class ExceptionHandlingExtensions
    {
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
