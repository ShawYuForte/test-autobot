#region

using System.Windows;
using System.Windows.Controls;

#endregion

namespace device.ui.controls
{
    /// <summary>
    ///     Interaction logic for StopClassPage.xaml
    /// </summary>
    public partial class StopClassPage : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowPauseButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPauseButtonProperty =
            DependencyProperty.Register("ShowPauseButton", typeof (bool), typeof (StopClassPage),
                new PropertyMetadata(false));

        public StopClassPage()
        {
            InitializeComponent();
        }

        public bool ShowPauseButton
        {
            get { return (bool) GetValue(ShowPauseButtonProperty); }
            set { SetValue(ShowPauseButtonProperty, value); }
        }
    }
}