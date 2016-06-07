using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        protected void LoadPresets()
        {
            var waitFor = TimeSpan.FromSeconds(30);
            var repeatAfter = TimeSpan.FromSeconds(10);
            IsBusy = true;
            var watch = new Stopwatch();
            watch.Start();
            Log("Loading preset (this takes a minute or more, please be patient)...");
            _vmixService.LoadPreset();
            Log("Waiting to verify preset is loaded...");

            _timer = new Timer(state =>
            {
                if (_vmixService.PresetLoaded())
                {
                    _timer.Dispose();
                    Dispatcher.Invoke(() =>
                    {
                        IsBusy = false;
                        Log($"Loaded preset ({(int)watch.Elapsed.TotalSeconds} s)");
                        FetchState();
                    });
                }
                else
                {
                    Dispatcher.Invoke(() => Log($"Waiting to verify preset is loaded ({(int)watch.Elapsed.TotalSeconds} s)..."));
                }
            }, null, waitFor, repeatAfter);
        }

        protected void FetchState()
        {
            IsBusy = true;
            State = _vmixService.FetchState();
            if (_vmixService.PresetLoaded() && WorkflowState == Workflow.NotStarted)
            {
                SetWorkflowStep(Workflow.PresetLoadVerified);
            }
            IsBusy = false;
            Log("vMix State Retrieved!");
        }
    }
}
