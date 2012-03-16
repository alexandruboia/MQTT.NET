using System;
using System.IO;

namespace Mqtt.Net
{
    public class MqttConnectMessage : MqttMessage
    {
        private byte[] _protoDescription = new byte[] {
            0, 
            6,
            (byte)'M',
            (byte)'Q',
            (byte)'i',
            (byte)'s',
            (byte)'d',
            (byte)'p',
            3
        };

        private QoSLevel _willQos;

        private bool _willRetain = false;

        private string _willTopic = null;

        private byte[] _willMessage = null;

        private string _userName = null;

        private string _password = null;

        private bool _cleanSession = false;
        
        private ushort _keepAliveTimer = 0;

        private string _clientId = null;

        private bool _hasWill = false;

        private bool _hasUserName = false;

        private bool _hasPassword = false;
        
        public MqttConnectMessage(string clientId, bool cleanSession, ushort keepAliveTimer)
        {
            _clientId = clientId;
            _cleanSession = cleanSession;
            _keepAliveTimer = keepAliveTimer;
        }

        public MqttConnectMessage(string clientId, bool cleanSession, ushort keepAliveTimer, string willTopic, byte[] willMessage, QoSLevel willQos, bool willRetain)
            : this(clientId, cleanSession, keepAliveTimer)
        {
            _willTopic = willTopic;
            if (!string.IsNullOrEmpty(_willTopic))
            {
                _willQos = willQos;
                _willRetain = willRetain;
                _willMessage = willMessage;
                _hasWill = true;
            }
        }
        
        public MqttConnectMessage(byte fixedHeader) 
            : base(fixedHeader)
        {
            return;
        }

        protected override void ComputeRemainingLength()
        {
            _remainingLength = 12;//variable header length for the connect message
            _remainingLength += _clientId.Length + 2;
            if (_hasWill)
            {
                _remainingLength += _willTopic.Length + 2;
                if (_willMessage != null)
                    _remainingLength += _willMessage.Length;
                _remainingLength += 2;
            }
        }

        protected override void EncodeMessage(Stream stream)
        {
            if (_remainingLength == 0)
                return;
            stream.Write(_protoDescription, 0, _protoDescription.Length);
        }

        protected override void DecodeMessage(Stream stream)
        {
            throw new NotImplementedException();
        }

        public string UserName
        {
            set
            {
                _userName = value;
                _hasUserName = !string.IsNullOrEmpty(_userName);
            }
            get
            {
                return _userName;
            }
        }

        public string Password
        {
            set
            {
                _password = value;
                _hasPassword = !string.IsNullOrEmpty(_password);
            }
            get
            {
                return _password;
            }
        }
    }
}