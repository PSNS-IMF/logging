using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Psns.Common.Logging
{
    public class Logger : ILogger
    {
        LogWriter _writer;

        public Logger()
        {
            _writer = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
        }

        public void Write(LoggerEntry loggerEntry)
        {
            var logEntry = new LogEntry
            {
                EventId = loggerEntry.EventId,
                Message = loggerEntry.Message,
                Priority = loggerEntry.Priority,
                Severity = loggerEntry.Severity
            };

            logEntry.Categories.Add(loggerEntry.Category);
            logEntry.ExtendedProperties.Add("username", loggerEntry.UserName);

            _writer.Write(logEntry);
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) 
            {
                if (_writer != null)
                {
                    _writer.Dispose();
                    _writer = null;
                }
            }
        }
        #endregion
    }
}
