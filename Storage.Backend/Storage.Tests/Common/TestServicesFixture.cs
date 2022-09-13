using AutoMapper;
using Storage.Application.Common.Mappings;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class TestServicesFixture : IDisposable
    {

        protected readonly IFileService FileService;

        protected readonly string TestFilesDirectory = Path.Combine(Environment.CurrentDirectory, "localStorageTest");

        protected readonly string StorageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp");

        protected readonly IMapper Mapper;

        protected readonly IFileHandlerService FileHandlerService;

        public TestServicesFixture()
        {
            if (!Directory.Exists(TestFilesDirectory))
                Directory.CreateDirectory(TestFilesDirectory);

            FileService = new LocalFileStorageService(StorageDirectory);

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(IFileHandlerService).Assembly));
            });

            Mapper = configurationProvider.CreateMapper();

            FileHandlerService = new ImagesFileHandlerService(Mapper, FileService);
        }

        public void Dispose()
        {
            TestHelper.RemoveTestData(TestFilesDirectory);
            TestHelper.RemoveTestData(StorageDirectory);
        }

        [CollectionDefinition("TestServicesCollection")]
        public class TestServicesCollection : ICollectionFixture<TestServicesFixture> { }


    }
}
