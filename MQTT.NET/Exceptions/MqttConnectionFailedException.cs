using System;

namespace Mqtt.Net
{
    [Serializable]
    public class MqttConnectionFailedException : Exception
    {
        public MqttConnectionFailedException(string message)
            : base(message)
        {
            return;
        }
    }
}
