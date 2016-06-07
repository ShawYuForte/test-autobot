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
    }
}