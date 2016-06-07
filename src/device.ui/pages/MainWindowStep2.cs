using System.Linq;
using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        protected void ExecuteStep2Workflow()
        {
            // Set the active window to static background image
            var backgroundImageInputs =
                State.Inputs.Where(input => input.Role == InputRole.OpeninStaticImage).ToList();
            if (backgroundImageInputs.Count != 1)
            {
                MessageBox.Show("Wrong number of background image inputs specified, can't tell which one to select!",
                    "Cannot set background", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            var backgroundImageInput = backgroundImageInputs.First();
            _vmixService.SetActive(backgroundImageInput);
            Log($"Set the active input to '{backgroundImageInput.Title}', waiting for Azure Program.");
            // Set the workflow step so the user can read the instructions
            SetWorkflowStep(Workflow.ReadyForAzure);
        }
    }
}