using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class FileHelperFixture : IDisposable
    {

        protected readonly string TestFilesDirectory = Path.Combine(Environment.CurrentDirectory, "test");


        public FileHelperFixture()
        {
            if (!Directory.Exists(TestFilesDirectory))
                Directory.CreateDirectory(TestFilesDirectory);
        }

        public void Dispose()
        {
            TestHelper.RemoveTestData(TestFilesDirectory);
        }

        [CollectionDefinition("TestFilesCollection")]
        public class TestCollection : ICollectionFixture<FileHelperFixture> { }
    }
}
