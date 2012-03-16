using System;
using System.Text;

namespace Mqtt.Net
{
    public class MqttPayload
    {
        public byte[] _buffer;

        public MqttPayload()
        {
            _buffer = new byte[0];
        }

        public MqttPayload(string payload)
        {
            if (!string.IsNullOrEmpty(payload))
                _buffer = Encoding.UTF8.GetBytes(payload);
            else
                _buffer = new byte[0];
        }

        public MqttPayload(byte[] payload)
        {
            if (payload == null)
                _buffer = new byte[0];
            else
                _buffer = payload;
        }

        public byte[] Buffer
        {
            get
            {
                return _buffer;
            }
        }

        public static MqttPayload Empty
        {
            get
            {
                return new MqttPayload();
            }
        }
    }
}
