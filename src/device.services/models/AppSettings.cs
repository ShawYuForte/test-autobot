#region

using System.ComponentModel;
using System.Configuration;
using AutoMapper;
using forte.device.Properties;
using Newtonsoft.Json;
using PostSharp.Patterns.Model;

#endregion

namespace forte.devices.models
{
    [NotifyPropertyChanged]
    public class AppSettings
    {
        /// <summary>
        ///     Minimum minutes to start program before the class starts
        /// </summary>
        public const int MinMinutesToStartProgramBeforeClass = 2;

        /// <summary>
        ///     Minimum minutes to start channel before the class starts
        /// </summary>
        public const int MinMinutesToStartChannelBeforeClass = 15;

        static AppSettings()
        {
            //if (Settings.Default.CallUpgrade)
            //{
            //    Settings.Default.Upgrade();
            //    Settings.Default.CallUpgrade = false;
            //    Settings.Default.Save();
            //}

#pragma warning disable CS0618 // Type or member is obsolete
            //Mapper.CreateMap<AppSettings, AppSettings>();
#pragma warning restore CS0618 // Type or member is obsolete

            //Instance = JsonConvert.DeserializeObject<AppSettings>(Settings.Default.AppSettings)
            //           ?? new AppSettings();
            Instance.SetDefaultValues();
            Instance.Initialized = true;
        }

        private AppSettings()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            ((INotifyPropertyChanged)this).PropertyChanged += OnPropertyChanged;
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
        ///     Overlay logo image
        /// </summary>
        [Category("Streaming")]
        [DisplayName("Overlay image")]
        [Description("Overlay logo image")]
        public string OverlayImage { get; set; }

        /// <summary>
        ///     How many minutes before class start to start the channel
        /// </summary>
        [Category("Azure")]
        [DisplayName("Start channel before")]
        [Description("Start channel this minutes before class starts (min 15)")]
        public int StartChannelMinutesBefore { get; set; }

        /// <summary>
        ///     How many minutes before class to start the program
        /// </summary>
        [Category("Azure")]
        [DisplayName("Start program before")]
        [Description("Start program this minutes before class starts (min 2)")]
        public int StartProgramMinutesBefore { get; set; }

        /// <summary>
        ///     Full studio name
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
                   !string.IsNullOrWhiteSpace(StudioName) &&
                   !(StartChannelMinutesBefore < MinMinutesToStartChannelBeforeClass) &&
                   !(StartProgramMinutesBefore < MinMinutesToStartProgramBeforeClass);
        }

        public string GetFirstInvalidProperty()
        {
            if (string.IsNullOrWhiteSpace(AmsAccountKey))
                return "Ams Account Key is missing";
            if (string.IsNullOrWhiteSpace(AmsAccountName))
                return "Ams Account Name is missing";
            if (string.IsNullOrWhiteSpace(VmixPresetFilePath))
                return "Vmix Preset File Path is missing";
            if (string.IsNullOrWhiteSpace(VmixExecutablePath))
                return "Vmix Executable Path is missing";
            if (string.IsNullOrWhiteSpace(StartupImage))
                return "Startup Image is missing";
            if (string.IsNullOrWhiteSpace(StartupVideo))
                return "Startup Video is missing";
            if (string.IsNullOrWhiteSpace(ClosingVideo))
                return "Closing Video is missing";
            if (string.IsNullOrWhiteSpace(ClosingImage))
                return "Closing Image is missing";
            if (string.IsNullOrWhiteSpace(OverlayImage))
                return "Overlay Image is missing";
            if (string.IsNullOrWhiteSpace(StudioName))
                return "Studio Name is missing";
            if (StartChannelMinutesBefore < MinMinutesToStartChannelBeforeClass)
                return $"Start Channel Minutes Before is {StartChannelMinutesBefore}, it must be at least {MinMinutesToStartChannelBeforeClass}";
            if (StartProgramMinutesBefore < MinMinutesToStartProgramBeforeClass)
                return $"Start Program Minutes Before is {StartProgramMinutesBefore}, it must be at least {MinMinutesToStartProgramBeforeClass}";

            return string.Empty;
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
            if (StartChannelMinutesBefore < MinMinutesToStartChannelBeforeClass)
                StartChannelMinutesBefore = MinMinutesToStartChannelBeforeClass;
            if (StartProgramMinutesBefore < MinMinutesToStartProgramBeforeClass)
                StartProgramMinutesBefore = MinMinutesToStartProgramBeforeClass;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!Initialized) return;
            var settingsString = JsonConvert.SerializeObject(this);
            //Settings.Default.AppSettings = settingsString;
            //Settings.Default.Save();

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