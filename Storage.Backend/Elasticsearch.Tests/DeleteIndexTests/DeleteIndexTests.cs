using Elasticsearch.Exceptions;
using Elasticsearch.Tests.Common;
using Elasticsearch.Tests.ElasticSearchCommon;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.Tests.DeleteIndexTests
{
    [Collection("IndexTestsCollection")]
    public class DeleteIndexTests
    {
        private CreateIndexTestsFixture _fixture;

        public DeleteIndexTests(CreateIndexTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DeleteIndexTest_Success()
        {
            var indexName = "index";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion

            #region Index removing mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.DELETE))
                .Returns(ElasticTestHelper.GetIndexDeletedSuccessResponse());
            #endregion

            await client.DeleteIndexAsync(indexName);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.DELETE), Times.Once);
        }

        [Fact]
        public async Task DeleteIndexTest_Error_IfIndexNotExists()
        {
            var indexName = "index";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexNotExistsResponse());
            #endregion

            var error = await Assert.ThrowsAsync<IndexNotFoundException>( async () => await client.DeleteIndexAsync(indexName));

            Assert.Equal(ErrorMessages.INDEX_NOT_EXISTS(indexName), error.Message);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.DELETE), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task DeleteIndexTest_Error_IndexNameEmpty( string indexName)
        {
            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexNotExistsResponse());
            #endregion

            var error = await Assert.ThrowsAsync<ArgumentNullException>(async () => await client.DeleteIndexAsync(indexName));

            Assert.Equal(nameof(indexName), error.ParamName);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Never);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.DELETE), Times.Never);
        }

        [Fact]
        public async Task DeleteIndexTest_Error_IfErrorWhileRemove()
        {
            var indexName = "index";

            var responseMock = new Mock<IElasticFakeResponse>();
            var client = _fixture.GetElasticsearchClient(responseMock);

            #region Index exists mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD))
                .Returns(ElasticTestHelper.GetIndexExistsResponse());
            #endregion
            #region Index removing mocking
            responseMock
                .Setup(s => s.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.DELETE))
                .Returns(ElasticTestHelper.GetInvalidResponse());
            #endregion

            var error = await Assert.ThrowsAsync<UnexpectedElasticException>(async () => await client.DeleteIndexAsync(indexName));

            Assert.Equal(ErrorMessages.ERROR_REMOVING_INDEX(indexName), error.UserfriendlyMessage);

            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.HEAD), Times.Once);
            responseMock.Verify(v => v.GetResponseData(It.Is<string>(m => m.Equals($"{_fixture.ElasticBasePath}/{indexName}")), Net.HttpMethod.DELETE), Times.Once);
        }
    }
}
