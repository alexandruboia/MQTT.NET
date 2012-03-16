using System;

namespace Mqtt.Net
{
    public enum MqttMessageType : byte
    {
        Connect = 1,

        ConnAck = 2,

        Publish = 3,

        PubAck = 4,

        PubRec = 5,

        PubRel = 6,

        PubComp = 7,

        Subscribe = 8,

        SubAck = 9,

        Unsubscribe = 10,

        UbsubAck = 11,

        PingReq = 12,

        PingResp = 13,

        Disconnect = 14
    }
}