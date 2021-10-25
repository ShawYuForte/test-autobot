using Newtonsoft.Json.Linq;
using Serilog.Events;
using Serilog.Sinks.EventLog;
using System;
using System.Linq;

namespace forte.EventLog
{
    public class DeviceEventIdProvider : IEventIdProvider
    {
        public ushort ComputeEventId(LogEvent logEvent)
        {
            var eventTypeProp = logEvent.Properties.FirstOrDefault(prop => prop.Key == "EventId");

            if (eventTypeProp.Value == null)
            {
                return (ushort)LogEventType.Error;
            }

            try
            {
                string eventType = eventTypeProp.Value.ToString();
                var parseEventType = JObject.Parse(eventType);

                var eventIdInt = parseEventType["Id"].ToString();

                if (eventType == null)
                {
                    return (int)LogEventType.Unknown;
                }

                var tryParseEventId = Enum.TryParse<LogEventType>(eventIdInt, ignoreCase: true, out var res);
                if (tryParseEventId)
                {
                    return (ushort)res;
                }

                return (ushort)LogEventType.Unknown;
            }
            catch (Exception)
            {
                return (ushort)LogEventType.Unknown;
            }
        }
    }
}
