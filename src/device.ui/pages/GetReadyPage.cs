#region

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using forte.device.models;
using Xceed.Wpf.Toolkit.Core;

#endregion

namespace device.ui.pages
{
    public partial class MainWindow
    {
        #region GetReadyPage_Next

        private void GetReadyPage_Next(object sender, CancelRoutedEventArgs cancelRoutedEventArgs)
        {
            // Cancel moving next, too much to do async
            cancelRoutedEventArgs.Cancel = true;

            // Start vMix
            if (!StartupVmix()) return;

            IsBusy = true;
            StartAzureChannelAndContinue(false);
        }

        #endregion

        private void StartAzureChannelAndContinue(bool continueOnAlreadyRunning)
        {
            Log("Starting azure channel...");
            // Start Azure channel
            _timer = new Timer(state =>
            {
                _timer.Dispose();
                if (!continueOnAlreadyRunning && _azureService.IsChannelRunning())
                {
                    Dispatcher.Invoke(StopAzureChannelAndContinue);
                    return;
                }
                _azureService.StartChannel();
                Dispatcher.Invoke(LoadVmixPresetsAndContinue);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        private void StopAzureChannelAndContinue()
        {
            Log("Azure channel already running...");

            var response =
                MessageBox.Show(this, 
                    "Looks like the Azure channel is already running, I recommend you shut it down first. If you have enough time, definitively recommended." +
                    Environment.NewLine + Environment.NewLine + "Do you want me to stop it for you?",
                    "Channel already running", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (response)
            {
                case MessageBoxResult.Cancel:
                    IsBusy = false;
                    return;

                case MessageBoxResult.Yes:
                    Log("Stopping it first...");
                    // Stop Azure channel
                    _timer = new Timer(state =>
                    {
                        _timer.Dispose();
                        _azureService.StopChannel();
                        Dispatcher.Invoke(() => StartAzureChannelAndContinue(false));
                    }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
                    break;

                case MessageBoxResult.No:
                    LoadVmixPresetsAndContinue();
                    break;
            }
        }

        private void LoadVmixPresetsAndContinue()
        {
            // Load vMix presets
            var waitFor = TimeSpan.FromSeconds(30);
            var repeatAfter = TimeSpan.FromSeconds(20);
            IsBusy = true;
            var watch = new Stopwatch();
            watch.Start();
            Log("Loading vMix preset (this takes a minute or more, please be patient)...");
            _vmixService.LoadPreset();

            _timer = new Timer(state =>
            {
                if (_vmixService.PresetLoaded())
                {
                    _timer.Dispose();
                    Dispatcher.Invoke(() =>
                    {
                        State = _vmixService.FetchState();
                        CreateAzureProgramAndContinue();
                    });
                }
                else
                {
                    Dispatcher.Invoke(() => Log($"Waiting to verify preset is loaded (been waiting for {(int)watch.Elapsed.TotalSeconds} s)..."));
                }
            }, null, waitFor, repeatAfter);

        }

        private void CreateAzureProgramAndContinue()
        {
            Log("Creating Azure program...");
            // Create Azure program
            _timer = new Timer(state =>
            {
                _timer.Dispose();
                AppState.CurrentProgram = _azureService.CreateProgram();
                Dispatcher.Invoke(StartStreamingOnVmixAndContinue);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }

        private void StartStreamingOnVmixAndContinue()
        {
            Log("Starting VMIX stream with background photo (Azure not recording yet)...");
            // Start streaming background image on vMix

            if (!ActivateOpeningStaticImage()) return;
            if (!PreviewOpeningVideo()) return;

            _vmixService.StartStreaming();
            Log("Started VMIX stream with background photo, waiting for class to start.");
            IsBusy = false;
            wizard.CurrentPage = StartClassPage;

            // Set the workflow step so the user can read the instructions
            AppState.WorkflowState = Workflow.ReadyToStartProgram;
        }

        #region ShutdownVmix

        private void ShutdownVmix()
        {
            var vMixProcess = GetVmixProcess();

            vMixProcess.Kill();
            Log("vMix automatically shut down!");
        }

        #endregion

        #region StartupVmix

        private bool StartupVmix()
        {
            var existingProcess = GetVmixProcess();

            if (existingProcess != null)
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
                if (response == MessageBoxResult.No) return false;
                ShutdownVmix();
            }
            var vMixProcess = Process.Start(new ProcessStartInfo
            {
                FileName = AppState.VmixExecutablePath,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            if (vMixProcess == null)
            {
                MessageBox.Show("Could not start vMix, no error info was provided, sorry :(", "I failed",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            AppState.VmixRuntime = new VmixRuntime
            {
                SessionId = vMixProcess.SessionId,
                StartTime = vMixProcess.StartTime
            };

            Log("vMix started.");
            return true;
        }

        #endregion

        #region GetVmixProcess

        private Process GetVmixProcess()
        {
            FileSystemInfo fileInfo = new FileInfo(AppState.VmixExecutablePath);
            var sExeName = fileInfo.Name.Replace(fileInfo.Extension, "");

            var existingProcess = Process.GetProcessesByName(sExeName).FirstOrDefault();
            
            return existingProcess;
        }

        #endregion

        private bool PreviewOpeningVideo()
        {
            VMixInput openingVideoInput;
            try
            {
                // Set the active window to static background image
                openingVideoInput = State.Inputs.Single(input => input.Role == InputRole.OpeningVideo);
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
                backgroundImageInput = State.Inputs.Single(input => input.Role == InputRole.OpeninStaticImage);
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