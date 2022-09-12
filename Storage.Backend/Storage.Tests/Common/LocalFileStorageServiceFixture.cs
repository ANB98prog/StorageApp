using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class LocalFileStorageServiceFixture : IDisposable
    {

        protected readonly IFileService FileService;

        protected readonly string TestFilesDirectory = Path.Combine(Environment.CurrentDirectory, "localStorageTest");

        public LocalFileStorageServiceFixture()
        {
            if (!Directory.Exists(TestFilesDirectory))
                Directory.CreateDirectory(TestFilesDirectory);

            FileService = Factory.CreateLocalFileStorageService();
        }

        public void Dispose()
        {
            TestHelper.RemoveTestData(TestFilesDirectory);
            TestHelper.RemoveTestData(Factory.StorageDirectory);
        }

        [CollectionDefinition("LocalFileStorageCollection")]
        public class LocalFileStorageServiceTestCollection : ICollectionFixture<LocalFileStorageServiceFixture> { }
    }
}
