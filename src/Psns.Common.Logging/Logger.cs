using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using AutoMapper;

namespace Psns.Common.Logging
{
    public class Logger : ILogger
    {
        LogWriter _writer;

        public Logger()
        {
            _writer = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
            Mapper.CreateMap<LoggerEntry, LogEntry>();
        }

        public void Write(LoggerEntry loggerEntry)
        {
            var logEntry = new LogEntry();
            logEntry.Categories.Add(loggerEntry.Category);
            logEntry.ExtendedProperties.Add("username", loggerEntry.UserName);

            _writer.Write(Mapper.Map<LoggerEntry, LogEntry>(loggerEntry, logEntry));
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
