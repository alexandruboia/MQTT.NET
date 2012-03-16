using System;

namespace Mqtt.Net
{
    public class PublishReceivedArgs : EventArgs
    {
        public PublishReceivedArgs(string topic, MqttPayload payload) 
            : base()
        {
            Topic = topic;
            Payload = payload;
        }

        public string Topic
        {
            get;
            private set;
        }

        public MqttPayload Payload
        {
            get;
            private set;
        }
    }
}