﻿using Elasticsearch.Interfaces;
using Moq;
using Serilog;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;

namespace Storage.Tests.ElasticStorageTests
{
    public class ElasticStorageFixture : IDisposable
    {
        public IStorageDataService GetElasticStorageService(Mock<IElasticsearchClient> elasticClientMock)
        {
            var loggerMock = new Mock<ILogger>();

            return new ElasticStorageService(elasticClientMock.Object, loggerMock.Object);
        }

        public void Dispose()
        {
            
        }
    }

    [CollectionDefinition("ElasticStorageCollection")]
    public class ElasticStorageCollection : ICollectionFixture<ElasticStorageFixture> { }
}
