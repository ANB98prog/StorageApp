using AutoMapper;
using Moq;
using Ninject;
using Serilog;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class TestServicesFixture : IDisposable
    {
        protected readonly IKernel Kernel;

        public readonly IFileService FileService;

        protected readonly IMapper Mapper;

        public readonly IFileHandlerService FileHandlerService;

        public readonly Mock<IStorageDataService> StorageDataServiceMock;

        public readonly Mock<IFileService> FileServiceMock;

        public readonly string PathToTestFiles;

        public TestServicesFixture()
        {
            var ioCModule = new IoCModule();

            Kernel = new StandardKernel(ioCModule);

            StorageDataServiceMock = ioCModule.StorageDataServiceMock;

            if (!Directory.Exists(TestConstants.TestFilesDirectory))
                Directory.CreateDirectory(TestConstants.TestFilesDirectory);

            FileService = Kernel.Get<IFileService>();
           
            FileHandlerService = Kernel.Get<IFileHandlerService>();

            Mapper = Kernel.Get<IMapper>();

            PathToTestFiles = Path.Combine(Directory.GetCurrentDirectory(), "TestData");
        }

        public IVideoFilesService GetVideoService(Mock<IFileService> fileServiceMock)
        {
            return new VideoFilesService(TestConstants.StorageDirectory, new Mock<ILogger>().Object, fileServiceMock.Object, Kernel.Get<IStorageDataService>());
        }

        public void Dispose()
        {
            TestHelper.RemoveTestData(TestConstants.TestFilesDirectory);
            TestHelper.RemoveTestData(TestConstants.StorageDirectory);
        }

        [CollectionDefinition("TestServicesCollection")]
        public class TestServicesCollection : ICollectionFixture<TestServicesFixture> { }
    }
}
