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
        private CancellationTokenSource _cancellationTokenSource;
        private ClientWebSocket _socket;

        public delegate void MessageReceivedDelegate(string message);

        public event MessageReceivedDelegate MessageReceived;

        public async Task Connect()
        {
            var hubConnection = new HubConnection(ConfigurationManager.AppSettings["server:url"]);
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("DeviceInteractionHub");
            stockTickerHubProxy.On("Hello", () => OnMessageReceived("Server said hello"));
            await hubConnection.Start();
        }

        public void Disconnect()
        {
            _cancellationTokenSource.Cancel();
        }

        private void Listen()
        {
            Task.Factory.StartNew(
                async () =>
                {
                    var rcvBytes = new byte[128];
                    var rcvBuffer = new ArraySegment<byte>(rcvBytes);
                    while (true)
                    {
                        var rcvResult =
                            await _socket.ReceiveAsync(rcvBuffer, _cancellationTokenSource.Token);
                        var msgBytes = rcvBuffer
                            .Skip(rcvBuffer.Offset)
                            .Take(rcvResult.Count).ToArray();
                        var message = Encoding.UTF8.GetString(msgBytes);
                        OnMessageReceived(message);
                    }
                }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }

        public async Task Send(string message)
        {
            var sendBytes = Encoding.UTF8.GetBytes(message);
            var sendBuffer = new ArraySegment<byte>(sendBytes);
            await _socket.SendAsync(
                sendBuffer,
                WebSocketMessageType.Text,
                endOfMessage: true,
                cancellationToken: _cancellationTokenSource.Token);
        }
    }
}