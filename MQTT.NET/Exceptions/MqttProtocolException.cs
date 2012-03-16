using System;

namespace Mqtt.Net
{
    [Serializable]
    public class MqttProtocolException : Exception
    {
        public MqttProtocolException(string message)
            : base(message)
        {
            return;
        }
    }
}
