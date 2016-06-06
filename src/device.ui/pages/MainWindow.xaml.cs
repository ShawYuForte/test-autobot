using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using forte.device.models;
using forte.device.services;

namespace device.ui.pages
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly VMixService _vmixService = new VMixService();
        Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            Log("Initializing...");
        }

        void Log(string message)
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
        }

        void SetVisibleStep(int step)
        {
            Step0Visible = step == 0 ? Visibility.Visible : Visibility.Collapsed;
            Step1Visible = step == 1 ? Visibility.Visible : Visibility.Collapsed;
            Step2Visible = step == 2 ? Visibility.Visible : Visibility.Collapsed;
            Step3Visible = step == 3 ? Visibility.Visible : Visibility.Collapsed;
            Step4Visible = step == 4 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}