using AutoMapper;
using Elasticsearch.Interfaces;
using Elasticsearch.Net;
using Mapper;
using Moq;

namespace Elasticsearch.Tests.Common
{
    public class CreateIndexTestsFixture
    {
        public readonly string ElasticBasePath = "http://localhost:9200";

        public IElasticsearchClient GetElasticsearchClient(Mock<IElasticFakeResponse> elasticResponseMock)
        {
            var connection = new ElasticTestConnection(elasticResponseMock.Object);
            var connectionPool = new SingleNodeConnectionPool(new Uri(ElasticBasePath));
            var settings = new ConnectionSettings(connectionPool, connection).DefaultIndex("project");
            var nestClient = new Nest.ElasticClient(settings);

            return new ElasticClient(nestClient);
        }

        public IElasticsearchClient GetRealElasticsearchClient()
        {
            var elasticUrl = "http://localhost:9200";
            var elasticUser = "elastic";
            var elasticPassword = "";

            var settings = new ConnectionSettings(new Uri(elasticUrl))
                                .BasicAuthentication(elasticUser, elasticPassword);

            var nestClient = new Nest.ElasticClient(settings);

            return new ElasticClient(nestClient);
        }
    }

    [CollectionDefinition("IndexTestsCollection")]
    public class CreateIndexTestsCollection : ICollectionFixture<CreateIndexTestsFixture>
    {
    }
}
