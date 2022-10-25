using AutoMapper;
using Elasticsearch.Interfaces;
using Moq;
using Ninject;
using Serilog;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;
using Storage.Tests.Common;

namespace Storage.Tests.ElasticStorageTests
{
    public class ElasticStorageFixture : IDisposable
    {
        public IStorageDataService GetElasticStorageService(Mock<IElasticsearchClient> elasticClientMock)
        {
            var ioc = new IoCModule();
            var kernel = new StandardKernel(ioc);

            var loggerMock = new Mock<ILogger>();

            return new ElasticStorageService("test_files", loggerMock.Object, kernel.Get<IMapper>(), elasticClientMock.Object);
        }

        public void Dispose()
        {
            
        }
    }

    [CollectionDefinition("ElasticStorageCollection")]
    public class ElasticStorageCollection : ICollectionFixture<ElasticStorageFixture> { }
}
