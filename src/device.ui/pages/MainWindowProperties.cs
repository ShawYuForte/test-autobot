using System.Windows;
using forte.device.models;

namespace device.ui.pages
{
    public partial class MainWindow : Window
    {
        // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof (VMixState), typeof (MainWindow), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof (bool), typeof (MainWindow), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for LogMessages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogMessagesProperty =
            DependencyProperty.Register("LogMessages", typeof (string), typeof (MainWindow));

        public VMixState State
        {
            get { return (VMixState) GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool) GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public string LogMessages
        {
            get { return (string) GetValue(LogMessagesProperty); }
            set { SetValue(LogMessagesProperty, value); }
        }
    }
}