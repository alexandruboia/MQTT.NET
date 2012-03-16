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

        private bool _hasWill = false;

        private QoSLevel _willQos;

        private bool _willRetain = false;

        private string _willTopic = null;

        private byte[] _willMessage = null;

        private string _userName = null;

        private string _password = null;

        private bool _hasUserName = false;

        private bool _hasPassword = false;

        private bool _cleanSession = false;
        
        private ushort _keepAliveTimer = 0;

        private string _clientId = null;
        
        public MqttConnectMessage(string clientId, bool cleanSession, ushort keepAliveTimer)
            : base(MqttMessageType.Connect, (QoSLevel)0, false, false)//QoS, DUP and RETAIN are not used
        {
            if (clientId != null && clientId.Length > 23)
                throw new MqttProtocolException("Client id cannot be longer than 23 characters, as per protocol specification");
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

        private void WriteConnectFlags(Stream stream)
        {
            byte connectionFlags = 0x00;

            if (_hasUserName)
                connectionFlags |= 0x80;
            if (_hasPassword)
                connectionFlags |= 0x40;
            if (_hasWill)
            {
                if (_willRetain)
                    connectionFlags |= 0x20;
                connectionFlags |= (byte)((byte)_willQos << 3);
                connectionFlags |= 0x04;
            }
            if (_cleanSession)
                connectionFlags |= 0x02;
        }

        protected override void ComputeRemainingLength()
        {
            //variable header length for the connect message
            _remainingLength = 12;
            //client id length
            _remainingLength += GetUTF8StringByteLength(_clientId) + 2;

            //if the will exists, add the length of both the will topic and will message
            if (_hasWill)
            {
                _remainingLength += _willTopic.Length + 2;
                if (_willMessage != null)
                    _remainingLength += _willMessage.Length;
                _remainingLength += 2;
            }

            //username length, if any
            if (_hasUserName)
                _remainingLength += GetUTF8StringByteLength(_userName) + 2;
            //password length, if any
            if (_hasPassword)
                _remainingLength += GetUTF8StringByteLength(_password) + 2;
        }

        protected override void EncodeMessage(Stream stream)
        {
            if (_remainingLength == 0)
                return;

            //write protocol description and version
            stream.Write(_protoDescription, 0, _protoDescription.Length);
            //write connect flags
            WriteConnectFlags(stream);
            //write the keepalive timer
            WriteUShort(_keepAliveTimer, stream);
        }

        protected override void DecodeMessage(Stream stream)
        {
            throw new MqttNotImplementedException("MqttConnectMessage decoding is not supported");
        }

        public void SetUserNameAndPassword(string userName, string password)
        {
            bool hasUserName = !string.IsNullOrEmpty(userName);
            bool hasPassword = !string.IsNullOrEmpty(password);
            
            if (!hasUserName && hasPassword)
                throw new MqttProtocolException("It is invalid to set a password without setting a username, as per protocol specification");
            
            _userName = userName;
            _hasUserName = hasUserName;

            _password = password;
            _hasPassword = hasPassword;
        }
        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
        }
    }
}