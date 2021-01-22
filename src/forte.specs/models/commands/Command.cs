using System;
using System.Collections.Generic;
using Forte.Domains.Commands.Entities;
using Newtonsoft.Json;

namespace forte.models.commands
{
    /// <summary>
    ///     Represents an asynchronous command
    /// </summary>
    public abstract class Command
    {
        protected Command()
        {
            Data = new Dictionary<string, DataValue>();
            TriggerEventQualifier = new Dictionary<string, DataValue>();
        }

        /// <summary>
        ///     Command data
        /// </summary>
        public Dictionary<string, DataValue> Data { get; set; }

        /// <summary>
        ///     Command description
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        ///     UTC date and time when the command was executed
        /// </summary>
        public DateTime ExecutedOn { get; protected set; }

        /// <summary>
        ///     Command instance identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Command name
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        ///     When scheduled, specifies the date and time when this command must be executed
        /// </summary>
        public DateTime? ScheduledOn { get; set; }

        /// <summary>
        ///     Command sender / originator
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        ///     Specifies how this command is triggered
        /// </summary>
        public CommandTriggerTypes TriggerType { get; set; }

        /// <summary>
        ///     When command is triggered by event, specifies trigger event type identifier
        /// </summary>
        public Guid? TriggerEventTypeId { get; set; }

        /// <summary>
        ///     For event triggered commands, specifies data expected in the trigger event
        /// </summary>
        public Dictionary<string, DataValue> TriggerEventQualifier { get; set; }

        /// <summary>
        ///     Command type identifier
        /// </summary>
        public Guid TypeId { get; protected set; }

        /// <summary>
        /// JSON representation of this command
        /// </summary>
        [JsonIgnore]
        public string Json { get; set; }

        public T GetData<T>(string name)
        {
            try
            {
                if (Data == null || !Data.ContainsKey(name))
                {
                    return default(T);
                }

                return Data[name].Get<T>();
            }
            catch
            {
                return default(T);
            }
        }

        public void SetData(string name, object data)
        {
            if (Data == null)
            {
                Data = new Dictionary<string, DataValue>();
            }

            var dataValue = data as DataValue;

            if (dataValue != null)
            {
                if (!Data.ContainsKey(name))
                {
                    Data.Add(name, dataValue);
                }
                else
                {
                    Data[name] = dataValue;
                }
            }
            else
            {
                if (!Data.ContainsKey(name))
                {
                    Data.Add(name, new DataValue(data));
                }
                else
                {
                    Data[name].Set(data);
                }
            }
        }
    }
}
