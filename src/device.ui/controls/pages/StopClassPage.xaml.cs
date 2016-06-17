#region

using System;
using System.Linq;
using System.Threading;
using System.Windows;
using forte.device.models;

#endregion

namespace device.ui.controls.pages
{
    /// <summary>
    ///     Interaction logic for StopClassPage.xaml
    /// </summary>
    public partial class StopClassPage
    {
        // Using a DependencyProperty as the backing store for StopProgramAt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopProgramAtProperty =
            DependencyProperty.Register("StopProgramAt", typeof (DateTime), typeof (StopClassPage));

        private readonly object _classStoppedLock = new object();

        private bool _classStopped = false;

        private Timer _timer;

        public StopClassPage()
        {
            InitializeComponent();
        }

        public DateTime StopProgramAt
        {
            get { return (DateTime) GetValue(StopProgramAtProperty); }
            set { SetValue(StopProgramAtProperty, value); }
        }

        private void StartTimer()
        {
            //var classStartTime = AppState.Instance.ClassStartTime;
            //var classDuration = AppState.Instance.ClassDuration;
            //var classEndTime = AppState.Instance.ClassStartTime.AddMinutes(classDuration);
            //var countDownSeconds = (classEndTime - DateTime.Now).TotalSeconds;

            //_timer = new Timer(state =>
            //{
            //    var timespanToEnd = TimeSpan.FromSeconds(countDownSeconds);
            //    var message =
            //        $"Class ending in {timespanToEnd.Hours} h : {timespanToEnd.Minutes} m : {timespanToEnd.Seconds} s";
            //    Dispatcher.Invoke(() => CountdownDisplay = message);
            //    if (countDownSeconds-- > 0) return;
            //    _timer.Dispose();
            //    Dispatcher.Invoke(InitiateReadyForNext);
            //}, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
        }

        public void PauseCountdownButton_Click(object source, RoutedEventArgs e)
        {
            //ShowPauseButton = false;
            //_timer.Dispose();
            //CountdownDisplay = "Timer stopped, you must manually click 'Next' when ready!";
        }

        public override void Process()
        {
            StopProgramAt = AppState.Instance.ClassStartTime.AddMinutes(AppState.Instance.ClassDuration);
            ClassEndCountdown.Start();
        }

        private void ClassEndCountdown_OnExpire(object sender, EventArgs e)
        {
            SetBusy();
            InitiateClassStopping();
        }

        protected void InitiateClassStopping()
        {
            lock (_classStoppedLock)
            {
                if (_classStopped) return;
                _classStopped = true;
            }

            var closingVideo =
                AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.ClosingVideo);
            _vmixService.SetPreview(closingVideo);
            Log("Placed closing video in preview.");

            // Turn off audio (possibly fade)
            var audioInput = AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.Audio);
            _vmixService.TurnAudioOff(audioInput);
            Log("Turned audio off.");

            // Remove logo overlay
            var overlayInput =
                AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            _vmixService.ToggleOverlay(overlayInput);
            Log("Turned logo overlay off.");

            // Stop playlist
            AppState.Instance.CurrentVmixState = _vmixService.StopPlaylist();
            Log("Stop the playlist");

            // Fade to intro video
            _vmixService.FadeToPreview();
            // After X seconds (intro video length), fade to camera 1
            _timer = new Timer(state =>
            {
                _timer.Dispose();
                Dispatcher.Invoke(EndLiveContent);
            }, null, TimeSpan.FromMilliseconds(closingVideo.Duration), TimeSpan.FromHours(1));

            Log("Switched to closing video.");

            // Set ending background image as preview
            var closingImageInput =
                AppState.Instance.CurrentVmixState.Inputs.First(input => input.Role == InputRole.ClosingStaticImage);
            _vmixService.SetPreview(closingImageInput);
            Log("Set closing image as preview.");
        }

        protected void EndLiveContent()
        {
            _vmixService.FadeToPreview();
            Log("Switched to closing image.");

            _timer = new Timer(state =>
            {
                _timer.Dispose();
                Dispatcher.Invoke(() => Log("Stopping Azure Program..."));
                _azureService.StopProgram();
                Dispatcher.Invoke(() => Log("Stopping vMix Stream..."));
                _vmixService.StopStreaming();
                Dispatcher.Invoke(() => Log("Stopping Azure Channel..."));
                if (_azureService.StopChannel())
                    Dispatcher.Invoke(() => Log("Stopped Azure channel (watching those $$$s)."));
                Dispatcher.Invoke(FinishWorkflow);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromHours(1));
        }

        protected void FinishWorkflow()
        {
            var vMixProcess = _vmixService.GetVmixProcess();
            vMixProcess?.Kill();
            Log("Stopped vMix.");

            SetNotBusy();
            AppState.Instance.WorkflowState = Workflow.CompletedSession;
            Log("Hooray! You completed a session, nothing else to be done here.");

            Done();
        }

        public void ForceContinue()
        {
            if (AppState.Instance.ClassStartTime.AddMinutes(AppState.Instance.ClassDuration) > DateTime.Now)
            {
                var response =
                    ShowQuestionMessageBox("Class has not ended yet, are you sure you want to stop the stream?",
                        "Confirm",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (response == MessageBoxResult.No) return;
            }
            InitiateClassStopping();
        }
    }
}