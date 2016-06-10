#region

using System;
using System.Linq;
using System.Threading;
using System.Windows;
using forte.device.models;
using Xceed.Wpf.Toolkit.Core;

#endregion

namespace device.ui.pages
{
    public partial class MainWindow
    {
        private void StopClassPage_Next(object sender, CancelRoutedEventArgs cancelRoutedEventArgs)
        {
            cancelRoutedEventArgs.Cancel = true;

            if (AppState.ClassStartTime.AddMinutes(AppState.ClassDuration) > DateTime.Now)
            {
                var response = MessageBox.Show("Class has not ended yet, are you sure you want to stop the stream?", "Confirm",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (response == MessageBoxResult.No) return;
            }

            InitializeComponent();
        }

        protected void InitiateClassStopping()
        {
            var closingVideo = State.Inputs.Single(input => input.Role == InputRole.ClosingVideo);
            _vmixService.SetPreview(closingVideo);
            Log("Placed closing video in preview.");

            // Turn on audio (possibly fade)
            var audioInput = State.Inputs.Single(input => input.Role == InputRole.Audio);
            _vmixService.ToggleAudio(audioInput);
            Log("Turned audio off.");

            // Remove logo overlay
            var overlayInput = State.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            _vmixService.ToggleOverlay(overlayInput);
            Log("Turned logo overlay off.");

            // Stop playlist
            State = _vmixService.StopPlaylist();
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
            var closingImageInput = State.Inputs.First(input => input.Role == InputRole.ClosingStaticImage);
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
                _azureService.StopProgram();
                Dispatcher.Invoke(() => Log("Stopped Azure Program, stopping channel..."));
                _azureService.StopChannel();
                Dispatcher.Invoke(FinishWorkflow);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromHours(1));
        }

        protected void FinishWorkflow()
        {
            Log("Stopped Azure channel (watching those $$$s).");
            SetWorkflowStep(Workflow.CompletedSession);
            Log("Hooray! You completed a session, nothing else to be done here.");

            wizard.CurrentPage = LastPage;
        }
    }
}