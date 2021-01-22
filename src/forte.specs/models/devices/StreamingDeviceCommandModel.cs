using System;
using System.Collections.Generic;

namespace forte.models.devices
{
    public enum StreamingDeviceCommands
    {
        UpdateSession = 0,
        CancelSession = 1,
        RestartSession = 2,
    }

    public enum StreamingDeviceType
    {
        Autobot = 0,
        Independent = 1,
    }

    public class StreamingDeviceCommandModel
    {
        private const string StrTimeStart = "TimeStart";
        private const string StrTimeEnd = "TimeEnd";
        private const string StrSessionId = "SessionId";
        private const string StrPermalink = "Permalink";
        private const string StrPreset = "Preset";
        private const string StrType = "Type";
        private const string StrDeviceType = "DeviceType";

        public StreamingDeviceCommandModel()
        {
            Data = new Dictionary<string, DataValue>();
        }

        /// <summary>
        ///     If command is canceled
        /// </summary>
        public DateTime? CanceledOn { get; set; }

        /// <summary>
        ///     Command to send to device
        /// </summary>
        public StreamingDeviceCommands Command { get; set; }

        /// <summary>
        ///     Command data, if any
        /// </summary>
        public Dictionary<string, DataValue> Data { get; set; }

        /// <summary>
        ///     If command is executed (i.e. complete)
        /// </summary>
        public DateTime? ExecutedOn { get; set; }

        /// <summary>
        ///     How many times execution was attempted until now
        /// </summary>
        public int ExecutionAttempts { get; set; }

        /// <summary>
        ///     Any execution messages
        /// </summary>
        public string ExecutionMessages { get; set; }

        /// <summary>
        ///     If execution succeeded
        /// </summary>
        public bool ExecutionSucceeded { get; set; }

        /// <summary>
        ///     Command instance unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     When was this command issued
        /// </summary>
        public DateTime IssuedOn { get; set; }

        /// <summary>
        ///     How many times should the device retry until giving up
        /// </summary>
        public int MaxAttemptsAllowed { get; set; }

        /// <summary>
        ///     Streaming device identifier
        /// </summary>
        public Guid StreamingDeviceId { get; set; }

        /// <summary>
        ///     Video stream identifier, if applicable
        /// </summary>
        public Guid? VideoStreamId { get; set; }

        public DateTime? TimeStart
        {
            get { return GetDataItem(StrTimeStart)?.DateTimeValue; } set { SetDataItem(StrTimeStart, value); }
        }

        public int? DeviceType
        {
            get { return GetDataItem(StrDeviceType)?.IntValue; } set { SetDataItem(StrDeviceType, value); }
        }

        public DateTime? TimeEnd
        {
            get { return GetDataItem(StrTimeEnd)?.DateTimeValue; } set { SetDataItem(StrTimeEnd, value); }
        }

        public Guid? SessionId
        {
            get { return GetDataItem(StrSessionId)?.GuidValue; } set { SetDataItem(StrSessionId, value); }
        }

        public string Permalink
        {
            get { return GetDataItem(StrPermalink)?.StringValue; } set { SetDataItem(StrPermalink, value); }
        }

        public string Preset
        {
            get { return GetDataItem(StrPreset)?.StringValue; } set { SetDataItem(StrPreset, value); }
        }

        public int? Type
        {
            get { return GetDataItem(StrType)?.IntValue; } set { SetDataItem(StrType, value); }
        }

        public void SetDataItem<T>(string item, T value)
        {
            Data[item] = new DataValue(value);
        }

        private DataValue GetDataItem(string item)
        {
            if (!Data.ContainsKey(item))
            {
                return null;
            }

            return Data[item];
        }
    }
}
