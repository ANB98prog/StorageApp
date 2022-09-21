using Moq;

namespace Elasticsearch.Tests.Common
{
    public class CreateIndexTestsFixture
    {
        public Mock<IElasticClient> ElasticClientMock;

        public CreateIndexTestsFixture()
        {
            ElasticClientMock = new Mock<IElasticClient>();
        }
    }

    [CollectionDefinition("IndexTestsCollection")]
    public class CreateIndexTestsCollection : ICollectionFixture<CreateIndexTestsFixture>
    {
    }
}
