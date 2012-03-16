using System;
using System.Threading;
using System.IO;
using System.Timers;

namespace Mqtt.Net
{
    public class Mqtt : IMqtt
    {
        private MqttConnectionInfo _connectionInfo = null;

        private AsyncCallback _listenCallback = null;

        private IMqttConnectionProvider _connectionProvider = null;

        private MqttMessageFactory _msgFactory = null;

        private ILogger _logger = null;

        private Stream _stream = null;

        private byte[] _headerBuffer = null;

        private bool _connected = false;

        #region Events

        public event EventHandler ConnectionSuccess;

        public event EventHandler<ConnectionFailedArgs> ConnectionFailed;

        public event EventHandler ConnectionLost;

        public event EventHandler<PublishReceivedArgs> PublishReceived;

        public event EventHandler<CompletedArgs> Subscribed;

        public event EventHandler<CompletedArgs> Published;

        public event EventHandler<CompletedArgs> Unsubscribed;

        #endregion

        public Mqtt(MqttConnectionInfo connectionInfo)
        {
            if (connectionInfo == null)
                throw new ArgumentNullException("connectionInfo");
            _connectionInfo = connectionInfo;
            _connectionProvider = new MqttConnectionProvider();
            _listenCallback = new AsyncCallback(_listenCallback);
            _msgFactory = new MqttMessageFactory();
            _headerBuffer = new byte[1];
        }

        private void OnConnected()
        {
            EventHandler connectionSuccess = ConnectionSuccess;
            if (connectionSuccess != null)
                connectionSuccess(this, EventArgs.Empty);
        }

        private void OnConnectionFailed(ConnectionFailedArgs args)
        {
            EventHandler<ConnectionFailedArgs> connectionFailed = ConnectionFailed;
            if (connectionFailed != null)
                connectionFailed(this, args);
        }

        private void OnConnectionLost()
        {
            EventHandler connectionLost = ConnectionLost;
            if (connectionLost != null)
                connectionLost(this, EventArgs.Empty);
        }

        private void HandleReceivedMessage(MqttMessage message)
        {

        }

        private void ListenCallback(IAsyncResult result)
        {
            MqttMessage _message = null;
            try
            {
                _stream.EndRead(result);
                _message = _msgFactory.CreateMessage(_headerBuffer[0]);
                if (_message != null)
                {
                    _message.ReadFromStream(_stream);
                    HandleReceivedMessage(_message);
                }
                Listen();
            }
            catch (Exception exc)
            {
                if (_logger != null)
                    _logger.LogException(exc);
            }
        }

        private void Listen()
        {
            _stream.BeginRead(_headerBuffer, 0, 1, _listenCallback, null);
        }

        private void DoConnect()
        {

        }

        public void Connect()
        {
            try
            {
                _connectionProvider.Connect(_connectionInfo.Host, _connectionInfo.Port);
                _stream = _connectionProvider.Stream;
            }
            catch (Exception exc)
            {
                if (_logger != null)
                    _logger.LogException(exc);
            }
            finally
            {
                if (_connectionProvider.Connected)
                    DoConnect();
                else
                    OnConnectionFailed(new ConnectionFailedArgs(ConnectionFailureReason.NetworkError));
            }
        }

        public void Disconnect()
        {
            if (!Connected)
                return;
            _connected = false;
            _stream = null;
            _connectionProvider.Disconnect();
        }

        public void Publish()
        {
            throw new NotImplementedException();
        }

        public void Subscribe()
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe()
        {
            throw new NotImplementedException();
        }

        public bool Connected
        {
            get
            {
                return _connected;
            }
        }
    }
}