using System;
using System.IO;

namespace Mqtt.Net
{
    public interface IMqttConnectionProvider
    {        
        void Connect(string host, int port);

        void Disconnect();

        Stream Stream
        {
            get;
        }

        bool Connected
        { 
            get;
        }
    }
}