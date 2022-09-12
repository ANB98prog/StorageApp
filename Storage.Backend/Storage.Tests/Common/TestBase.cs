using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class TestBase : IDisposable
    {
        protected readonly IFileService FileService;

        protected readonly string TestFilesDirectory = Path.Combine(Environment.CurrentDirectory, "test");


        public TestBase()
        {
            if (!Directory.Exists(TestFilesDirectory))
                Directory.CreateDirectory(TestFilesDirectory);

            FileService = Factory.CreateLocalFileStorageService();
        }

        public void Dispose()
        {
            var filesToRemove = Directory.GetFiles(TestFilesDirectory, "*.*", SearchOption.AllDirectories);

            foreach (var file in filesToRemove)
            {
                File.Delete(file);
            }

            var dirToRemove = Directory.GetDirectories(TestFilesDirectory);

            foreach (var dir in dirToRemove)
            {
                Directory.Delete(dir);
            }
        }

        [CollectionDefinition("TestFilesCollection")]
        public class TestCollection : ICollectionFixture<TestBase> { }
    }
}
