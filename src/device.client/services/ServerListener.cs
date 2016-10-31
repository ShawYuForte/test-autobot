using System;
using System.Threading;
using System.Threading.Tasks;
using forte.devices.models;
using forte.services;
using Microsoft.AspNet.SignalR.Client;

namespace forte.devices.services
{
    public class ServerListener : IServerListener
    {
        private IHubProxy _deviceInteractionHubProxy;
        private HubConnection _hubConnection;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogger _logger;
        private Timer _timer;
        private bool _retry;

        public ServerListener(IConfigurationManager configurationManager, ILogger logger)
        {
            _configurationManager = configurationManager;
            _logger = logger;
        }

        public event MessageReceivedDelegate MessageReceived;

        public void Disconnect()
        {
            //_cancellationTokenSource.Cancel();
            _retry = false;
            _hubConnection.Stop();
            _hubConnection.Dispose();
        }

        private async Task ConnectAsync()
        {
            _retry = true;
            if (_hubConnection != null)
            {
                await _hubConnection.Start();
                return;
            }

            var config = _configurationManager.GetDeviceConfig();
            var serverUrl = config.Get<string>(SettingParams.ServerRootPath);
            _hubConnection = new HubConnection(serverUrl);
            _deviceInteractionHubProxy = _hubConnection.CreateHubProxy("DeviceInteractionHub");
            _deviceInteractionHubProxy.On("CommandAvailable", deviceId =>
            {
                // If not our event, ignore
                if (Guid.Parse(deviceId) != DeviceId) return;
                _logger?.Debug($"Server notified us of a command available for device {deviceId}");
                OnMessageReceived("CommandAvailable");
            });

            _hubConnection.Closed += OnHubConnectionOnClosed;
            _hubConnection.ConnectionSlow += () => _logger?.Warning("Connection slow...!");
            _hubConnection.Error += exception => _logger?.Error($"Connection error: {exception.Message}");
            _hubConnection.Reconnected += () => _logger?.Debug($"Connection re-established");
            _hubConnection.Reconnecting += () => _logger?.Debug($"Re-connecting...");
            _hubConnection.StateChanged += state => _logger?.Warning($"Connection state changed from {state.OldState} to {state.NewState}");
            _hubConnection.Received += data => _logger?.Debug($"Received {data}");
            await _hubConnection.Start();
        }

        private Guid? _deviceId;

        private Guid DeviceId
        {
            get
            {
                if (_deviceId != null) return _deviceId.Value;
                var config = _configurationManager.GetDeviceConfig();
                _deviceId = config.DeviceId;
                return _deviceId.Value;
            }
        }

        public void Connect()
        {
            try
            {
                ConnectAsync().Wait();
            }
            catch (Exception exception)
            {
                _logger?.Error(exception, "Could not connect to hub");
                OnHubConnectionOnClosed();
            }
        }

        private void OnHubConnectionOnClosed()
        {
            if (!_retry) return;
            _logger?.Debug("Connection closed, will retry in 10 seconds!");
            _timer = new Timer(state =>
            {
                _logger?.Debug("Attempting to re-connect");
                Connect();
                _timer.Dispose();
            }, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }

        public void Dispose()
        {
            try
            {
                Disconnect();
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
