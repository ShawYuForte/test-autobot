using System;

namespace forte.services
{
    public interface ILogger : IDisposable
    {
        /// <summary>
        ///     Write a log event with the <see cref="F:Serilog.Events.LogEventLevel.Debug" /> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the <see cref="F:Serilog.Events.LogEventLevel.Debug" /> level.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </example>
        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the <see cref="F:Serilog.Events.LogEventLevel.Information" /> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </example>
        void Information(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the Warning level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the Warning level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </example>
        void Warning(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the Error level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the Error level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>
        ///     Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </example>
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the Fatal level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>Log.Fatal("Process terminating.");</example>
        void Fatal(string messageTemplate, params object[] propertyValues);

        /// <summary>
        ///     Write a log event with the Fatal level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example>Log.Fatal(ex, "Process terminating.");</example>
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);
    }
}
