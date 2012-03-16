using System;

namespace Mqtt.Net
{
    public interface ILogger
    {
        void Enable();

        void Disable();

        void LogException(Exception exc);

        void LogError(string error);

        void LogWarning(string warning);

        void LogInfo(string info);

        void LogDebug(string info);
    }
}