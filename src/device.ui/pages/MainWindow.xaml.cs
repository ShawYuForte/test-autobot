using System;
using System.Threading;
using System.Windows;
using forte.device.services;
using Xceed.Wpf.Toolkit.Core;

namespace device.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly VMixService _vmixService = new VMixService();
        private readonly AzureService _azureService = new AzureService();
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
            //FetchState();
            //CalculateWorkflowStep();
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
                case nameof(SettingsPage):
                    SettingsPage_Next(sender, e);
                    break;

                case nameof(GetReadyPage):
                    GetReadyPage_Next(sender, e);
                    break;
            }
        }

        private void PresetPage_Enter(object sender, RoutedEventArgs e)
        {
            
        }

        private void PresetPage_Leave(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsPage_Leave(object sender, RoutedEventArgs e)
        {
            AppState.WorkflowState = forte.device.models.Workflow.SettingsConfirmed;
        }

        private void wizard_Cancel(object sender, RoutedEventArgs e)
        {
            if (AppState.WorkflowState != forte.device.models.Workflow.NotStarted)
            {
                var response = MessageBox.Show("Start over?", "Confirm", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (response == MessageBoxResult.No) return;
            }
            AppState.Reset();
        }
    }
}