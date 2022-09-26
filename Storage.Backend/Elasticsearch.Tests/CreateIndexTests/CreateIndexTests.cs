using Elasticsearch.Exceptions;
using Elasticsearch.Tests.Common;
using Moq;

namespace Elasticsearch.Tests.CreateIndexTests.CreateIndexTests
{
    [Collection("IndexTestsCollection")]
    public class CreateIndexTests
    {
        private CreateIndexTestsFixture _fixture;

        public CreateIndexTests(CreateIndexTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void CreateIndexTest_Success()
        {
            var indexName = "index";

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

            await client.CreateIndexAsync(indexName, CancellationToken.None);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT), Times.Once);
        }

        [Fact]
        public async void CreateIndexTest_Error_IndexAlreadyExists()
        {
            var indexName = "index";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());

            var error = await Assert.ThrowsAsync<IndexCreationException>(async () => await client.CreateIndexAsync(indexName, CancellationToken.None));

            Assert.Equal(ErrorMessages.INDEX_ALREADY_EXISTS(indexName), error.UserfriendlyMessage);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT), Times.Never);
        }

        [Fact]
        public async void CreateIndexTest_Error_IndexNameIsEmpty()
        {
            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);
            
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.CreateIndexAsync(null, CancellationToken.None));

            responseMock.Verify(v => v.GetResponseData(It.IsAny<string>(), Net.HttpMethod.HEAD), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.IsAny<string>(), Net.HttpMethod.PUT), Times.Never);
        }

        [Fact]
        public async void CreateIndexTest_Error_IfUnexpectedErrorOccured()
        {
            var indexName = "index";

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
                .Throws(new Exception());
            #endregion

            var error = await Assert.ThrowsAsync<UnexpectedElasticException>(async () => await client.CreateIndexAsync(indexName, CancellationToken.None));

            Assert.Equal(ErrorMessages.ERROR_CREATING_INDEX(indexName), error.UserfriendlyMessage);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.PUT), Times.Once);
        }
    }
}