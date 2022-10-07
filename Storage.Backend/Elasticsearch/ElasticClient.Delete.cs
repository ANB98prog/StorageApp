using Elasticsearch.Exceptions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexNotFoundException"></exception>
        /// <exception cref="DeleteDocumentException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
        /// <returns>Acknowledged</returns>
        public async Task<bool> DeleteDocumentAsync(string index, string documentId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));
                if (string.IsNullOrWhiteSpace(documentId))
                    throw new ArgumentNullException(nameof(documentId));

                var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                if (!exists.IsValid
                    || !exists.Exists)
                {
                    throw new IndexNotFoundException(index);
                }

                var result = await _client.DeleteAsync(new DeleteRequest(index, documentId), cancellationToken);

               if (!result.IsValid
                    && result.ServerError != null)
                {
                    throw new DeleteDocumentException(ErrorMessages.ERROR_DELETE_BY_ID_DOCUMENT(index, documentId), result.OriginalException);
                }
                else if (!result.IsValid)
                {
                    return false;
                }

                return true;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (IndexNotFoundException ex)
            {
                throw ex;
            }
            catch (DeleteDocumentException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                var indexName = string.IsNullOrWhiteSpace(index)
                                        ? string.Empty : $"'{index}'";
                var docId = string.IsNullOrWhiteSpace(documentId)
                                        ? string.Empty : $"'{index}'";

                throw new UnexpectedElasticException(ErrorMessages.ERROR_DELETE_BY_ID_DOCUMENT(indexName, docId), ex);
            }
        }

        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="ids">List of documents ids to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task DeleteBulkAsync<TDocument>(string index, IEnumerable<string> ids, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));

                if (ids != null
                    && ids.Any())
                {
                    var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                    if (!exists.IsValid
                        || !exists.Exists)
                    {
                        throw new IndexNotFoundException(index);
                    }

                    var result = await _client.DeleteByQueryAsync<TDocument>(s =>
                                        s.Query(q =>
                                            q.Ids(i => i.Values(ids)))
                                        .Index(index));

                    if (!result.IsValid)
                    {
                        throw new UnexpectedElasticException();
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (IndexNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                var indexName = string.IsNullOrWhiteSpace(index)
                                        ? string.Empty : $"'{index}'";

                throw new UnexpectedElasticException(ErrorMessages.UNEXPECTED_ERROR_REMOVING_BULK_DOCUMENTS(indexName), ex);
            }
        }

        /// <summary>
        /// Deletes document
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index to delete in</param>
        /// <param name="selector">Documents selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task DeleteBulkAsync<TDocument>(Func<DeleteByQueryDescriptor<TDocument>, IDeleteByQueryRequest> selector, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                if(selector == null)
                    throw new ArgumentNullException(nameof(selector));

                var result = await _client.DeleteByQueryAsync(selector , cancellationToken);

                if (!result.IsValid)
                {
                    throw new UnexpectedElasticException();
                }

            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (IndexNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new UnexpectedElasticException(ErrorMessages.UNEXPECTED_ERROR_REMOVING_BULK_DOCUMENTS_BY_QUERY(), ex);
            }
        }
    }
}
