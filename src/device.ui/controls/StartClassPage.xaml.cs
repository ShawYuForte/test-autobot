#region

using System;
using System.Windows.Controls;

#endregion

namespace device.ui.controls
{
    /// <summary>
    ///     Interaction logic for StartClassPage.xaml
    /// </summary>
    public partial class StartClassPage : UserControl
    {
        public StartClassPage()
        {
            InitializeComponent();
        }

        public event EventHandler OnPause;

        private void Countdown_OnStop(object sender, EventArgs e)
        {
            OnPause?.Invoke(this, EventArgs.Empty);
        }
    }
}