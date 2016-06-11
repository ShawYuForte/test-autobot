using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using forte.device.models;

namespace device.ui.pages
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public AppSettings Settings
        {
            get { return (AppSettings)GetValue(SettingsProperty); }
            set { SetValue(SettingsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Settings.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.Register("Settings", typeof(AppSettings), typeof(SettingsWindow), new PropertyMetadata(null));

        public SettingsWindow()
        {
            InitializeComponent();

            Settings = AppSettings.Instance.Copy();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            AppSettings.Instance.OverrideWith(Settings);
            Close();
        }
    }
}
