using System;

namespace forte.devices.models
{
    public class DeviceCommandModel
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public DeviceCommands Command { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime? ExecutedOn { get; set; }
        public bool ExecutionSucceeded { get; set; }
        public string ExecutionMessages { get; set; }
    }

    public enum DeviceCommands
    {
        /// <summary>
        ///     Device is requested to update its state
        /// </summary>
        UpdateState,

        /// <summary>
        ///     Device is requested to start streaming
        /// </summary>
        StartStreaming,

        /// <summary>
        ///     Device is requested to stop streaming
        /// </summary>
        StopStreaming
    }
}