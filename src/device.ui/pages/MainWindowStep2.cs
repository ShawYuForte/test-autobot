using System;
using System.Linq;
using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        protected void ExecuteStep2Workflow()
        {
        }

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