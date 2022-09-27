using Elasticsearch.Tests.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Tests.AddDocumentTests
{
    [Collection("IndexTestsCollection")]
    public class AddDocumentTests
    {
        private CreateIndexTestsFixture _fixture;

        public AddDocumentTests(CreateIndexTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddDocumentTest_Success()
        {
            var indexName = "index";
            var id = "docId";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Create index mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT))
                .Returns(ElasticTestHelper.GetIndexCreateSuccessResponse(indexName));
            #endregion

            #region Add document mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.AddDocumentSuccessResponse(indexName, id));
            #endregion

            var result = await client.AddDocumentAsync<TestModel>(indexName, new TestModel(), CancellationToken.None);

            Assert.NotEmpty(result);
            Assert.Equal(id, result);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc")), Net.HttpMethod.POST), Times.Once);
        }

        [Fact]
        public async Task AddDocumentTest_Success_IfIndexNotExists()
        {
            var indexName = "index";
            var id = "docId";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexNotExistsResponse());
            #endregion

            #region Create index mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT))
                .Returns(ElasticTestHelper.GetIndexCreateSuccessResponse(indexName));
            #endregion

            #region Add document mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.AddDocumentSuccessResponse(indexName, id));
            #endregion

            var result = await client.AddDocumentAsync<TestModel>(indexName, new TestModel(), CancellationToken.None);

            Assert.NotEmpty(result);
            Assert.Equal(id, result);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Exactly(2));
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc")), Net.HttpMethod.POST), Times.Once);
        }
    }
}
