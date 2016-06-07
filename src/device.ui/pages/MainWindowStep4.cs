using System;
using System.Linq;
using System.Threading;
using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        protected void ExecuteStep4Workflow()
        {
            var closingVideo = State.Inputs.Single(input => input.Role == InputRole.ClosingVideo);
            _vmixService.SetPreview(closingVideo);

            // Turn on audio (possibly fade)
            var audioInput = State.Inputs.Single(input => input.Role == InputRole.Audio);
            _vmixService.ToggleAudio(audioInput);
            Log("Turned audio off.");

            // Add logo overlay
            var overlayInput = State.Inputs.Single(input => input.Role == InputRole.LogoOverlay);
            _vmixService.ToggleOverlay(overlayInput);
            Log("Turned logo overlay off.");

            // Activate playlist
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
            // Set camera 1 at preview
            var closingImageInput = State.Inputs.First(input => input.Role == InputRole.ClosingStaticImage);
            _vmixService.SetPreview(closingImageInput);
            Log("Set closing image as preview.");
        }

        protected void EndLiveContent()
        {
            _vmixService.FadeToPreview();
            Log("Switched to closing image.");

            SetWorkflowStep(Workflow.CompletedSession);
            Log("Hooray! You completed a session, nothing else to be done here.");
        }
    }
}