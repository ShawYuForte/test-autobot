#region

using System;
using System.ComponentModel;
using System.Configuration;
using AutoMapper;
using forte.device.Properties;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;

#endregion

namespace forte.device.models
{
    [NotifyPropertyChanged]
    public class AppState
    {
        static AppState()
        {
            if (Settings.Default.CallUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.CallUpgrade = false;
                Settings.Default.Save();
            }
            Mapper.CreateMap<AppState, AppState>();
            Instance = JsonConvert.DeserializeObject<AppState>(Settings.Default.State)
                       ?? new AppState().Reset();
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
        ///     Specifies whether AMS settings were verified (on Azure)
        /// </summary>
        public bool AmsSettingsVerified { get; set; }

        /// <summary>
        ///     Azure Media Services Account Name
        /// </summary>
        public string AmsAccountName { get; set; }

        /// <summary>
        ///     Azure Media Services Account Key
        /// </summary>
        public string AmsAccountKey { get; set; }

        /// <summary>
        ///     Azure Media Services Channel Encoding
        /// </summary>
        public string AmsEncoding { get; set; }

        /// <summary>
        ///     Azure Media Services Channel Name
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        ///     Azure Media Services Program Name
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        ///     Name of the trainer being recorded (used for asset naming)
        /// </summary>
        public string TrainerName { get; set; }

        /// <summary>
        ///     Class start time
        /// </summary>
        public DateTime ClassStartTime { get; set; }

        /// <summary>
        ///     Class duration in minutes
        /// </summary>
        public int ClassDuration { get; set; }

        /// <summary>
        ///     VMIX preset file path
        /// </summary>
        public string VmixPresetFilePath { get; set; }

        /// <summary>
        ///     Path to the VMIX executable
        /// </summary>
        public string VmixExecutablePath { get; set; }

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
        public AzureProgram CurrentProgram { get; set; }

        /// <summary>
        ///     Sets the default values for fields that are not specified
        /// </summary>
        protected void SetDefaultValues()
        {
            if (string.IsNullOrWhiteSpace(VmixPresetFilePath))
                VmixPresetFilePath = ConfigurationManager.AppSettings["presetFilePath"];
            if (string.IsNullOrWhiteSpace(VmixExecutablePath))
                VmixExecutablePath = ConfigurationManager.AppSettings["vmixExeFilePath"];
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
            return this;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Initialized) return;
            var stateString = JsonConvert.SerializeObject(this);
            Settings.Default.State = stateString;
            Settings.Default.Save();

            ResetAzureSettingsConfirmed(e);
        }

        private void ResetAzureSettingsConfirmed(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AmsAccountKey) || e.PropertyName == nameof(AmsAccountName) ||
                e.PropertyName == nameof(ChannelName))
            {
                AmsSettingsVerified = false;
            }
        }
    }

    public class VmixRuntime
    {
        public DateTime StartTime { get; set; }
        public long SessionId { get; set; }
    }
}