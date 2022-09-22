using Elasticsearch.Net;
using Elasticsearch.Tests.ElasticSearchCommon;
using Moq;
using Ninject;
using Ninject.Modules;

namespace Elasticsearch.Tests.Common
{
    public class IoCModule : NinjectModule
    {
        public Mock<IElasticFakeResponse> ElasticFakeResponseMock;

        public IoCModule()
        {
            ElasticFakeResponseMock = new Mock<IElasticFakeResponse>();
        }

        public override void Load()
        {
            var connection = new TestConnection(ElasticFakeResponseMock.Object);
            var connectionPool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(connectionPool, connection).DefaultIndex("project");

            Bind<IElasticFakeResponse>().ToMethod(s => ElasticFakeResponseMock.Object);
            Bind<IElasticClient>().ToMethod(s => new Nest.ElasticClient(settings));
            Bind<IElasticsearchClient>().ToMethod(s => new ElasticClient(s.Kernel.Get<IElasticClient>()));
        }
    }
}
