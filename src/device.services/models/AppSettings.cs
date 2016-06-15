#region

using System.ComponentModel;
using System.Configuration;
using System.IO;
using AutoMapper;
using forte.device.Properties;
using Newtonsoft.Json;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

#endregion

namespace forte.device.models
{
    [NotifyPropertyChanged]
    public class AppSettings
    {
        static AppSettings()
        {
            if (Settings.Default.CallUpgrade)
            {
                Settings.Default.Upgrade();
                Settings.Default.CallUpgrade = false;
                Settings.Default.Save();
            }
            Mapper.CreateMap<AppSettings, AppSettings>();
            Instance = JsonConvert.DeserializeObject<AppSettings>(Settings.Default.AppSettings)
                       ?? new AppSettings();
            Instance.Initialized = true;
            Instance.SetDefaultValues();
        }

        private AppSettings()
        {
            ((INotifyPropertyChanged) this).PropertyChanged += OnPropertyChanged;
        }

        public static AppSettings Instance { get; }

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
        [Category("Azure")]
        [DisplayName("AMS Account Name")]
        [Description("Azure Media Services account name")]
        public string AmsAccountName { get; set; }

        /// <summary>
        ///     Azure Media Services Account Key
        /// </summary>
        [Category("Azure")]
        [DisplayName("AMS Account Key")]
        [Description("Azure Media Services account key")]
        public string AmsAccountKey { get; set; }

        /// <summary>
        ///     Azure Media Services Channel Name
        /// </summary>
        [Category("Azure")]
        [DisplayName("AMS Channel Name")]
        [Description("Azure Media Services channel name dedicated to this studio")]
        public string ChannelName { get; set; }

        /// <summary>
        ///     VMIX preset file path
        /// </summary>
        [Category("vMix")]
        [DisplayName("Presets File")]
        [Description("vMix Presets file path")]
        public string VmixPresetFilePath { get; set; }

        /// <summary>
        ///     Path to the VMIX executable
        /// </summary>
        [Category("vMix")]
        [DisplayName("Executable File")]
        [Description("vMix Application executable file path")]
        public string VmixExecutablePath { get; set; }

        /// <summary>
        ///     Startup static image
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Startup image")]
        [Description("Startup image displayed before class starts")]
        public string StartupImage { get; set; }

        /// <summary>
        ///     Startup video
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Startup video")]
        [Description("Startup video displayed before class starts")]
        public string StartupVideo { get; set; }

        /// <summary>
        ///     Closing video
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Closing video")]
        [Description("Closing video displayed after class is over")]
        public string ClosingVideo { get; set; }

        /// <summary>
        ///     Closing video
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Closing image")]
        [Description("Closing image displayed after class is over")]
        public string ClosingImage { get; set; }

        /// <summary>
        /// Overlay logo image
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Overlay image")]
        [Description("Overlay logo image")]
        public string OverlayImage { get; set; }

        /// <summary>
        /// How many minutes before class start to start the channel
        /// </summary>
        [Category("Azure")]
        [DisplayName("Start channel before")]
        [Description("Start channel this minutes before class starts (min 15)")]
        [Range(15, int.MaxValue)]
        public int StartChannelMinutesBefore { get; set; }

        /// <summary>
        /// How many minutes before class to start the program
        /// </summary>
        [Category("Azure")]
        [DisplayName("Start program before")]
        [Description("Start program this minutes before class starts (min 3)")]
        [Range(3, int.MaxValue)]
        public int StartProgramMinutesBefore { get; set; }

        /// <summary>
        /// Full studio name
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Studio name")]
        [Description("Full studio name")]
        public string StudioName { get; set; }

        public AppSettings Copy()
        {
            return Mapper.Map(this, new AppSettings());
        }

        public void OverrideWith(AppSettings newSettings)
        {
            Mapper.Map(newSettings, this);
        }

        public bool AreValid()
        {
            return !string.IsNullOrWhiteSpace(AmsAccountKey) &&
                   !string.IsNullOrWhiteSpace(AmsAccountName) &&
                   !string.IsNullOrWhiteSpace(VmixPresetFilePath) &&
                   !string.IsNullOrWhiteSpace(VmixExecutablePath) &&
                   !string.IsNullOrWhiteSpace(StartupImage) &&
                   !string.IsNullOrWhiteSpace(StartupVideo) &&
                   !string.IsNullOrWhiteSpace(ClosingVideo) &&
                   !string.IsNullOrWhiteSpace(ClosingImage) &&
                   !string.IsNullOrWhiteSpace(OverlayImage) && 
                   StartChannelMinutesBefore > 15 &&
                   StartProgramMinutesBefore > 3;
        }

        /// <summary>
        ///     Sets the default values for fields that are not specified
        /// </summary>
        protected void SetDefaultValues()
        {
            if (string.IsNullOrWhiteSpace(VmixPresetFilePath))
                VmixPresetFilePath = ConfigurationManager.AppSettings["presetFilePath"];
            if (string.IsNullOrWhiteSpace(VmixExecutablePath))
                VmixExecutablePath = ConfigurationManager.AppSettings["vmixExeFilePath"];
            if (string.IsNullOrWhiteSpace(StartupImage))
                StartupImage = "logo_dark_background.jpg";
            if (string.IsNullOrWhiteSpace(StartupVideo))
                StartupVideo = "logo_dark_background_mantis.mp4";
            if (string.IsNullOrWhiteSpace(ClosingVideo))
                ClosingVideo = "logo_reveal_house.mp4";
            if (string.IsNullOrWhiteSpace(ClosingImage))
                ClosingImage = "logo_girl_warrior_stance.jpg";
            if (string.IsNullOrWhiteSpace(OverlayImage))
                OverlayImage = "overlay_1280_720.png";
            if (StartChannelMinutesBefore <= 0)
                StartChannelMinutesBefore = 15;
            if (StartProgramMinutesBefore <= 0)
                StartProgramMinutesBefore = 3;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Initialized) return;
            var settingsString = JsonConvert.SerializeObject(this);
            Settings.Default.AppSettings = settingsString;
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
}