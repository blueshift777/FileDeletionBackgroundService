
using Microsoft.Extensions.Logging;

namespace FileDeletionBackgroundService.Tests
{
    public class LogEntry
    {
        public LogLevel LogLevel { get; set; }
        public string Category { get; set; } = string.Empty;
        public EventId EventId { get; set; }
        public string Message { get; set; } = string.Empty;
        public Exception Exception { get; set; } = new Exception();
        public DateTime Timestamp { get; set; }
    }
}
