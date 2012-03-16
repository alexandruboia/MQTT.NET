using System;
using System.IO;

namespace Mqtt.Net
{
    public interface IMqtt
    {
        #region Events

        event EventHandler ConnectionSuccess;

        event EventHandler<ConnectionFailedArgs> ConnectionFailed;

        event EventHandler ConnectionLost;

        event EventHandler<PublishReceivedArgs> PublishReceived;

        event EventHandler<CompletedArgs> Subscribed;

        event EventHandler<CompletedArgs> Published;

        event EventHandler<CompletedArgs> Unsubscribed;

        #endregion

        #region Methods

        void Connect();

        void Disconnect();

        void Publish();

        void Subscribe();

        void Unsubscribe();

        #endregion

        #region Properties

        bool Connected { get; }

        #endregion
    }
}