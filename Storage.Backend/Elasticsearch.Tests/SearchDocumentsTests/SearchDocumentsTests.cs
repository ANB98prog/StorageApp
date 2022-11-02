using Elasticsearch.Exceptions;
using Elasticsearch.Tests.Common;
using Moq;

namespace Elasticsearch.Tests.SearchDocumentsTests
{
    [Collection("IndexTestsCollection")]
    public class SearchDocumentsTests
    {
        private CreateIndexTestsFixture _fixture;

        public SearchDocumentsTests(CreateIndexTestsFixture fixture)
        {
            _fixture = fixture;
        }

        #region By id
        [Fact]
        public async Task GetDocById_Success()
        {
            var indexName = "test";
            var id = "id";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc/{id}")), Net.HttpMethod.GET))
                .Returns(ElasticTestHelper.GetByIdSuccessResponse(id, indexName));


            var result = await client.GetByIdAsync<TestModel>(indexName, id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("title", result.Title);
            Assert.Equal(10.0f, result.Rating);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc/{id}")), Net.HttpMethod.GET), Times.Once);
        }

        [Fact]
        public async Task GetDocById_Error_NotFound()
        {
            var indexName = "test";
            var id = "id";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc/{id}")), Net.HttpMethod.GET))
                .Returns(ElasticTestHelper.GetByIdNotFoundResponse(id, indexName));

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_refresh")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetRefreshResponse());

            var error = await Assert.ThrowsAsync<ItemNotFoundException>(async () => await client.GetByIdAsync<TestModel>(indexName, id));

            Assert.Equal(error.Message, "Item with 'id' is not found!");

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc/{id}")), Net.HttpMethod.GET), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetDocById_Error_EmptyId(string id)
        {
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            var result = await Assert.ThrowsAsync<ArgumentNullException>( async () => await client.GetByIdAsync<TestModel>(indexName, id));

            Assert.Equal("id", result.ParamName);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc/{id}")), Net.HttpMethod.GET), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task GetDocById_Error_EmptyIndex(string indexName)
        {
            var id = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.GetByIdAsync<TestModel>(indexName, id));

            Assert.Equal("index", result.ParamName);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_doc/{id}")), Net.HttpMethod.GET), Times.Never);
        }

        #endregion

        #region By Query
        [Fact]
        public async Task SearchDocuments_Success()
        {
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.SearchSuccessfulResponse());

            var request = new SearchRequest<TestModel>(indexName)
            {
                Query = new MatchAllQuery()
            };

            var result = await client.SearchAsync(request);

            Assert.Equal(2, result.Count);
            Assert.Equal("id1", result.Documents.First().Document.Id);
            Assert.Equal("id2", result.Documents.Last().Document.Id);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST), Times.Once);
        }

        [Fact]
        public async Task SearchDocuments_Success_NoHits()
        {
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.SearchEmptyResponse());

            var request = new SearchRequest<TestModel>(indexName)
            {
                Query = new MatchAllQuery()
            };

            var result = await client.SearchAsync(request);

            Assert.Empty(result.Documents);
            Assert.Equal(0, result.Count);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST), Times.Once);
        }

        [Fact]
        public async Task SearchDocuments_Error_Index_Not_Found()
        {
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.SearchIndexNotFoundResponse(indexName));

            var request = new SearchRequest<TestModel>(indexName)
            {
                Query = new MatchAllQuery()
            };

            var result = await Assert.ThrowsAsync<IndexNotFoundException>(async () => await client.SearchAsync(request));

            Assert.Equal(ErrorMessages.INDEX_NOT_EXISTS(indexName), result.Message);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST), Times.Once);
        }

        [Fact]
        public async Task SearchDocuments_Error_IfUnexpectedErrorOccured()
        {
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST))
                .Throws(new Exception());

            var request = new SearchRequest<TestModel>(indexName)
            {
                Query = new MatchAllQuery()
            };

            var result = await Assert.ThrowsAsync<UnexpectedElasticException>(async () => await client.SearchAsync(request));

            Assert.Equal(ErrorMessages.UNEXPECTED_ERROR_SEARCHING_DOCUMENTS, result.Message);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST), Times.Once);
        }
        #endregion
    }
}
