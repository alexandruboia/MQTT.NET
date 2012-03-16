using System;

namespace Mqtt.Net
{
    public class CompletedArgs : EventArgs
    {
        public CompletedArgs(string topic, int messageId)
            : base()
        {
            Topic = topic;
            MessageId = messageId;
        }

        public string Topic
        {
            get;
            private set;
        }

        public int MessageId
        {
            get;
            private set;
        }
    }
}