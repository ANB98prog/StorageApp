using AutoMapper;
using Moq;
using Ninject;
using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class TestServicesFixture : IDisposable
    {
        protected readonly IKernel Kernel;

        protected readonly IFileService FileService;

        protected readonly IMapper Mapper;

        protected readonly IFileHandlerService FileHandlerService;

        protected readonly Mock<IStorageDataService> StorageDataServiceMock;

        protected readonly string PathToTestFiles;

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

            PathToTestFiles = Path.Combine(Directory.GetCurrentDirectory(), "FileHandlerServiceTests", "TestData");
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
