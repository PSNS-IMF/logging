using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Psns.Common.Logging
{
    public static class ILoggerExtensions
    {
        public static void GeneralInformation(this ILogger @this, 
            string message, 
            [CallerFilePath] string callerPath = null,
            [CallerMemberName] string callerName = null,
            bool includeHeader = true)
        {
            Write(@this, message, TraceEventType.Information, "General", callerPath, callerName, includeHeader);
        }

        public static void Error(this ILogger @this,
            string message,
            [CallerFilePath] string callerPath = null,
            [CallerMemberName] string callerName = null)
        {
            Write(@this, message, TraceEventType.Error, "General", callerPath, callerName);
        }

        public static void Warning(this ILogger @this,
            string message,
            [CallerFilePath] string callerPath = null,
            [CallerMemberName] string callerName = null)
        {
            Write(@this, message, TraceEventType.Warning, "General", callerPath, callerName);
        }

        public static void TracingVerbose(this ILogger @this,
            string message,
            [CallerFilePath] string callerPath = null,
            [CallerMemberName] string callerName = null)
        {
            Write(@this, message, TraceEventType.Verbose, "Tracing", callerPath, callerName);
        }

        public static void Verbose(this ILogger @this,
            string message,
            string category,
            [CallerFilePath] string callerPath = null,
            [CallerMemberName] string callerName = null)
        {
            Write(@this, message, TraceEventType.Verbose, category, callerPath, callerName);
        }

        public static void Write(this ILogger @this, 
            string message, 
            TraceEventType eventType,
            string category,
            [CallerFilePath] string callerPath = null, 
            [CallerMemberName] string callerName = null,
            bool includeHeader = true)
        {
            

            @this.Write(new LoggerEntry(category)
            {
                Severity = eventType,
                Message = FormatMessage(
                    Process.GetCurrentProcess().Id,
                    Thread.CurrentThread.ManagedThreadId,
                    callerPath,
                    callerName,
                    message,
                    includeHeader)
            });
        }

        static string FormatMessage(int processId, 
            int threadId, 
            string callerFilePath, 
            string callerMethod, 
            string message, 
            bool includeHeader)
        {
            var filePathParts = callerFilePath.Split(new char[] { '\\' });
            var format = includeHeader
                ? "Pid:{0}|Tid:{1}|File:{2}|Method:{3} -> {4}"
                : "{4}";

            return string.Format(format,
                    processId,
                    threadId,
                    filePathParts.Length > 0 ? filePathParts.Last() : null,
                    callerMethod,
                    message);
        }
    }
}