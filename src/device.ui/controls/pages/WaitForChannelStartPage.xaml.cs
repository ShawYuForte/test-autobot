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
    ///     Interaction logic for WaitForChannelStartPage.xaml
    /// </summary>
    public partial class WaitForChannelStartPage
    {
        // Using a DependencyProperty as the backing store for ShowCountdown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCountdownProperty =
            DependencyProperty.Register("ShowCountdown", typeof (Visibility), typeof (GetReadyPage),
                new PropertyMetadata(Visibility.Collapsed));

        // Using a DependencyProperty as the backing store for StartChannelAt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartChannelAtProperty =
            DependencyProperty.Register("StartChannelAt", typeof (DateTime), typeof (GetReadyPage));

        private Timer _azureTimer;

        public WaitForChannelStartPage()
        {
            InitializeComponent();
        }

        public Visibility ShowCountdown
        {
            get { return (Visibility) GetValue(ShowCountdownProperty); }
            set { SetValue(ShowCountdownProperty, value); }
        }

        public DateTime StartChannelAt
        {
            get { return (DateTime) GetValue(StartChannelAtProperty); }
            set { SetValue(StartChannelAtProperty, value); }
        }

        private void StartChannelCountdown_OnStop(object sender, EventArgs e)
        {
        }

        private void StartChannelCountdown_OnExpire(object sender, EventArgs e)
        {
            Log("Time is up!");
            SetBusy();
            ShowCountdown = Visibility.Collapsed;
            StartAzureChannelThen(ContinueMixedWorkflow);
        }

        private void ContinueMixedWorkflow()
        {
            Log("Starting VMIX stream with background photo (Azure not recording yet)...");
            // Start streaming background image on vMix

            if (!ActivateOpeningStaticImage())
            {
                Fail("Could not activate background image for opening scene!");
                return;
            }
            if (!PreviewOpeningVideo())
            {
                Fail("Could not set the opening video as preview in vMix!");
                return;
            }

            _vmixService.StartStreaming();
            Log("Started VMIX stream with background photo, waiting for class to start.");

            // Set the workflow step so the user can read the instructions
            AppState.Instance.WorkflowState = Workflow.ReadyToStartProgram;

            Done();
        }

        private void StartAzureChannelThen(Action callback)
        {
            Log("Starting azure channel...");
            // Start Azure channel
            _azureTimer = new Timer(state =>
            {
                _azureTimer.Dispose();
                _azureService.StartChannel();
                Dispatcher.Invoke(() => Log("Azure channel started."));
                AppState.Instance.AzureChannelRunning = true;
                Dispatcher.Invoke(callback);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        public override void Process()
        {
            Log("All set, waiting for the timer to expire before starting the channel (save some $$).");
            StartChannelAt =
                AppState.Instance.ClassStartTime.AddMinutes(-1*AppSettings.Instance.StartChannelMinutesBefore);

            if (AppState.Instance.AzureChannelRunning)
            {
                Log("Azure channel already running, skipping the wait!");
                ContinueMixedWorkflow();
                return;
            }

            Log($"Will start channel at {StartChannelAt}.");
            StartChannelCountdown.Start(StartChannelAt);
        }

        private bool PreviewOpeningVideo()
        {
            VMixInput openingVideoInput;
            try
            {
                // Set the active window to static background image
                openingVideoInput =
                    AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.OpeningVideo);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(GetParentWindow(),
                    "Wrong number of starting video inputs specified, can't tell which one to select!",
                    "Cannot set background", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            _vmixService.SetPreview(openingVideoInput);
            Log($"Set the preview input to '{openingVideoInput.Title}'.");
            return true;
        }

        private bool ActivateOpeningStaticImage()
        {
            VMixInput backgroundImageInput;
            try
            {
                // Set the active window to static background image
                backgroundImageInput =
                    AppState.Instance.CurrentVmixState.Inputs.Single(input => input.Role == InputRole.OpeninStaticImage);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(GetParentWindow(), "Wrong number of background image inputs specified, can't tell which one to select!",
                    "Cannot set background", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            _vmixService.SetActive(backgroundImageInput);
            Log($"Set the active input to '{backgroundImageInput.Title}'.");
            return true;
        }

        private void StartChannelCountdown_OnLog(string message)
        {
            Log(message);
        }
    }
}