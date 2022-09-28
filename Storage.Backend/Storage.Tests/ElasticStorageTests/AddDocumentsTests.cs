using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using Moq;
using Nest;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Tests.ElasticStorageTests
{
    [Collection("ElasticStorageCollection")]
    public class AddDocumentsTests
    {
        private readonly ElasticStorageFixture _fixture;

        public AddDocumentsTests(ElasticStorageFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddDocument_Success()
        {
            var elasticMock = new Mock<IElasticsearchClient>();

            var storage = _fixture.GetElasticStorageService(elasticMock);

            var document = new StorageTestData
            {
                Id = Guid.NewGuid().ToString(),
                Title = "title"
            };

            var indexName = ElasticHelper.GetFormattedIndexName(nameof(StorageTestData));

            elasticMock.Setup(
                s => s.AddDocumentAsync<StorageTestData>(It.Is<string>(s => s.Equals(indexName)), document, CancellationToken.None))
                .ReturnsAsync(document.Id);

            var result = await storage.AddDataToStorageAsync(document);

            Assert.Equal(document.Id, result.ToString());

            elasticMock.Verify(
                s => s.AddDocumentAsync<StorageTestData>(It.Is<string>(s => s.Equals(indexName)), document, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task AddDocument_Error_IfDataIsNull()
        {
            var elasticMock = new Mock<IElasticsearchClient>();

            var storage = _fixture.GetElasticStorageService(elasticMock);


            var error = await Assert.ThrowsAsync<ElasticStorageServiceException>( async () => await storage.AddDataToStorageAsync<StorageTestData>(null));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("data"), error.UserFriendlyMessage);

            elasticMock.Verify(
                s => s.AddDocumentAsync<StorageTestData>(It.IsAny<string>(), It.IsAny<StorageTestData>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task AddDocument_Error_IfElasticReturnsError()
        {
            var elasticMock = new Mock<IElasticsearchClient>();

            var storage = _fixture.GetElasticStorageService(elasticMock);

            var document = new StorageTestData
            {
                Id = Guid.NewGuid().ToString(),
                Title = "title"
            };

            var indexName = ElasticHelper.GetFormattedIndexName(nameof(StorageTestData));

            elasticMock.Setup(
                s => s.AddDocumentAsync<StorageTestData>(It.Is<string>(s => s.Equals(indexName)), document, CancellationToken.None))
                .ThrowsAsync(new UnexpectedElasticException(Elasticsearch.Exceptions.ErrorMessages.ERROR_ADDITION_DOCUMENT("index"), new Exception()));

            var error = await Assert.ThrowsAsync<ElasticStorageServiceException>( async () => await storage.AddDataToStorageAsync(document));

            Assert.Equal(ErrorMessages.UNEXPECTED_ERROR_WHILE_ADD_ITEM_TO_STORAGE_MESSAGE, error.UserFriendlyMessage);

            elasticMock.Verify(
                s => s.AddDocumentAsync<StorageTestData>(It.Is<string>(s => s.Equals(indexName)), document, CancellationToken.None), Times.Once);
        }
    }
}
