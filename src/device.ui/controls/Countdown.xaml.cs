#region

using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace device.ui.controls
{
    /// <summary>
    ///     Interaction logic for Countdown.xaml
    /// </summary>
    public partial class Countdown : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowStopButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowStopButtonProperty =
            DependencyProperty.Register("ShowStopButton", typeof(bool), typeof(Countdown),
                new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for FootnoteText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FootnoteTextProperty =
            DependencyProperty.Register("FootnoteText", typeof(string), typeof(Countdown),
                new PropertyMetadata("Click stop to stop the counter (manual mode)"));

        // Using a DependencyProperty as the backing store for CountdownTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountdownToProperty =
            DependencyProperty.Register("CountdownTo", typeof(DateTime), typeof(Countdown),
                new PropertyMetadata(DateTime.Now.AddMinutes(5)));

        // Using a DependencyProperty as the backing store for DisplayText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText", typeof(string), typeof(Countdown));

        private readonly TimeSpan _oneSecond = TimeSpan.FromSeconds(1);

        private readonly TimeSpan _zeroSeconds = TimeSpan.FromSeconds(0);

        private bool _countdownStarted = false;
        private readonly object _countdownStartedLock = new Object();

        private Timer _timer;

        public Countdown()
        {
            InitializeComponent();
            DisplayText = "Countdown";
        }

        public string EventName { get; set; }
        public bool CanStop { get; set; }

        public string FootnoteText
        {
            get { return (string)GetValue(FootnoteTextProperty); }
            set { SetValue(FootnoteTextProperty, value); }
        }

        public DateTime CountdownTo
        {
            get { return (DateTime)GetValue(CountdownToProperty); }
            set { SetValue(CountdownToProperty, value); }
        }

        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public bool StartOnRender { get; set; }

        public bool ShowStopButton
        {
            get { return (bool)GetValue(ShowStopButtonProperty); }
            set { SetValue(ShowStopButtonProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (StartOnRender) Start();
        }

        public void Start()
        {
            lock (_countdownStartedLock)
            {
                if (_countdownStarted) return;
                _countdownStarted = true;
            }

            ShowStopButton = CanStop;
            _timer = new Timer(state =>
            {
                var expired = false;
                Dispatcher.Invoke(() =>
                {
                    if (CountdownTo >= DateTime.Now) return;
                    CounterExpired();
                    expired = true;
                });

                if (expired)
                {
                    _timer.Dispose();
                    return;
                }

                Dispatcher.Invoke(DisplayCountdownText);
            }, null, _zeroSeconds, _oneSecond);
        }

        private void DisplayCountdownText()
        {
            var timeRemaining = CountdownTo.Subtract(DateTime.Now);

            if (timeRemaining.TotalSeconds < 0)
            {
                DisplayText = "Time's up!";
                return;
            }

            var buffer = new StringBuilder();
            buffer.Append($"{timeRemaining.Hours.ToString().PadLeft(2, '0')}:");
            buffer.Append($"{timeRemaining.Minutes.ToString().PadLeft(2, '0')}:");
            buffer.Append($"{timeRemaining.Seconds.ToString().PadLeft(2, '0')}");

            DisplayText =
                string.IsNullOrWhiteSpace(EventName) ? buffer.ToString() : $"{EventName} in {buffer}";
        }

        private void CounterExpired()
        {
            ShowStopButton = false;
            OnExpire?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler OnStop;
        public event EventHandler OnExpire;

        private void StopCountdownButton_Click(object sender, RoutedEventArgs e)
        {
            ShowStopButton = false;
            _timer?.Dispose();
            DisplayText = "Counter stopped, you must click 'Next' if you want to continue";
            OnStop?.Invoke(this, EventArgs.Empty);
        }
    }
}