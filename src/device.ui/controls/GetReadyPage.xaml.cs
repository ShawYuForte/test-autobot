#region

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using forte.device.models;
using forte.device.services;

#endregion

namespace device.ui.controls
{
    /// <summary>
    ///     Interaction logic for GetReadyPage.xaml
    /// </summary>
    public partial class GetReadyPage
    {
        // Using a DependencyProperty as the backing store for ShowFootnote.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowFootnoteProperty =
            DependencyProperty.Register("ShowFootnote", typeof (Visibility), typeof (GetReadyPage),
                new PropertyMetadata(Visibility.Visible));

        // Using a DependencyProperty as the backing store for ShowCountdown.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCountdownProperty =
            DependencyProperty.Register("ShowCountdown", typeof (Visibility), typeof (GetReadyPage),
                new PropertyMetadata(Visibility.Collapsed));

        private readonly AzureService _azureService = new AzureService();
        private readonly VMixService _vmixService = new VMixService();
        private bool _azureFailed;
        private Timer _azureTimer;
        private bool _vmixFailed;
        private bool _vmixReady;
        private Timer _vmixTimer;

        public DateTime StartChannelAt
        {
            get { return (DateTime)GetValue(StartChannelAtProperty); }
            set { SetValue(StartChannelAtProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartChannelAt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartChannelAtProperty =
            DependencyProperty.Register("StartChannelAt", typeof(DateTime), typeof(GetReadyPage));


        public GetReadyPage()
        {
            InitializeComponent();

            ShowCountdown = Visibility.Collapsed;
        }

        public AppState AppState => AppState.Instance;
        public AppSettings AppSettings => AppSettings.Instance;

        private VMixState VMixState { get; set; }

        public Visibility ShowFootnote
        {
            get { return (Visibility) GetValue(ShowFootnoteProperty); }
            set { SetValue(ShowFootnoteProperty, value); }
        }

        public Visibility ShowCountdown
        {
            get { return (Visibility) GetValue(ShowCountdownProperty); }
            set { SetValue(ShowCountdownProperty, value); }
        }

        private void StartChannelCountdown_OnStop(object sender, EventArgs e)
        {
        }

        private void StartChannelCountdown_OnExpire(object sender, EventArgs e)
        {
            StartAzureChannelThen(ContinueMixedWorkflow);
        }

        public override void Process()
        {
            // Start vMix
            SetUpVmix();
            SetUpAzure();
        }

        private void SetUpAzure()
        {
            var startWaitForClassTimer = new Action(WaitForClassTimer);
            var createAzureProgram = new Action(delegate { CreateAzureProgramThen(startWaitForClassTimer); });

            CheckIfAzureIsReadyThen(createAzureProgram);
        }

        private void ContinueMixedWorkflow()
        {
            _azureTimer = new Timer(state =>
            {
                if (_vmixFailed)
                {
                    _azureTimer.Dispose();
                    Dispatcher.Invoke(() => Fail("VMix Failed, stopping!"));
                    return;
                }
                if (_azureFailed)
                {
                    _azureTimer.Dispose();
                    Dispatcher.Invoke(() => Fail("Azure setup failed, stopping!"));
                    return;
                }
                if (!_vmixReady)
                {
                    // wait some more, highly unlikely to end up here
                    return;
                }
                Dispatcher.Invoke(StartStreamingOnVmixAndContinue);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
        }

        private void WaitForClassTimer()
        {
            //if (!_vmixReady)
            //{
            //    Log("Waiting for vMix...");
            //    _azureTimer = new Timer(state =>
            //    {
            //        if (!_vmixReady) return;
            //        WaitForClassTimer();
            //        _azureTimer.Dispose();
            //    }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            //    return;
            //}

            Log("All set, waiting for the timer to expire before starting the channel (save some $$).");
            StartChannelAt = AppState.ClassStartTime.AddMinutes(-1*AppSettings.StartChannelMinutesBefore);

            ShowCountdown = Visibility.Visible;
            ShowFootnote = Visibility.Collapsed;
            StartChannelCountdown.Start();

            SetNotBusy();
        }

        #region CheckIfAzureIsReadyThen

        /// <summary>
        ///     Checks if Azure is ready:
        ///     - Channel cannot be running
        ///     - No programs can be running
        /// </summary>
        /// <param name="completeCallback"></param>
        private void CheckIfAzureIsReadyThen(Action completeCallback)
        {
            Log("Verifying azure channel is in expected state...");

            // verify
            _azureTimer = new Timer(state =>
            {
                _azureTimer.Dispose();

                if (_azureService.ThereAreProgramsRunning())
                {
                    Dispatcher.Invoke(OtherProgramsRunningError);
                    return;
                }

                if (_azureService.IsChannelRunning())
                {
                    Dispatcher.Invoke(() => StopAzureChannelThen(completeCallback));
                    return;
                }
                Dispatcher.Invoke(completeCallback);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        #endregion

        /// <summary>
        ///     Fail Azure and notify user via dialog
        /// </summary>
        private void OtherProgramsRunningError()
        {
            _azureFailed = true;
            Fail("Could not continue!");
            MessageBox.Show(GetParentWindow(),
                "There are existing programs running, it's not safe to continue. Please go to Azure Portal and turn them off manually, then try again",
                "Azure Programs Running", MessageBoxButton.OK, MessageBoxImage.Stop);
        }

        private void StartAzureChannelThen(Action callback)
        {
            Log("Starting azure channel...");
            // Start Azure channel
            _azureTimer = new Timer(state =>
            {
                _azureTimer.Dispose();
                _azureService.StartChannel();
                Dispatcher.Invoke(callback);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        #region StopAzureChannelThen

        private void StopAzureChannelThen(Action callback)
        {
            Log("Azure channel already running...");

            var response =
                MessageBox.Show(GetParentWindow(),
                    "Looks like the Azure channel is already running, I recommend you shut it down first. If you have enough time, definitively recommended." +
                    Environment.NewLine + Environment.NewLine + "Do you want me to stop it for you?",
                    "Channel already running", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (response)
            {
                case MessageBoxResult.Cancel:
                    Fail("Canceled by user");
                    return;

                case MessageBoxResult.Yes:
                    Log("Stopping it first...");
                    // Stop Azure channel
                    _azureTimer = new Timer(state =>
                    {
                        _azureTimer.Dispose();
                        _azureService.StopChannel();
                        Dispatcher.Invoke(() => callback?.Invoke());
                    }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
                    break;

                case MessageBoxResult.No:
                    callback?.Invoke();
                    break;
            }
        }

        #endregion

        private void CreateAzureProgramThen(Action callback)
        {
            Log("Creating Azure program...");
            // Create Azure program
            _azureTimer = new Timer(state =>
            {
                _azureTimer.Dispose();
                AppState.CurrentProgram = _azureService.CreateProgram();
                Dispatcher.Invoke(() => Log("Azure program created"));
                Dispatcher.Invoke(callback);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        private void StartStreamingOnVmixAndContinue()
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
            AppState.WorkflowState = Workflow.ReadyToStartProgram;

            Done();
        }

        #region ShutdownVmix

        private bool ShutdownVmix(Process existingProcess)
        {
            Log("vMix found already running!");
            var messageBuffer = new StringBuilder();

            if (AppState.VmixRuntime != null && existingProcess.StartTime == AppState.VmixRuntime.StartTime &&
                existingProcess.SessionId == AppState.VmixRuntime.SessionId)
            {
                messageBuffer.Append("It looks like I have left vMix running from a previous session. ");
            }
            else
            {
                messageBuffer.Append("vMix is already running, ");
            }

            messageBuffer.AppendLine("I need to make sure I run it from scratch so I can configure it properly. ");
            messageBuffer.AppendLine("I must shut it down before I can continue. Can I shut it down? ");
            messageBuffer.Append(Environment.NewLine + Environment.NewLine +
                                 "(You can answer 'No', shut it down yourself, and try again)");

            var response = MessageBox.Show(messageBuffer.ToString(), "vMix already running", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (response == MessageBoxResult.No)
            {
                _vmixFailed = true;
                return false;
            }

            existingProcess.Kill();
            Log("vMix automatically shut down!");
            return true;
        }

        #endregion

        private void SetUpVmix()
        {
            var existingProcess = _vmixService.GetVmixProcess();

            if (existingProcess != null && !ShutdownVmix(existingProcess)) return;

            StartupVmix();

            // Delay loading preset because vMix loads slowly
            _vmixTimer = new Timer(state =>
            {
                _vmixTimer.Dispose();
                Dispatcher.Invoke(LoadVmixPresets);
            }, null, TimeSpan.FromSeconds(5), TimeSpan.FromDays(1));
        }

        #region StartupVmix

        private void StartupVmix()
        {
            var vMixProcess = System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = AppSettings.Instance.VmixExecutablePath,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            if (vMixProcess == null)
            {
                MessageBox.Show("Could not start vMix, no error info was provided, sorry :(", "I failed",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _vmixFailed = true;
                return;
            }

            AppState.VmixRuntime = new VmixRuntime
            {
                SessionId = vMixProcess.SessionId,
                StartTime = vMixProcess.StartTime
            };

            Log("vMix started.");
        }

        #endregion

        private void LoadVmixPresets()
        {
            // Load vMix presets
            var waitFor = TimeSpan.FromSeconds(30);
            var repeatAfter = TimeSpan.FromSeconds(20);

            var watch = new Stopwatch();
            watch.Start();
            Log("Loading vMix preset (this takes a minute or more, please be patient)...");
            _vmixService.LoadPreset();

            _vmixTimer = new Timer(state =>
            {
                if (!_vmixService.PresetLoaded()) return;

                _vmixTimer.Dispose();
                Dispatcher.Invoke(() =>
                {
                    VMixState = _vmixService.FetchState();
                    _vmixReady = true;
                    Log("Loaded vMix preset!");
                });
            }, null, waitFor, repeatAfter);
        }

        private bool PreviewOpeningVideo()
        {
            VMixInput openingVideoInput;
            try
            {
                // Set the active window to static background image
                openingVideoInput = VMixState.Inputs.Single(input => input.Role == InputRole.OpeningVideo);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Wrong number of starting video inputs specified, can't tell which one to select!",
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
                backgroundImageInput = VMixState.Inputs.Single(input => input.Role == InputRole.OpeninStaticImage);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Wrong number of background image inputs specified, can't tell which one to select!",
                    "Cannot set background", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            _vmixService.SetActive(backgroundImageInput);
            Log($"Set the active input to '{backgroundImageInput.Title}'.");
            return true;
        }
    }
}