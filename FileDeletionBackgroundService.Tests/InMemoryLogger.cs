
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace FileDeletionBackgroundService.Tests
{
    public class InMemoryLogger : ILogger
    {
        private readonly string _category;
        private readonly ConcurrentBag<LogEntry> _logEntries;

        public InMemoryLogger(string category, ConcurrentBag<LogEntry> logEntries)
        {
            _category = category;
            _logEntries = logEntries;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            _logEntries.Add(new LogEntry
            {
                LogLevel = logLevel,
                Category = _category,
                EventId = eventId,
                Message = message,
                Exception = exception,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
