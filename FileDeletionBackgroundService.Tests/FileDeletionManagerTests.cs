using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace FileDeletionBackgroundService.Tests
{
    public class FileDeletionManagerTests
    {
        private readonly string _testFilesDirectory = Path.Combine(AppContext.BaseDirectory, "Test Data");

        [OneTimeSetUp]
        public void SetupBeforeAllTests()
        {
            if (!Directory.Exists(_testFilesDirectory))
            {
                Directory.CreateDirectory(_testFilesDirectory);
            }
        }

        [SetUp]
        public void SetupBeforeEveryTest()
        {
            FileInfo[] files = new DirectoryInfo(_testFilesDirectory).GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
        }

        [Test]
        public void Delete_ReturnsExpectedNumberOfDeletedFiles()
        {
            //Create set of files in a temp directory
            CreateTestFiles();

            //Call Delete method
            ILogger logger = new InMemoryLogger("Tests", new System.Collections.Concurrent.ConcurrentBag<LogEntry>());
            var mgr = new FileDeletionManager(TimeSpan.FromSeconds(1), _testFilesDirectory, "*.*", logger);

            int nNumberOfDeletedFiles = mgr.Delete();
            //Assert the files are deleted

            Assert.That(nNumberOfDeletedFiles, Is.EqualTo(5));
        }

        private void CreateTestFiles()
        {
            int numberOfFiles = 5;

            for (int i = 0; i < numberOfFiles; i++)
            {
                string fileName = Path.GetRandomFileName();
                string filePath = Path.Combine(_testFilesDirectory, fileName);
                File.WriteAllText(filePath, $"This is temp file #{i + 1}");
            }
        }
    }
}
