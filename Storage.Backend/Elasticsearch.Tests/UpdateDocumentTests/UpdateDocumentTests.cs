using Elasticsearch.Tests.Common;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Tests.UpdateDocumentTests
{
    [Collection("IndexTestsCollection")]
    public class UpdateDocumentTests
    {
        private CreateIndexTestsFixture _fixture;

        public UpdateDocumentTests(CreateIndexTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task UpdateDocumentTest_Success()
        {
            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            var indexName = "simple";

            var doc = new SimpleClass()
            {
                Id = "1",
                Name = "name"
            };

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Updated by id mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_update/{doc.Id}")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetSuccessUpdateResponse(doc.Id, indexName));
            #endregion

            #region Refresh mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_refresh")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetRefreshResponse());
            #endregion

            doc.Name = "updated";

            var isUpdated = await client.UpdateAsync<SimpleClass>("simple", doc.Id, doc);

            Assert.True(isUpdated);
            responseMock
               .Verify(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock
                .Verify(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_update/{doc.Id}")), Net.HttpMethod.POST), Times.Once);
            responseMock
                .Verify(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_refresh")), Net.HttpMethod.POST), Times.Once);
        }

        [Fact]
        public async Task UpdateBulkDocumentTest_Success()
        {
            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            var indexName = "simple";

            var docs = new List<SimpleClass>{
                new SimpleClass()
                {
                    Id = "11",
                    Name = "name"
                },
                new SimpleClass()
                {
                    Id = "22",
                    Name = "name"
                }
            };

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Updated bulk id mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_bulk?refresh=true")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetBulkUpdateResponse(indexName));
            #endregion

            var isUpdated = await client.BulkUpdateAsync<SimpleClass>("simple", docs);

            Assert.True(isUpdated.Acknowledged);

            responseMock
                .Verify(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);

            responseMock
                .Verify(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_bulk?refresh=true")), Net.HttpMethod.POST), Times.Once);
        }
    }

    public class SimpleClass
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
