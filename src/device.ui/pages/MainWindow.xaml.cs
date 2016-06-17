#region

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using forte.device.services;
using Xceed.Wpf.Toolkit.Core;

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
                    //GetReadyPage_Next(sender, e);
                    WizardGetReadyPage.Process();
                    e.Cancel = true;
                    break;

                case nameof(StartClassPage):
                    //StartClassPage_Next(sender, e);
                    
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
                var response = MessageBox.Show(this, "Start over?", "Confirm", MessageBoxButton.YesNo,
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
            WizardStartClassPage.Process();
        }

        private void LastPage_Enter(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentProgram != null) Clipboard.SetText(AppState.CurrentProgram.PublishUrl);
        }

        private void wizard_Finish(object sender, RoutedEventArgs e)
        {
            AppState.Reset();
        }

        private void TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Log($"Size changed to {e.NewSize.Width} and {e.NewSize.Height}");
        }

        private void wizard_Help(object sender, RoutedEventArgs e)
        {
            var settingsPage = new SettingsWindow();
            settingsPage.ShowDialog();
        }

        private void WizardGetReadyPage_OnDone(object sender, EventArgs e)
        {
            wizard.CurrentPage = WaitForChannelStartPage;
            Log("Auto advancing to next step, waiting for channel to start.");
        }

        private void WizardGetReadyPage_OnError(object sender, EventArgs e)
        {
        }

        private void WizardPage_OnBusy(object sender, EventArgs e)
        {
            IsBusy = true;
        }

        private void WizarPage_OnNotBusy(object sender, EventArgs e)
        {
            IsBusy = false;
        }

        private void WizardWaitForChannelStartPage_OnDone(object sender, EventArgs e)
        {
            Log("Auto advancing to next step, waiting for class to start.");
            wizard.CurrentPage = StartClassPage;
        }

        private void WizardWaitForChannelStartPage_OnError(object sender, EventArgs e)
        {

        }

        private void WaitForChannelStartPage_Enter(object sender, RoutedEventArgs e)
        {
            WizardWaitForChannelStartPage.Process();
        }

        private void WizardStartClassPage_OnError(object sender, EventArgs e)
        {

        }

        private void WizardStartClassPage_OnDone(object sender, EventArgs e)
        {
            Log("Auto advancing to next step, waiting for class to end.");
            wizard.CurrentPage = StopClassPage;
        }

        private void StopClassPage_Next(object sender, CancelRoutedEventArgs cancelRoutedEventArgs)
        {
            cancelRoutedEventArgs.Cancel = true;
            WizardStopClassPage.ForceContinue();
        }

        private void WizardStopClassPage_OnDone(object sender, EventArgs e)
        {
            wizard.CurrentPage = LastPage;
        }

        private void StopClassPage_Enter(object sender, RoutedEventArgs e)
        {
            WizardStopClassPage.Process();
        }
    }
}