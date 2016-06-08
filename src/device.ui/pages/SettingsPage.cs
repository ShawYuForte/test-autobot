using System;
using System.IO;
using System.Windows;
using Xceed.Wpf.Toolkit.Core;

namespace device.ui.pages
{
    public partial class MainWindow
    {
        private void SettingsPage_Next(object sender, CancelRoutedEventArgs e)
        {
            if (!VerifyRequiredSettings(e)) return;

            if (!VerifyVmixSettings(e)) return;

            if (!VerifyClassStartTime(e)) return;

            if (!VerifyAzureSettings(e)) return;

            Log("User confirmed settings, continuing to next step.");
        }

        #region VerifyClassStartTime
        private bool VerifyClassStartTime(CancelRoutedEventArgs e)
        {
            if (AppState.ClassStartTime > DateTime.Now) return true;

            var response =
                MessageBox.Show(
                    "You have specified a class time in the past. While it doesn't matter to me, the asset will be created with a date in the past. Are you sure you want to continue?",
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (response != MessageBoxResult.No) return true;
            e.Cancel = true;
            return false;
        }
        #endregion

        #region VerifyVmixSettings
        private bool VerifyVmixSettings(CancelRoutedEventArgs e)
        {
            if (!File.Exists(AppState.VmixExecutablePath))
            {
                e.Cancel = true;
                MessageBox.Show(
                    $"VMIX executable file '{AppState.VmixExecutablePath}' does not exist, please verify the file path.",
                    "Validation failed", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }

            if (File.Exists(AppState.VmixPresetFilePath)) return true;

            e.Cancel = true;
            MessageBox.Show(
                $"VMIX preset file '{AppState.VmixPresetFilePath}' does not exist, please verify the file path.",
                "Validation failed", MessageBoxButton.OK, MessageBoxImage.Stop);
            return false;
        }
        #endregion

        #region VerifyRequiredSettings
        private bool VerifyRequiredSettings(CancelRoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AppState.AmsAccountKey) &&
                !string.IsNullOrWhiteSpace(AppState.AmsAccountName) && !string.IsNullOrWhiteSpace(AppState.TrainerName) &&
                !string.IsNullOrWhiteSpace(AppState.ChannelName) &&
                !string.IsNullOrWhiteSpace(AppState.VmixPresetFilePath)) return true;
            e.Cancel = true;
            MessageBox.Show("All info on this page is required", "Validation failed", MessageBoxButton.OK,
                MessageBoxImage.Stop);
            return false;
        }
        #endregion

        #region VerifyAzureSettings

        private bool VerifyAzureSettings(CancelRoutedEventArgs e)
        {
            if (AppState.AmsSettingsVerified)
            {
                Log("Azure settings already verified during a previous session, skipping verification.");
                return true;
            }
            IsBusy = true;

            _timer = new System.Threading.Timer(state =>
            {
                _timer.Dispose();
                _azureService.OnLog += delegate(string message)
                {
                    Dispatcher.Invoke(() => Log(message));
                };
                if (_azureService.VerifySettings())
                {
                    Dispatcher.Invoke(OnAzureSettingsVerified);
                }
                else
                {
                    Dispatcher.Invoke(OnCouldNotVerifyAzureSettings);
                }
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));

            // Cancel for now, asynchronous calls to Azure
            e.Cancel = true;
            return false;
        }
        #endregion

        private void OnAzureSettingsVerified()
        {
            IsBusy = false;
            AppState.AmsSettingsVerified = true;
            wizard.CurrentPage = GetReadyPage;
            Log("Azure Media Service settings verified.");
        }

        private void OnCouldNotVerifyAzureSettings()
        {
            IsBusy = false;
            Log("Could not verify Azure Media Service settings!");
        }
    }
}