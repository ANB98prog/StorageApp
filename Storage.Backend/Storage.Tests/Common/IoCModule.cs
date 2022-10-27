using AutoMapper;
using Mapper;
using Moq;
using Ninject;
using Ninject.Modules;
using Serilog;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;
using Storage.Domain;

namespace Storage.Tests.Common
{
    public class IoCModule : NinjectModule
    {
        public Mock<IStorageDataService> StorageDataServiceMock;

        public IoCModule()
        {
            StorageDataServiceMock = new Mock<IStorageDataService>();
        }

        public override void Load()
        {
            if(!Directory.Exists(TestConstants.StorageDirectory))
                Directory.CreateDirectory(TestConstants.StorageDirectory);

            if (!Directory.Exists(TestConstants.CommandsFilesDirectory))
                Directory.CreateDirectory(TestConstants.CommandsFilesDirectory);

            if (!Directory.Exists(TestConstants.TestFilesDirectory))
                Directory.CreateDirectory(TestConstants.TestFilesDirectory);

            Bind<IFileService>()
                .ToMethod(ctx => new LocalFileStorageService(TestConstants.StorageDirectory, new Mock<ILogger>().Object));

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(typeof(IFileHandlerService).Assembly));
                cfg.AddProfile(new AssemblyMappingProfile(typeof(BaseFile).Assembly));
            });
            Bind<IMapper>()
                .ToMethod(ctx => configurationProvider.CreateMapper());

            Bind<IStorageDataService>()
                .ToMethod(ctx => StorageDataServiceMock.Object);

            Bind<IFileHandlerService>()
                .ToMethod(ctx => new FileHandlerService(TestConstants.StorageDirectory, new Mock<ILogger>().Object, ctx.Kernel.Get<IMapper>(), ctx.Kernel.Get<IFileService>(), ctx.Kernel.Get<IStorageDataService>()));
        }
    }
}
