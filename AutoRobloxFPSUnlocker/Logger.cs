using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRobloxFPSUnlocker
{
    public class Logger
    {
        public static List<LogEvent> LogEvents = new List<LogEvent>();

        public static void LogMessage(string message)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Info, message, null));
            Console.WriteLine(message);
        }
        public static void LogInfo(string message)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Info, message, null));
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogWarning(string message)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Warning, message, null));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogError(string message, Exception exception)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Error, message, exception));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogCritical(string message, Exception exception)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Critical, message, exception));
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogError(string message)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Error, message, null));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogCritical(string message)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Critical, message, null));
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void LogException(Exception exception)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Error, exception.Message, exception));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
            Console.ResetColor();
        }

        public static void SaveLog()
        {
            // Save log to file
            StringBuilder sb = new StringBuilder();
            foreach (var log in LogEvents)
            {
                sb.AppendLine($"[{log.Time}] {log.Severity.ToString().ToUpper()}: {log.Message}");
                if (log.Exception != null)
                {
                    sb.AppendLine($"Exception: {log.Exception.Message}");
                    sb.AppendLine($"Stack Trace: {log.StackTrace}");
                }
            }

            System.IO.File.WriteAllText("log.txt", sb.ToString());
        }

        public static void LogSuccess(string v)
        {
            LogEvents.Add(new LogEvent(LogSeverity.Info, v, null));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(v);
            Console.ResetColor();
        }
    }

    public class LogEvent
    {
        public DateTime Time { get; set; }
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public string StackTrace { get; set; }

        public LogEvent(LogSeverity severity, string message, Exception exception)
        {
            Time = DateTime.Now;
            Severity = severity;
            Message = message;

            if (exception != null)
            {
                Exception = exception;
                StackTrace = exception.StackTrace;
            }
        }
    }

    public enum LogSeverity
    {
        Info,
        Warning,
        Error,
        Critical,
    }

}
