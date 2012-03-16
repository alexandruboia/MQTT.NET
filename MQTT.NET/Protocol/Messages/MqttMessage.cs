using System;
using System.IO;
using System.Text;

namespace Mqtt.Net
{
    public abstract class MqttMessage
    {
        protected MqttMessageType _messageType;

        protected QoSLevel _qosLevel;

        protected int _remainingLength;

        protected bool _retain;

        protected bool _dup;

        public MqttMessage(byte fixedHeader)
        {
            ReadFixedHeader(fixedHeader);
        }

        public MqttMessage(MqttMessageType messageType, QoSLevel qosLevel, bool retain, bool dup)
        {
            _qosLevel = qosLevel;
            _messageType = messageType;
            _retain = retain;
            _dup = dup;
        }

        private void ReadFixedHeader(byte fixedHeader)
        {
            _retain = (fixedHeader & 0x01) != 0;
            _qosLevel = (QoSLevel)((fixedHeader & 0x06) >> 1);
            _dup = (fixedHeader & 0x08) != 0;
            _messageType = (MqttMessageType)((fixedHeader & 0xf0) >> 4);
        }

        private void WriteFixedHeader(Stream stream)
        {
            byte _fixedHeader = 0x00;

            _fixedHeader |= (byte)((byte)_messageType << 4);
            if (_dup)
                _fixedHeader |= 0x08;
            _fixedHeader |= (byte)((byte)_qosLevel << 1);
            if (_retain)
                _fixedHeader |= 0x01;

            stream.WriteByte(_fixedHeader);
        }

        protected void ReadRemainingLength(Stream stream)
        {
            int _mul = 1;
            int _digit = 0;

            _remainingLength = 0;
            do
            {
                _digit = stream.ReadByte();
                if (_digit < 0)
                {
                    _remainingLength = 0;
                    break;
                }
                _remainingLength += ((_digit & 127) * _mul);
                _mul *= 128;
            }
            while ((_digit & 128) != 0);
        }

        protected void WriteRemainingLength(Stream stream)
        {
            byte _digit;
            int _length = _remainingLength;
            do
            {
                _digit = (byte)(_length % 128);
                _length = _length / 128;
                if (_length > 0)
                    _digit |= 0x80;
                stream.WriteByte(_digit);
            }
            while (_length > 0);
        }

        protected void WriteUShort(ushort val, Stream stream)
        {
            byte[] _buffer = new byte[2];
            _buffer[0] = (byte)(val >> 8);
            _buffer[1] = (byte)(val & 0xFF);
            stream.Write(_buffer, 0, 2);
        }

        protected ushort ReadUShort(Stream stream)
        {
            byte[] _buffer = new byte[2];
            stream.Read(_buffer, 0, 2);//0 - MSB, 1 - LSB
            return (ushort)((_buffer[0] << 8) + _buffer[1]);
        }

        protected void WriteUTF8String(string str, Stream stream)
        {
            byte[] _buffer = Encoding.UTF8.GetBytes(str);
            ushort _length = (ushort)_buffer.Length;
            
            WriteUShort(_length, stream);
            stream.Write(_buffer, 0, _length);
        }

        protected string ReadUTF8String(Stream stream)
        {
            byte[] _buffer = null;
            ushort _length = 0;

            _length = ReadUShort(stream);
            _buffer = new byte[_length];

            stream.Read(_buffer, 0, _length);
            return Encoding.UTF8.GetString(_buffer);
        }

        protected short GetUTF8StringByteLength(string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return (short)Encoding.UTF8.GetByteCount(str);
        }

        public void WriteToStream(Stream stream)
        {
            WriteFixedHeader(stream);
            ComputeRemainingLength();
            WriteRemainingLength(stream);
            EncodeMessage(stream);
        }

        public void ReadFromStream(Stream stream)
        {
            ReadRemainingLength(stream);
            DecodeMessage(stream);
        }

        protected abstract void DecodeMessage(Stream stream);

        protected abstract void EncodeMessage(Stream stream);

        protected abstract void ComputeRemainingLength();

        public bool Dup
        {
            get
            {
                return _dup;
            }
        }

        public bool Retain
        {
            get
            {
                return _retain;
            }
        }

        public QoSLevel QoSLevel
        {
            get
            {
                return _qosLevel;
            }
        }

        public int RemainingLength
        {
            get
            {
                return _remainingLength;
            }
        }

        public MqttMessageType MessageType
        {
            get
            {
                return _messageType;
            }
        }
    }
}
