using System;

namespace Psns.Common.Logging
{
    public interface ILogger : IDisposable
    {
        void Write(LoggerEntry entry);
    }
}
