using Nest;

namespace Elasticsearch
{
    public interface IElasticsearchClient
    {
        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="descriptor">Description of index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task CreateIndexAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> descriptor = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task CreateIndexAsync(string indexName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes index
        /// </summary>
        /// <param name="indexName">Index to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteIndexAsync(string indexName, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Indexes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="document">Document to index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task<Guid> IndexDocumentAsync<TDocument>(TDocument document, CancellationToken cancellationToken = default(CancellationToken)) where TDocument : class;

        //// <summary>
        /// Indexes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="document">Document to index</param>
        /// <param name="selector">Index selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task<Guid> IndexDocumentAsync<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> selector, CancellationToken cancellationToken = default(CancellationToken)) where TDocument : class;

        /// <summary>
        /// Indexes many documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public Task<IEnumerable<Guid>> IndexManyAsync<TDocument>(IEnumerable<TDocument> documents, IndexName index = null, CancellationToken cancellationToken = default(CancellationToken)) where TDocument : class;

        /// <summary>
        /// Indexes bulk of documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public Task<IEnumerable<Guid>> BulkDocuments<TDocument>(IEnumerable<TDocument> documents, string index, CancellationToken cancellationToken = default(CancellationToken)) where TDocument : class;
    }
}
