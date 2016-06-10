#region

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace device.ui.controls
{
    /// <summary>
    ///     Interaction logic for SummaryPage.xaml
    /// </summary>
    public partial class SummaryPage : UserControl
    {
        // Using a DependencyProperty as the backing store for FinishImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FinishImageSourceProperty =
            DependencyProperty.Register("FinishImageSource", typeof (string), typeof (SummaryPage),
                new PropertyMetadata(null));

        public SummaryPage()
        {
            InitializeComponent();

            var random = new Random().Next(1, 4);
            FinishImageSource = $"/images/end/end{random}.jpg";
        }

        public string FinishImageSource
        {
            get { return (string) GetValue(FinishImageSourceProperty); }
            set { SetValue(FinishImageSourceProperty, value); }
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"Image resized to {e.NewSize.Width} {e.NewSize.Height}");
        }
    }
}