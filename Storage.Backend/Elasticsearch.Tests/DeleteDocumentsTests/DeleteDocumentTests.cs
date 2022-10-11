using Elasticsearch.Exceptions;
using Elasticsearch.Tests.Common;
using Moq;

namespace Elasticsearch.Tests.DeleteDocumentsTests
{
    [Collection("IndexTestsCollection")]
    public class DeleteDocumentTests
    {
        private CreateIndexTestsFixture _fixture;

        public DeleteDocumentTests(CreateIndexTestsFixture fixture)
        {
            _fixture = fixture;
        }

        #region By id

        [Fact]
        public async Task RemoveDocumentById_Success()
        {
            var docId = "931ca62a-2bcd-4376-ac0e-93eaec7f07bc";
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Index removing mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE))
                .Returns(ElasticTestHelper.GetDocumentDeletedSuccessResponse(indexName, docId));
            #endregion

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_refresh")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetRefreshResponse());

            await client.DeleteDocumentAsync(indexName, docId);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE), Times.Once);
        }

        [Fact]
        public async Task RemoveDocumentById_Success_DocNotFound()
        {
            var docId = "931ca62a-2bcd-4376-ac0e-93eaec7f07bc";
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Index removing mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE))
                .Returns(ElasticTestHelper.GetDocumentDeletedNotFoundResponse(indexName, docId));
            #endregion


            await client.DeleteDocumentAsync(indexName, docId);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE), Times.Once);
        }

        [Fact]
        public async Task RemoveDocumentById_Error_IndexNotFound()
        {
            var docId = "931ca62a-2bcd-4376-ac0e-93eaec7f07bc";
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexNotExistsResponse());
            #endregion

            var error = await Assert.ThrowsAsync<IndexNotFoundException>( async () =>  await client.DeleteDocumentAsync(indexName, docId));

            Assert.Equal(ErrorMessages.INDEX_NOT_EXISTS(indexName), error.Message);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task RemoveDocumentById_Error_IndexNull(string indexName)
        {
            var docId = "931ca62a-2bcd-4376-ac0e-93eaec7f07bc";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            var error = await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.DeleteDocumentAsync(indexName, docId));

            Assert.Equal("index", error.ParamName);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task RemoveDocumentById_Error_IdNull(string docId)
        {
            var indexName = "test";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            var error = await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.DeleteDocumentAsync(indexName, docId));

            Assert.Equal("documentId", error.ParamName);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_doc/{docId}")), Net.HttpMethod.DELETE), Times.Never);
        }

        #endregion

        [Fact]
        public async Task RemoveBulkDocument_Success()
        {
            var ids = new List<string>
            {
                "58a16e84-0296-49fa-b4a3-db4f280b7832",
                "83aba3ba-cd19-448f-95e9-e23162f49224",
                "931ca62a-2bcd-4376-ac0e-93eaec7f07br",
            };

            var indexName = "image";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Index removing mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_delete_by_query")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetBulkDocumentsDeletedSuccessResponse());
            #endregion

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_refresh")), Net.HttpMethod.POST))
                .Returns(ElasticTestHelper.GetRefreshResponse());

            await client.DeleteBulkByIdAsync(indexName, ids);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}/_delete_by_query")), Net.HttpMethod.POST), Times.Once);
        }
    }
}
