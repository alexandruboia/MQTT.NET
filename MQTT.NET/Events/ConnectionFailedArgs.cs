using System;

namespace Mqtt.Net
{
    public class ConnectionFailedArgs : EventArgs
    {
        public ConnectionFailedArgs(ConnectionFailureReason reason)
            : base()
        {
            Reason = reason;
        }

        public ConnectionFailureReason Reason
        {
            get;
            private set;
        }
    }
}