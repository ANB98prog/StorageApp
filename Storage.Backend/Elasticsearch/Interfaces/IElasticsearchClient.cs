using Elasticsearch.Models;
using Nest;

namespace Elasticsearch.Interfaces
{
    public interface IElasticsearchClient
    {
        #region Indexing
        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="descriptor">Description of index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task CreateIndexAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> descriptor = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task CreateIndexAsync(string indexName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes index
        /// </summary>
        /// <param name="indexName">Index to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteIndexAsync(string indexName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Indexes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index</param>
        /// <param name="document">Document to index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task<string> AddDocumentAsync<TDocument>(string index, TDocument document, CancellationToken cancellationToken = default) where TDocument : class;

        //// <summary>
        /// Indexes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index</param>
        /// <param name="document">Document to index</param>
        /// <param name="selector">Index selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task<string> AddDocumentAsync<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> selector, CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Indexes many documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public Task<AddManyResponseModel> AddManyAsync<TDocument>(string index, IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Indexes bulk of documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public Task<AddManyResponseModel> AddBulkDocuments<TDocument>(string index, IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : class;
        #endregion

        #region Search

        /// <summary>
        /// Searches documents
        /// </summary>
        /// <typeparam name="TDocument">Documents types</typeparam>
        /// <param name="index">Index to search in</param>
        /// <param name="request">Search request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Documents</returns>
        public Task<Models.SearchResponse<TDocument>> SearchAsync<TDocument>(string index, SearchRequest<TDocument> request, CancellationToken cancellationToken = default) where TDocument : class;

        #endregion

        #region Delete
        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="documentId">Document to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteDocumentAsync<TDocument>(string index, string documentId, CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="ids">List of documents ids to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteBulkAsync<TDocument>(string index, IEnumerable<string> ids, CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="selector">Documents selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteBulkAsync<TDocument>(string index, Func<DeleteDescriptor<TDocument>, IDeleteRequest> selector, CancellationToken cancellationToken = default) where TDocument : class;

        #endregion

    }
}
