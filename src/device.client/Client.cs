#region

using System;
using System.Configuration;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

#endregion

namespace device.client
{
    public class Client
    {
        private HubConnection _hubConnection;

        public delegate void MessageReceivedDelegate(string message);

        public event MessageReceivedDelegate MessageReceived;
        private IHubProxy _stockTickerHubProxy;

        public async Task Connect()
        {
             _hubConnection = new HubConnection(ConfigurationManager.AppSettings["server:url"]);
             _stockTickerHubProxy = _hubConnection.CreateHubProxy("DeviceInteractionHub");
            _stockTickerHubProxy.On("Hello", (message) =>
            {
                OnMessageReceived($"Server said {message}");
            });
            _stockTickerHubProxy.On("RequestState", (deviceId) =>
            {
                OnMessageReceived($"Server requested state for device id {deviceId}");
            });
            await _hubConnection.Start();
        }

        public void Disconnect()
        {
            //_cancellationTokenSource.Cancel();
            _hubConnection.Dispose();
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }

        public async Task Send(string message)
        {
            await _stockTickerHubProxy.Invoke("SendHello", message);
        }
    }
}