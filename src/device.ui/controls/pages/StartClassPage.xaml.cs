#region

using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using forte.device.models;
using forte.devices.models;
using Xceed.Wpf.Toolkit.Core;

#endregion

namespace device.ui.controls.pages
{
    /// <summary>
    ///     Interaction logic for StartClassPage.xaml
    /// </summary>
    public partial class StartClassPage
    {
        private Timer _timer;

        public DateTime StartProgramAt
        {
            get { return (DateTime)GetValue(StartProgramAtProperty); }
            set { SetValue(StartProgramAtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartProgramAt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartProgramAtProperty =
            DependencyProperty.Register("StartProgramAt", typeof(DateTime), typeof(StartClassPage));

        public StartClassPage()
        {
            InitializeComponent();
        }

        public event EventHandler OnPause;

        private void Countdown_OnStop(object sender, EventArgs e)
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        }

        private bool _classStarted = false;
        private readonly object _classStartedLock = new Object();

        private void StartAzureProgram()
        {
            lock (_classStartedLock)
            {
                if (_classStarted) return;
                _classStarted = true;
            }

            SetBusy();
            Log("Starting program!");
            _timer = new Timer(state =>
            {
                _timer.Dispose();
                _azureService.StartProgram();
                Dispatcher.Invoke(StartLiveContentTransition);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        protected void StartLiveContentTransition()
        {
            Log("Starting intro video...");
            var openingVideo = AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.OpeningVideo);
            _vmixService.SetPreview(openingVideo);
            // Fade to intro video
            _vmixService.FadeToPreview();
            // After X seconds (intro video length), fade to camera 1
            _timer = new Timer(state =>
            {
                _timer.Dispose();
                Dispatcher.Invoke(StartLiveContent);
            }, null, TimeSpan.FromMilliseconds(openingVideo.Duration), TimeSpan.FromHours(1));

            // Set camera 1 at preview
            var cameraInput = AppState.Instance.CurrentVmixState.Inputs.First(input => input.Role == InputRole.Camera);
            _vmixService.SetPreview(cameraInput);
            Log("Set camera 1 as preview.");
        }

        protected void StartLiveContent()
        {
            _vmixService.FadeToPreview();
            Log("Switched to camera 1 video.");

            // Turn on audio (possibly fade)
            _vmixService.TurnAudioOn();
            Log("Turned audio on.");

            // Add logo overlay
            _vmixService.TurnOverlayOn();
            Log("Turned logo overlay on.");

            // Activate playlist
            AppState.Instance.CurrentVmixState = _vmixService.StartPlaylist();
            Log("Started the playlist");

            AppState.Instance.WorkflowState = Workflow.Streaming;
            Log("Completed step 3, waiting for the end of session");

            SetNotBusy();
            Done();
        }

        public override void Process()
        {
            // Wait at least 5 seconds, or the total number of seconds until 5 minutes before the class
            StartProgramAt = AppState.Instance.ClassStartTime.AddMinutes(-1 * AppSettings.Instance.StartProgramMinutesBefore);
            Log($"Will start program at {StartProgramAt}...");
            if (DateTime.Compare(StartProgramAt, DateTime.Now) < 0)
            {
                Log("Looks like I'm running behind, I'll start in 5 seconds...");
                StartProgramAt = DateTime.Now.AddSeconds(5);
            }
            ProgramStartCountdown.Start(StartProgramAt);
        }

        private void Countdown_OnExpire(object sender, EventArgs e)
        {
            StartAzureProgram();
        }
    }
}