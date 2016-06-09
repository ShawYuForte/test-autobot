#region

using System;

#endregion

namespace device.ui.pages
{
    // TODO:
    //  - Add ability to "Hold" or "Stop"
    //  - Add instructions
    public partial class MainWindow
    {
        private void StartClassTimer()
        {
            // Wait at least 5 seconds, or the total number of seconds until 5 minutes before the class
            var seconds = Math.Max(5, (AppState.ClassStartTime - DateTime.Now).TotalSeconds - 60 * 5);

            _timer = new System.Threading.Timer(state =>
            {
                var remaining = seconds;
                Dispatcher.Invoke(() =>
                {
                    var timeSpan = TimeSpan.FromSeconds(remaining);
                    StartClassTimerDisplay = $"Starting program in {(int)timeSpan.TotalHours} hours, {timeSpan.Minutes} minutes, {timeSpan.Seconds} seconds";
                });
                if (seconds-- <= 0)
                {
                    _timer.Dispose();
                    Dispatcher.Invoke(StartAzureProgram);
                }
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
        }

        private void StartAzureProgram()
        {
            Log("Starting program!");
            StartClassTimerDisplay = "Starting program NOW!";
            _timer = new System.Threading.Timer(state =>
            {
                _timer.Dispose();
                _azureService.StartProgram();
                Dispatcher.Invoke(StartAzureProgram);
            }, null, TimeSpan.FromSeconds(0), TimeSpan.FromDays(1));
        }
    }
}