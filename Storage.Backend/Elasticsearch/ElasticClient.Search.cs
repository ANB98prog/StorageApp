using Elasticsearch.Common;
using Elasticsearch.Exceptions;
using Elasticsearch.Models;
using Nest;
using System.Text.RegularExpressions;

namespace Elasticsearch
{
    public partial class ElasticClient
    {
        /// <summary>
        /// Gets document by id
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Documents' index</param>
        /// <param name="id">Document id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Removed doc id</returns>
        public async Task<TDocument?> GetByIdAsync<TDocument>(string index, string id, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentNullException(nameof(id));

                var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                if (!exists.IsValid
                    || !exists.Exists)
                {
                    throw new IndexNotFoundException(index);
                }

                var result = await _client.GetAsync<TDocument>(id, s => s.Index(index), cancellationToken);

                if (!result.IsValid
                    || !result.Found)
                {
                    return null;
                }

                return result.Source;
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
                var docId = string.IsNullOrWhiteSpace(id)
                                        ? string.Empty : $"'{index}'";

                throw new UnexpectedElasticException(ErrorMessages.ERROR_GET_BY_ID_DOCUMENT(indexName, docId), ex);
            }
        }

        /// <summary>
        /// Searches documents
        /// </summary>
        /// <typeparam name="TDocument">Documents types</typeparam>
        /// <param name="index">Index to search in</param>
        /// <param name="request">Search request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Documents</returns>
        public async Task<Models.SearchResponse<TDocument>> SearchAsync<TDocument>(SearchRequest<TDocument> request, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                var result = await _client.SearchAsync<TDocument>(request);

                if (!result.IsValid)
                {
                    if((result.ServerError?.Error?.Reason ?? string.Empty)
                        .StartsWith(ElasticConstants.INDEX_NOT_EXISTS_SERVER_MESSAGE))
                    {
                        var index = Regex.Match(result.ServerError.Error.Reason, @"\[\w*\]");
                        throw new IndexNotFoundException(index.Value);
                    }

                    return null;
                }

                return result.MapHits();
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
                throw new UnexpectedElasticException(ErrorMessages.UNEXPECTED_ERROR_SEARCHING_DOCUMENTS(), ex);
            }
        }
    }
}
