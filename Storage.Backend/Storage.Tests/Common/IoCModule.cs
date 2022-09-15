using AutoMapper;
using Ninject;
using Ninject.Modules;
using Storage.Application.Common.Mappings;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.Common
{
    public class IoCModule : NinjectModule
    {
        public override void Load()
        {
            if(!Directory.Exists(TestConstants.StorageDirectory))
                Directory.CreateDirectory(TestConstants.StorageDirectory);

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
                .ToMethod(ctx => new ImagesFileHandlerService(ctx.Kernel.Get<IMapper>(), ctx.Kernel.Get<IFileService>()));
        }
    }
}
