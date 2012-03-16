using System;

namespace Mqtt.Net
{
    public class MqttMessageFactory
    {
        public MqttMessage CreateMessage(byte header)
        {
            MqttMessage _message = null;
            MqttMessageType _messageType = (MqttMessageType)((header & 0xf0) >> 4);

            switch (_messageType)
            { 
                case MqttMessageType.Connect:
                    _message = new MqttConnectMessage(header);
                    break;
                case MqttMessageType.ConnAck:

                    break;
            }

            return _message;
        }
    }
}