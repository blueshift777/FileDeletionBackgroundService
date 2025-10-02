
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace FileDeletionBackgroundService.Tests
{
    public class InMemoryLoggerProvider : ILoggerProvider
    {
        public ConcurrentBag<LogEntry> LogEntries { get; } = new();

        public ILogger CreateLogger(string categoryName)
        {
            return new InMemoryLogger(categoryName, LogEntries);
        }

        public void Dispose() { }
    }
}
