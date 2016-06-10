#region

using System;
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
            AppState.Reset();
            StartupPageImageSource = $"/images/start/start{new Random().Next(1, 3)}.jpg";
        }

        private void Log(string message)
        {
            Logger.Log(message);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) => args.Result = typeof (MainWindow).Assembly.GetName().Version.ToString();
            worker.RunWorkerCompleted += (o, args) => AppTitle = $"Forte Autobot v{(string) args.Result}";
            worker.RunWorkerAsync();
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

                case nameof(StartClassPage):
                    StartClassPage_Next(sender, e);
                    break;

                case nameof(StopClassPage):
                    StopClassPage_Next(sender, e);
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
            //Log($"Window size changed to {Width} width and {Height} height");
        }

        private void StartClassPage_Enter(object sender, RoutedEventArgs e)
        {
            StartClassTimer();
        }

        private void StartClassPage_OnPause(object sender, System.EventArgs e)
        {
            PauseClassTimer();
        }

        private void StopClassPage_ReadyForNext(object sender, System.EventArgs e)
        {
            InitiateClassStopping();
        }

        private void LastPage_Enter(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentProgram != null) Clipboard.SetText(AppState.CurrentProgram.AssetUrl);
        }

        private void wizard_Finish(object sender, RoutedEventArgs e)
        {
            AppState.Reset();
        }

        private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Log($"Size changed to {e.NewSize.Width} and {e.NewSize.Height}");
        }
    }
}