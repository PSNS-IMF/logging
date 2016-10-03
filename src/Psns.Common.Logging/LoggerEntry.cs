using System;
using System.Diagnostics;

namespace Psns.Common.Logging
{
    public class LoggerEntry
    {
        public LoggerEntry(string category)
        {
            if(category == null)
                throw new ArgumentNullException("category must be provided");

            Category = category;
            Message = string.Empty;
            UserName = string.Empty;
            Severity = TraceEventType.Information;
        }

        public int EventId { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public int Priority { get; set; }
        public string UserName { get; set; }
        public TraceEventType Severity { get; set; }
    }
}
