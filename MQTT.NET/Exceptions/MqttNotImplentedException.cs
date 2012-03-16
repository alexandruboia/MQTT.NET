using System;

namespace Mqtt.Net
{
    [Serializable]
    public class MqttNotImplementedException : Exception
    {
        public MqttNotImplementedException(string message)
            : base(message)
        {
            return;
        }
    }
}
