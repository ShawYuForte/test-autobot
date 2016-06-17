#region

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows;
using forte.device.models;

#endregion

namespace device.ui.controls.pages
{
    /// <summary>
    ///     Interaction logic for GetReadyPage.xaml
    /// </summary>
    public partial class GetReadyPage
    {
        // Using a DependencyProperty as the backing store for ShowFootnote.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowFootnoteProperty =
            DependencyProperty.Register("ShowFootnote", typeof(Visibility), typeof(GetReadyPage),
                new PropertyMetadata(Visibility.Visible));

        private bool _azureFailed;
        private bool _azureReady;
        private Timer _azureTimer;
        private bool _vmixFailed;
        private bool _vmixReady;
        private Timer _vmixTimer;
        private object _readyCheckLock = new object();

        public GetReadyPage()
        {
            InitializeComponent();
        }

        public AppState AppState => AppState.Instance;
        public AppSettings AppSettings => AppSettings.Instance;

        public Visibility ShowFootnote
        {
            get { return (Visibility)GetValue(ShowFootnoteProperty); }
            set { SetValue(ShowFootnoteProperty, value); }
        }

        public override void Process()
        {
            // Start vMix
            SetUpVmix();
            SetUpAzure();
        }

        #region Azure Steps

        #region SetUpAzure
        private void SetUpAzure()
        {
            var doneIfReady = new Action(DoneIfReady);
            var createAzureProgram = new Action(delegate { CreateAzureProgramThen(doneIfReady); });

            CheckIfAzureIsReadyThen(createAzureProgram);
        }
        #endregion SetUpAzure

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
                    AppState.AzureChannelRunning = true;
                    Dispatcher.Invoke(() => StopAzureChannelThen(completeCallback));
                    return;
                }
                Dispatcher.Invoke(completeCallback);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        #endregion

        #region OtherProgramsRunningError
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
        #endregion

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
                    _azureFailed = true;
                    Fail("Canceled by user");
                    return;

                case MessageBoxResult.Yes:
                    Log("Stopping it first...");
                    // Stop Azure channel
                    _azureTimer = new Timer(state =>
                    {
                        _azureTimer.Dispose();
                        _azureService.StopChannel();
                        AppState.AzureChannelRunning = false;
                        Dispatcher.Invoke(() => callback?.Invoke());
                    }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
                    break;

                case MessageBoxResult.No:
                    callback?.Invoke();
                    break;
            }
        }
        #endregion

        #region CreateAzureProgramThen

        private void CreateAzureProgramThen(Action callback)
        {
            Log("Creating Azure program...");
            // Create Azure program
            _azureTimer = new Timer(state =>
            {
                if (_vmixFailed)
                {
                    _azureTimer.Dispose();
                    return;
                }

                _azureTimer.Dispose();
                AppState.CurrentProgram = _azureService.CreateProgram();
                Dispatcher.Invoke(() => Log("Azure program created"));

                // Signal the paralel threads we're good to go
                _azureReady = true;

                Dispatcher.Invoke(callback);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }
        #endregion

        #endregion

        #region VMix Steps

        #region SetUpVmix
        private void SetUpVmix()
        {
            var existingProcess = _vmixService.GetVmixProcess();

            if (existingProcess != null && !ShutdownVmix(existingProcess))
            {
                _vmixFailed = true;
                return;
            }

            // Delay loading preset because vMix loads slowly
            _vmixTimer = new Timer(state =>
            {
                _vmixTimer.Dispose();
                Dispatcher.Invoke(StartupVmix);
                Dispatcher.Invoke(LoadVmixPresets);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }
        #endregion

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
                MessageBox.Show(GetParentWindow(), "Could not start vMix, no error info was provided, sorry :(", "I failed",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _vmixFailed = true;
                return;
            }

            while (string.IsNullOrWhiteSpace(vMixProcess.MainWindowTitle))
            {
                Thread.Sleep(100);
                vMixProcess.Refresh();
            }

            AppState.VmixRuntime = new VmixRuntime
            {
                SessionId = vMixProcess.SessionId,
                StartTime = vMixProcess.StartTime
            };

            Log("vMix started.");
        }

        #endregion

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

            var response = MessageBox.Show(GetParentWindow(), messageBuffer.ToString(), "vMix already running", MessageBoxButton.YesNo,
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

        #region LoadVmixPresets
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
                if (_vmixFailed || _azureFailed)
                {
                    _vmixTimer.Dispose();
                    return;
                }

                if (!_vmixService.PresetLoaded()) return;

                _vmixTimer.Dispose();
                Dispatcher.Invoke(() =>
                {
                    AppState.CurrentVmixState = _vmixService.FetchState();
                    _vmixReady = true;
                    Log("Loaded vMix preset!");
                    DoneIfReady();
                });
            }, null, waitFor, repeatAfter);
        }
        #endregion

        #endregion

        private void DoneIfReady()
        {
            lock (_readyCheckLock)
            {
                if (!_azureReady || !_vmixReady) return;
            }

            //if (!_vmixReady)
            //{
            //    Log("Waiting for vMix...");
            //    _azureTimer = new Timer(state =>
            //    {
            //        if (!_vmixReady) return;
            //        DoneIfReady();
            //        _azureTimer.Dispose();
            //    }, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            //    return;
            //}

            //ShowFootnote = Visibility.Collapsed;

            SetNotBusy();
            Done();
        }
    }
}