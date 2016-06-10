﻿#region

using System;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace device.ui.controls
{
    /// <summary>
    ///     Interaction logic for StartClassPage.xaml
    /// </summary>
    public partial class StartClassPage : UserControl
    {
        // Using a DependencyProperty as the backing store for ShowPauseButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPauseButtonProperty =
            DependencyProperty.Register("ShowPauseButton", typeof (bool), typeof (StartClassPage), new PropertyMetadata(true));


        public StartClassPage()
        {
            InitializeComponent();
        }

        public bool ShowPauseButton
        {
            get { return (bool) GetValue(ShowPauseButtonProperty); }
            set { SetValue(ShowPauseButtonProperty, value); }
        }

        public event EventHandler OnPause;

        private void PauseCountdownButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPauseButton = false;
            OnPause?.Invoke(this, EventArgs.Empty);
        }
    }
}