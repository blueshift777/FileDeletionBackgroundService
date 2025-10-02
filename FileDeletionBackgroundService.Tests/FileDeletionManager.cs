
using Microsoft.Extensions.Logging;
using System.Timers;

namespace FileDeletionBackgroundService.Tests
{
    public class FileDeletionManager
    {
        private readonly System.Timers.Timer _fileDeletionTimer = new System.Timers.Timer();
        private readonly TimeSpan _fileDeletionInterval;
        private readonly ILogger _logger;
        private readonly string _directoryPath;
        private readonly string _fileSearchPattern = "*.*";

        public FileDeletionManager(TimeSpan fileDeletionInterval,
                                   string directoryPath,
                                   string fileSearchPattern,
                                   ILogger logger)
        {
            _fileDeletionInterval = fileDeletionInterval;
            _logger = logger;
            _directoryPath = directoryPath;
            _fileSearchPattern = fileSearchPattern;

            _fileDeletionTimer = new System.Timers.Timer(_fileDeletionInterval.TotalMilliseconds);
            _fileDeletionTimer.Elapsed += OnDeletionTimerElapsed;
        }

        private void OnDeletionTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Delete();
        }

        public bool Start()
        {
            _fileDeletionTimer.Start();

            return _fileDeletionTimer.Enabled;
        }

        public bool Stop()
        {
            _fileDeletionTimer.Stop();

            return !_fileDeletionTimer.Enabled;

        }


        public int Delete()
        {
            int numberOfDeletedFiles = 0;

            var directoryInfo = new DirectoryInfo(_directoryPath);
            FileInfo[] files = directoryInfo.GetFiles(_fileSearchPattern);
            foreach (FileInfo file in files)
            {
                try
                {
                    file.Delete();
                    numberOfDeletedFiles++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting file {FileName}", file.FullName);
                }
            }


            return numberOfDeletedFiles;
        }

    }
}
