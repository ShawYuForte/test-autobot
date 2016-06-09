#region

using System.ComponentModel;
using System.Threading;
using System.Windows;
using forte.device.services;

#endregion

namespace device.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AzureService _azureService = new AzureService();
        private readonly VMixService _vmixService = new VMixService();
        private Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            Log("Initializing...");
        }

        private void Log(string message)
        {
            Logger.Log(message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //FetchState();
            //CalculateWorkflowStep();
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) => args.Result = typeof (MainWindow).Assembly.GetName().Version.ToString();
            worker.RunWorkerCompleted += (o, args) => AppTitle = $"Forte Autobot v{(string) args.Result}";
            worker.RunWorkerAsync();

            wizard.CurrentPage = StartClassPage;
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Log($"Window size changed to {Width} width and {Height} height");
        }

        private void StartClassPage_Enter(object sender, RoutedEventArgs e)
        {
            StartClassTimer();
        }
    }
}