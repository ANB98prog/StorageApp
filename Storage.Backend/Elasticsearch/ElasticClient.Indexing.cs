using Elasticsearch.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch
{
    public partial class ElasticClient
    {
        /// <summary>
        /// Indexes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="document">Document to index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task<Guid> IndexDocumentAsync<TDocument>(TDocument document, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }

        //// <summary>
        /// Indexes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="document">Document to index</param>
        /// <param name="selector">Index selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public Task<Guid> IndexDocumentAsync<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> selector, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indexes many documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public Task<IEnumerable<Guid>> IndexManyAsync<TDocument>(IEnumerable<TDocument> documents, string index = null, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indexes bulk of documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public Task<IEnumerable<Guid>> BulkDocuments<TDocument>(IEnumerable<TDocument> documents, string index, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }
    }
}
