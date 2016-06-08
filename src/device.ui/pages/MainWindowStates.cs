using System.Linq;
using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        public AppState AppState => AppState.Instance;

        private void SetWorkflowStep(Workflow step)
        {
            AppState.WorkflowState = step;
        }
    }
}