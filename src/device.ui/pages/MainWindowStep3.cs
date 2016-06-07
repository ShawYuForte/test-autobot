using System;
using System.Linq;
using System.Threading;
using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        protected void ExecuteStep3Workflow()
        {
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

            Log("Switched to opening video.");
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
        }
    }
}