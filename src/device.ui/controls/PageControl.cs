﻿#region

using System;
using System.Windows;
using System.Windows.Controls;
using forte.device.services;

#endregion

namespace device.ui.controls
{
    public abstract class PageControl : UserControl
    {
        protected readonly AzureService _azureService = new AzureService();
        protected readonly VMixService _vmixService = new VMixService();


        public delegate void LogEventDelegate(string message);

        public abstract void Process();

        public event LogEventDelegate OnLog;
        public event EventHandler OnDone;
        public event EventHandler OnBusy;
        public event EventHandler OnNotBusy;
        public event EventHandler OnError;

        protected void Log(string message)
        {
            OnLog?.Invoke(message);
        }

        protected void SetBusy()
        {
            OnBusy?.Invoke(this, new EventArgs());
        }

        protected void SetNotBusy()
        {
            OnNotBusy?.Invoke(this, new EventArgs());
        }

        protected void Done()
        {
            OnDone?.Invoke(this, new EventArgs());
        }

        protected void Fail(string message)
        {
            Log(message);
            OnError?.Invoke(this, new EventArgs());
        }

        protected Window GetParentWindow()
        {
            return Window.GetWindow(this);
        }

        protected void ShowInfoMessageBox(string message, string title = "Info")
        {
            MessageBox.Show(GetParentWindow(), message, title,
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        protected void ShowWarningMessageBox(string message, string title = "Warning")
        {
            MessageBox.Show(GetParentWindow(), message, title,
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}