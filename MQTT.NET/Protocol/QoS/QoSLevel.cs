using System;

namespace Mqtt.Net
{
    /// <summary>
    /// Defines Quality of Service (QoS) values
    /// </summary>
    public enum QoSLevel : byte
    {
        /// <summary>
        /// The package arrives either once or not at all. A response is not expected and no retry semantics apply.
        /// </summary>
        AtMostOnce = 0,

        /// <summary>
        /// The receival of a message is acknowledged by the server with a PUBACK message. If no such ack is received, the message is resent with the DUP flag set
        /// </summary>
        AtLeastOnce = 1,

        /// <summary>
        /// The receival of a message is confirmed using a handshake. The message is guaranteed to arrive exactly once
        /// </summary>
        ExactlyOnce = 2
    }
}