using AutoMapper;
using Moq;
using Ninject;
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

        public readonly IVideoFilesService VideoFilesService;

        public TestServicesFixture()
        {
            var ioCModule = new IoCModule();

            Kernel = new StandardKernel(ioCModule);

            StorageDataServiceMock = ioCModule.StorageDataServiceMock;
            FileServiceMock = ioCModule.FileServiceMock;

            if (!Directory.Exists(TestConstants.TestFilesDirectory))
                Directory.CreateDirectory(TestConstants.TestFilesDirectory);

            FileService = Kernel.Get<IFileService>();
           
            FileHandlerService = Kernel.Get<IFileHandlerService>();

            Mapper = Kernel.Get<IMapper>();

            PathToTestFiles = Path.Combine(Directory.GetCurrentDirectory(), "TestData");

            VideoFilesService = Kernel.Get<IVideoFilesService>();
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
