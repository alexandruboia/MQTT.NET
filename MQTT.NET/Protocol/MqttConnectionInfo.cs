using System;
using System.Net;

namespace Mqtt.Net
{
    public class MqttConnectionInfo
    {
        private string _host;

        private int _port;

        private string _userName;

        private string _password;

        private string _clientId;

        private string _willTopic = string.Empty;

        private string _willMessage = string.Empty;

        private QoSLevel _willQoS;

        private bool _clean = true;

        private ushort _keepAliveInterval;

        public MqttConnectionInfo(string clientId, string host, int port)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException("clientId");
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");
            if (port <= 0)
                throw new ArgumentException("Invalid port");

            _host = host;
            _port = port;
            _clientId = clientId;
            _willQoS = QoSLevel.AtMostOnce;
            _keepAliveInterval = 10;
        }

        public MqttConnectionInfo(string clientId, string host, int port, bool clean) 
            : this(clientId, host, port)
        {
            _clean = clean;
        }

        public string WillTopic
        {
            get
            {
                return _willTopic;
            }
            set
            {
                _willTopic = value;
            }
        }

        public string WillMessage
        {
            get
            {
                return _willMessage;
            }
            set
            {
                _willMessage = value;
            }
        }

        public QoSLevel WillQos
        {
            get
            {
                return _willQoS;
            }
            set
            {
                _willQoS = value;
            }
        }

        public ushort KeepAliveInterval
        {
            get
            {
                return _keepAliveInterval;
            }
            set
            {
                _keepAliveInterval = value;
            }
        }

        public bool HasWill
        {
            get
            {
                return !string.IsNullOrEmpty(_willTopic);
            }
        }

        public bool HasUserName
        {
            get
            {
                return !string.IsNullOrEmpty(_userName);
            }
        }

        public bool HasPassword
        {
            get
            {
                return !string.IsNullOrEmpty(_password);
            }
        }

        public bool Clean
        {
            get
            {
                return _clean;
            }
        }

        public string ClientId
        {
            get
            {
                return _clientId;
            }
        }

        public string Host
        {
            get
            {
                return _host;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }
    }
}