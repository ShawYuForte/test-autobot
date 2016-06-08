using System;
using System.Threading;
using System.Windows;
using forte.device.services;

namespace device.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly VMixService _vmixService = new VMixService();
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            Log("Initializing...");
        }

        private void Log(string message)
        {
            logTextBlock.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
            logTextBlock.ScrollToEnd();
        }

        private void loadPresetButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPresets();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FetchState();
            CalculateWorkflowStep();
        }

        private void startAzureButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStep2Workflow();
        }

        private void startStreamButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStep3Workflow();
        }

        private void endStreamButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStep4Workflow();
        }

        private void Wizard_Next(object sender, Xceed.Wpf.Toolkit.Core.CancelRoutedEventArgs e)
        {
            switch (wizard.CurrentPage.Name)
            {
                case "IntroPage":
                    if (string.IsNullOrWhiteSpace(AppState.AmsAccountKey) ||
                        string.IsNullOrWhiteSpace(AppState.AmsAccountName) ||
                        string.IsNullOrWhiteSpace(AppState.TrainerName) ||
                        string.IsNullOrWhiteSpace(AppState.ChannelName))
                    {
                        e.Cancel = true;
                        MessageBox.Show("All info on this page is required", "Validation failed", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;

                case "PresetPage":
                    if (!_vmixService.PresetLoaded())
                    {
                        e.Cancel = true;
                        MessageBox.Show("Load presets before continuing", "Validation failed", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
            }
        }

        private void PresetPage_Enter(object sender, RoutedEventArgs e)
        {
            
        }

        private void PresetPage_Leave(object sender, RoutedEventArgs e)
        {

        }

        private void IntroPage_Leave(object sender, RoutedEventArgs e)
        {
            AppState.WorkflowState = forte.device.models.Workflow.AzureInformationConfirmed;
        }
    }
}