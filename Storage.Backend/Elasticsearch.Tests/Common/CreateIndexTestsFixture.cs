using Elasticsearch.Net;
using Elasticsearch.Tests.ElasticSearchCommon;
using Moq;
using Nest;
using Ninject;

namespace Elasticsearch.Tests.Common
{
    public class CreateIndexTestsFixture
    {
        internal Mock<IElasticFakeResponse> ElasticFakeResponseMock;

        internal IElasticsearchClient ElasticClient;

        internal IKernel Kernel;

        public CreateIndexTestsFixture()
        {
            var iocModule = new IoCModule();
            Kernel = new StandardKernel(iocModule);

            ElasticFakeResponseMock = iocModule.ElasticFakeResponseMock;
            ElasticClient = Kernel.Get<IElasticsearchClient>();
        }
    }

    [CollectionDefinition("IndexTestsCollection")]
    public class CreateIndexTestsCollection : ICollectionFixture<CreateIndexTestsFixture>
    {
    }
}
