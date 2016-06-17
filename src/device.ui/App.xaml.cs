#region

using System;
using System.Windows;
using forte.device.extensions;

#endregion

namespace device.ui
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += OnAppDomainUnhandledException;
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string errorMessage = $"An unrecoverable exception occurred: {e.ExceptionObject.InnerMessage(true)}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OnDispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = $"An unhandled exception occurred: {e.Exception.InnerMessage(true)}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}