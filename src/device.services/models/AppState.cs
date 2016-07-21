#region

using System;
using System.ComponentModel;
using AutoMapper;
using forte.device.models;
using forte.device.Properties;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;

#endregion

namespace forte.devices.models
{
    [NotifyPropertyChanged]
    public class AppState
    {
        static AppState()
        {
            //if (Settings.Default.CallUpgrade)
            //{
            //    Settings.Default.Upgrade();
            //    Settings.Default.CallUpgrade = false;
            //    Settings.Default.Save();
            //}
            //Mapper.CreateMap<AppState, AppState>();
            //Instance = JsonConvert.DeserializeObject<AppState>(Settings.Default.State)
            //           ?? new AppState().Reset();
            Instance.Initialized = true;
            Instance.SetDefaultValues();
        }

        private AppState()
        {
            ((INotifyPropertyChanged) this).PropertyChanged += OnPropertyChanged;
        }

        public static AppState Instance { get; }

        /// <summary>
        ///     Internal flag to determine if the state was initialized from the data store
        /// </summary>
        protected bool Initialized { get; set; }

        /// <summary>
        ///     Azure Media Services Program Name
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        ///     Class start time
        /// </summary>
        public DateTime ClassStartTime { get; set; }

        /// <summary>
        ///     Class duration in minutes
        /// </summary>
        public int ClassDuration { get; set; }

        /// <summary>
        ///     VMIX runtime, if started by the app
        /// </summary>
        public VmixRuntime VmixRuntime { get; set; }

        /// <summary>
        ///     Current workflow state
        /// </summary>
        public Workflow WorkflowState { get; set; }

        /// <summary>
        ///     Currently created program
        /// </summary>
        public VMixState CurrentVmixState { get; set; }

        /// <summary>
        ///     Currently created program
        /// </summary>
        public AzureProgram CurrentProgram { get; set; }

        /// <summary>
        ///     Specify whether Azure channel is running
        /// </summary>
        public bool AzureChannelRunning { get; set; }

        /// <summary>
        ///     Name of the class being streamed
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///     Sets the default values for fields that are not specified
        /// </summary>
        protected void SetDefaultValues()
        {
        }

        /// <summary>
        ///     Reset state
        /// </summary>
        public AppState Reset()
        {
            var next30MinPoint = DateTime.Now;
            while (next30MinPoint.Minute%30 != 0) next30MinPoint = next30MinPoint.AddMinutes(1);
            // clear seconds to zero (doesn't matter, just for visual)
            next30MinPoint = next30MinPoint.AddSeconds(-next30MinPoint.Second);
            ClassStartTime = next30MinPoint;
            WorkflowState = Workflow.NotStarted;
            CurrentProgram = null;
            AzureChannelRunning = false;
            return this;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Initialized) return;
            var stateString = JsonConvert.SerializeObject(this);
            //Settings.Default.State = stateString;
            //Settings.Default.Save();
        }
    }

    public class VmixRuntime
    {
        public DateTime StartTime { get; set; }
        public long SessionId { get; set; }
    }
}