using Nest;

namespace Elasticsearch
{
    public partial class ElasticClient
    {
        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="documentId">Document to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteDocumentAsync<TDocument>(string index, string documentId, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="ids">List of documents ids to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteBulkAsync<TDocument>(string index, IEnumerable<string> ids, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="selector">Documents selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task DeleteBulkAsync<TDocument>(string index, Func<DeleteDescriptor<TDocument>, IDeleteRequest> selector, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }
    }
}
