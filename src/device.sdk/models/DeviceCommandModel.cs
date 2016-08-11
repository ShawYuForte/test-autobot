using System;
using System.Collections.Generic;

namespace forte.devices.models
{
    public class DeviceCommandModel
    {
        public DeviceCommandModel()
        {
            Data = new Dictionary<string, DataValue>();
        }

        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public DeviceCommands Command { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime? ExecutedOn { get; set; }
        public bool ExecutionSucceeded { get; set; }
        public string ExecutionMessages { get; set; }
        public int RetryCount { get; set; }
        public Dictionary<string, DataValue> Data { get; set; }
        public ExecutionStatus Status { get; set; }
        public DateTime? PublishedOn { get; set; }
    }

    public enum DeviceCommands
    {
        /// <summary>
        ///     Device is requested to update its state
        /// </summary>
        UpdateState,

        /// <summary>
        ///     Device is requested to load preset and start streaming
        /// </summary>
        StartStreaming,

        /// <summary>
        ///     Device is requested to kick off it's program
        /// </summary>
        StartProgram,

        /// <summary>
        ///     Device is requested to stop streaming
        /// </summary>
        StopStreaming,

        /// <summary>
        ///     Device is requested to stop program
        /// </summary>
        StopProgram
    }

    public enum ExecutionStatus
    {
        /// <summary>
        ///     Command has been received
        /// </summary>
        Received = 0,

        /// <summary>
        ///     Command has been executed successfully
        /// </summary>
        Executed = 1,

        /// <summary>
        ///     Command execution failed
        /// </summary>
        Failed = 2
    }
}