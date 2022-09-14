using AutoMapper;
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

        public TestServicesFixture()
        {
            var ioCModule = new IoCModule();

            Kernel = new StandardKernel(ioCModule);

            if (!Directory.Exists(TestConstants.TestFilesDirectory))
                Directory.CreateDirectory(TestConstants.TestFilesDirectory);

            FileService = Kernel.Get<IFileService>();
           
            FileHandlerService = Kernel.Get<IFileHandlerService>();

            Mapper = Kernel.Get<IMapper>();
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
