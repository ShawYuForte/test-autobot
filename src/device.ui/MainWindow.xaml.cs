using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using forte.device.models;
using forte.device.services;

namespace device.ui
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public VMixState State
        {
            get { return (VMixState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(VMixState), typeof(MainWindow), new PropertyMetadata(null));


        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public string LogMessages
        {
            get { return (string)GetValue(LogMessagesProperty); }
            set { SetValue(LogMessagesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogMessages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogMessagesProperty =
            DependencyProperty.Register("LogMessages", typeof(string), typeof(MainWindow));

        private readonly VMixService _vmixService = new VMixService();
        Timer _timer;

        public MainWindow()
        {
            InitializeComponent();
            Log("Initializing...");
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            Log("Fetching vMix State...");
            IsBusy = true;
            State = _vmixService.FetchState();
            IsBusy = false;
            Log("vMix State Retrieved!");
        }

        void Log(string message)
        {
            logTextBlock.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
            logTextBlock.ScrollToEnd();
        }

        private void loadPresetButton_Click(object sender, RoutedEventArgs e)
        {
            var waitFor = TimeSpan.FromSeconds(30);
            var repeatAfter = TimeSpan.FromSeconds(10);
            IsBusy = true;
            var watch = new Stopwatch();
            watch.Start();
            Log("Loading preset (this takes a minute or more, please be patient)...");
            _vmixService.LoadPreset();
            Log("Waiting to verify preset is loaded...");

            _timer = new Timer(state =>
            {
                if (_vmixService.PresetLoaded())
                {
                    _timer.Dispose();
                    Dispatcher.Invoke(() =>
                    {
                        IsBusy = false;
                        Log($"Loaded preset ({watch.Elapsed.Seconds} s)");
                    });
                }
                else
                {
                    Dispatcher.Invoke(() => Log($"Waiting to verify preset is loaded ({watch.Elapsed.TotalSeconds} s)..."));
                }
            }, null, waitFor, repeatAfter);
        }
    }
}