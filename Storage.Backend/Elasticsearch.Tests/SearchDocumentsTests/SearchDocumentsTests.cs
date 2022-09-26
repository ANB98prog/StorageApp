using Elasticsearch.Exceptions;
using Elasticsearch.Tests.Common;
using Elasticsearch.Tests.ElasticSearchCommon;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

            Assert.Equal(ErrorMessages.UNEXPECTED_ERROR_SEARCHING_DOCUMENTS(), result.Message);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.StartsWith($"{_fixture.ElasticBasePath}/{indexName}/_search")), Net.HttpMethod.POST), Times.Once);
        }
    }
}
