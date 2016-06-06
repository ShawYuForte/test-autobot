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

        // Using a DependencyProperty as the backing store for Step0Visible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Step0VisibleProperty =
            DependencyProperty.Register("Step0Visible", typeof (Visibility), typeof (MainWindow),
                new PropertyMetadata(Visibility.Visible));

        // Using a DependencyProperty as the backing store for Step1Visible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Step1VisibleProperty =
            DependencyProperty.Register("Step1Visible", typeof (Visibility), typeof (MainWindow),
                new PropertyMetadata(Visibility.Collapsed));

        // Using a DependencyProperty as the backing store for Step2Visible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Step2VisibleProperty =
            DependencyProperty.Register("Step2Visible", typeof (Visibility), typeof (MainWindow),
                new PropertyMetadata(Visibility.Collapsed));

        // Using a DependencyProperty as the backing store for Step3Visible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Step3VisibleProperty =
            DependencyProperty.Register("Step3Visible", typeof (Visibility), typeof (MainWindow),
                new PropertyMetadata(Visibility.Collapsed));

        // Using a DependencyProperty as the backing store for Step4Visible.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Step4VisibleProperty =
            DependencyProperty.Register("Step4Visible", typeof (Visibility), typeof (MainWindow),
                new PropertyMetadata(Visibility.Collapsed));

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

        public Visibility Step0Visible
        {
            get { return (Visibility)GetValue(Step0VisibleProperty); }
            set { SetValue(Step0VisibleProperty, value); }
        }

        public Visibility Step1Visible
        {
            get { return (Visibility) GetValue(Step1VisibleProperty); }
            set { SetValue(Step1VisibleProperty, value); }
        }

        public Visibility Step2Visible
        {
            get { return (Visibility) GetValue(Step2VisibleProperty); }
            set { SetValue(Step2VisibleProperty, value); }
        }

        public Visibility Step3Visible
        {
            get { return (Visibility) GetValue(Step3VisibleProperty); }
            set { SetValue(Step3VisibleProperty, value); }
        }


        public Visibility Step4Visible
        {
            get { return (Visibility) GetValue(Step4VisibleProperty); }
            set { SetValue(Step4VisibleProperty, value); }
        }
    }
}