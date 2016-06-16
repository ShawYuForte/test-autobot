#region

using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using forte.device.models;

#endregion

namespace device.ui.controls.pages
{
    /// <summary>
    ///     Interaction logic for StopClassPage.xaml
    /// </summary>
    public partial class StopClassPage : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowPauseButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPauseButtonProperty =
            DependencyProperty.Register("ShowPauseButton", typeof (bool), typeof (StopClassPage),
                new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for CountdownDisplay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CountdownDisplayProperty =
            DependencyProperty.Register("CountdownDisplay", typeof (string), typeof (StopClassPage),
                new PropertyMetadata(null));

        private Timer _timer;

        public StopClassPage()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            StartTimer();
        }

        public string CountdownDisplay
        {
            get { return (string) GetValue(CountdownDisplayProperty); }
            set { SetValue(CountdownDisplayProperty, value); }
        }

        public bool ShowPauseButton
        {
            get { return (bool) GetValue(ShowPauseButtonProperty); }
            set { SetValue(ShowPauseButtonProperty, value); }
        }

        public event EventHandler ReadyForNext;

        private void StartTimer()
        {
            var classStartTime = AppState.Instance.ClassStartTime;
            var classDuration = AppState.Instance.ClassDuration;
            var classEndTime = AppState.Instance.ClassStartTime.AddMinutes(classDuration);
            var countDownSeconds = (classEndTime - DateTime.Now).TotalSeconds;

            _timer = new Timer(state =>
            {
                var timespanToEnd = TimeSpan.FromSeconds(countDownSeconds);
                var message =
                    $"Class ending in {timespanToEnd.Hours} h : {timespanToEnd.Minutes} m : {timespanToEnd.Seconds} s";
                Dispatcher.Invoke(() => CountdownDisplay = message);
                if (countDownSeconds-- > 0) return;
                _timer.Dispose();
                Dispatcher.Invoke(InitiateReadyForNext);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
        }

        private void InitiateReadyForNext()
        {
            ShowPauseButton = false;
            CountdownDisplay = "Class ended";
            OnReadyForNext();
        }

        public void PauseCountdownButton_Click(object source, RoutedEventArgs e)
        {
            ShowPauseButton = false;
            _timer.Dispose();
            CountdownDisplay = "Timer stopped, you must manually click 'Next' when ready!";
        }

        protected virtual void OnReadyForNext()
        {
            ReadyForNext?.Invoke(this, EventArgs.Empty);
        }
    }
}