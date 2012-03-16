using System;

namespace Mqtt.Net
{
    /// <summary>
    /// Reasons for which a MQTT connection may fail
    /// </summary>
    public enum ConnectionFailureReason
    {
        /// <summary>
        /// Server could not be reached
        /// </summary>
        NetworkError = 1,

        /// <summary>
        /// Protocol version is not supported or accepted by the server
        /// </summary>
        UnacceptableProtoVersion = 2,

        /// <summary>
        /// Received if the unique client identifier is not between 1 and 23 characters in length
        /// </summary>
        IdentifierRejected = 3,

        /// <summary>
        /// Serve is not available
        /// </summary>
        ServerUnavailable = 4,

        /// <summary>
        /// Bad username and/or password provided
        /// </summary>
        BadCredentials = 5,

        /// <summary>
        /// Not authorized by the server
        /// </summary>
        NotAuthorized = 6
    }
}