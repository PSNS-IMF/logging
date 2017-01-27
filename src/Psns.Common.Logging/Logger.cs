using System.Diagnostics;
using NLog;
using iLogger = Psns.Common.Logging.ILogger;

namespace Psns.Common.Logging
{
    public class Logger : iLogger
    {
        public void Write(LoggerEntry entry)
        {
            LogManager.ThrowExceptions = true;
            var logger = LogManager.GetLogger(entry.Category);
            var logLevel = LogLevel.Off;

            switch(entry.Severity)
            {
                case TraceEventType.Critical:
                    logLevel = LogLevel.Fatal;
                    break;

                case TraceEventType.Error:
                    logLevel = LogLevel.Error;
                    break;

                case TraceEventType.Warning:
                    logLevel = LogLevel.Warn;
                    break;

                case TraceEventType.Information:
                    logLevel = LogLevel.Info;
                    break;

                case TraceEventType.Verbose:
                    logLevel = LogLevel.Debug;
                    break;
            }

            logger.Log(logLevel, entry.Message);
        }
    }
}