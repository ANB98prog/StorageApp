using AutoMapper;
using Mapper;
using Moq;
using Ninject;
using Ninject.Modules;
using Serilog;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;

namespace Storage.Tests.Common
{
    public class IoCModule : NinjectModule
    {
        public override void Load()
        {
            if(!Directory.Exists(TestConstants.StorageDirectory))
                Directory.CreateDirectory(TestConstants.StorageDirectory);

            if (!Directory.Exists(TestConstants.CommandsFilesDirectory))
                Directory.CreateDirectory(TestConstants.CommandsFilesDirectory);

            if (!Directory.Exists(TestConstants.TestFilesDirectory))
                Directory.CreateDirectory(TestConstants.TestFilesDirectory);

            Bind<IFileService>()
                .ToMethod(ctx => new LocalFileStorageService(TestConstants.StorageDirectory));

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(
                    typeof(IFileHandlerService).Assembly));
            });
            Bind<IMapper>()
                .ToMethod(ctx => configurationProvider.CreateMapper());

            Bind<IFileHandlerService>()
                .ToMethod(ctx => new FileHandlerService(ctx.Kernel.Get<IMapper>(), new Mock<ILogger>().Object, ctx.Kernel.Get<IFileService>()));
        }
    }
}
