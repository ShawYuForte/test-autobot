using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        protected void LoadPresets()
        {
        }

        protected void FetchState()
        {
            IsBusy = true;
            State = _vmixService.FetchState();
            if (_vmixService.PresetLoaded() && AppState.WorkflowState == Workflow.NotStarted)
            {
                //SetWorkflowStep(Workflow.PresetLoadVerified);
            }
            IsBusy = false;
            Log("vMix State Retrieved!");
        }
    }
}
