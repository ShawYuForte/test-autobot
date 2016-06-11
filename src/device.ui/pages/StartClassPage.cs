#region

using System;
using System.Linq;
using System.Threading;
using forte.device.models;
using Xceed.Wpf.Toolkit.Core;

#endregion

namespace device.ui.pages
{
    public partial class MainWindow
    {
        private bool _classStarted = false;
        private readonly object _classStartedLock = new Object();

        private void StartClassTimer()
        {
            // Wait at least 5 seconds, or the total number of seconds until 5 minutes before the class
            var seconds = Math.Max(5, (AppState.ClassStartTime - DateTime.Now).TotalSeconds - 60 * 5);

            _timer = new Timer(state =>
            {
                var remaining = seconds;
                Dispatcher.Invoke(() =>
                {
                    var timeSpan = TimeSpan.FromSeconds(remaining);
                    StartClassTimerDisplay = $"Starting program in {(int)timeSpan.TotalHours} hours, {timeSpan.Minutes} minutes, {timeSpan.Seconds} seconds";
                });
                if (!(seconds-- <= 0)) return;
                _timer.Dispose();
                Dispatcher.Invoke(StartAzureProgram);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
        }

        private void PauseClassTimer()
        {
            _timer.Dispose();
            StartClassTimerDisplay = "Countdown stopped, click 'Next' to continue manually!";
        }

        private void StartClassPage_Next(object sender, CancelRoutedEventArgs cancelRoutedEventArgs)
        {
            cancelRoutedEventArgs.Cancel = true;
            _timer.Dispose();
            StartAzureProgram();
        }

        private void StartAzureProgram()
        {
            lock (_classStartedLock)
            {
                if (_classStarted) return;
                _classStarted = true;
            }

            IsBusy = true;
            Log("Starting program!");
            StartClassTimerDisplay = "Starting program NOW!";
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
            var openingVideo = State.Inputs.Single(input => input.Role == InputRole.OpeningVideo);
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
            var cameraInput = State.Inputs.First(input => input.Role == InputRole.Camera);
            _vmixService.SetPreview(cameraInput);
            Log("Set camera 1 as preview.");
        }

        protected void StartLiveContent()
        {
            _vmixService.FadeToPreview();
            Log("Switched to camera 1 video.");

            // Turn on audio (possibly fade)
            var audioInput = State.Inputs.Single(input => input.Role == InputRole.Audio);
            _vmixService.ToggleAudio(audioInput);
            Log("Turned audio on.");

            // Add logo overlay
            var overlayInput = State.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            _vmixService.ToggleOverlay(overlayInput);
            Log("Turned logo overlay on.");

            // Activate playlist
            State = _vmixService.StartPlaylist();
            Log("Started the playlist");

            SetWorkflowStep(Workflow.Streaming);
            Log("Completed step 3, waiting for the end of session");

            IsBusy = false;
            wizard.CurrentPage = StopClassPage;
        }
    }
}