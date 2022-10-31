using Elasticsearch.Common;
using Elasticsearch.Exceptions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ItemNotFoundException"></exception>
        /// <exception cref="IndexNotFoundException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
        /// <returns>Document info</returns>
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
                    throw new ItemNotFoundException(id);
                }

                return result.Source;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ItemNotFoundException ex)
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
        /// Gets many documents by ids
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Documents' index</param>
        /// <param name="ids">Documents ids</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Removed doc id</returns>
        public async Task<List<TDocument>> GetManyByIdsAsync<TDocument>(string index, List<string> ids, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));

                if (ids.Any())
                {
                    var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                    if (!exists.IsValid
                        || !exists.Exists)
                    {
                        throw new IndexNotFoundException(index);
                    }

                    var result = await _client.GetManyAsync<TDocument>(ids, index, cancellationToken);

                    if (!result.Any())
                    {
                        return new List<TDocument>();
                    }

                    return result.Where(item => item.Found)
                                    .Select(hit => hit.Source)
                                        .ToList(); 
                }

                return new List<TDocument>();
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

                throw new UnexpectedElasticException(ErrorMessages.ERROR_GET_MANY_BY_IDS_DOCUMENTS(indexName), ex);
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
                        var indexNameReg = Regex.Match(result.ServerError.Error.Reason, @"\[\w*\]");

                        var index = "";
                        if (indexNameReg != null
                            && indexNameReg.Success)
                        {
                            index = Regex.Replace(indexNameReg.Value, @"[\[\]]", string.Empty);
                        }

                        throw new IndexNotFoundException(index);
                    }

                    return null;
                }

                return result.MapHits(request.Aggregations.Select(a => a.Key).ToList());
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

        /// <summary>
        /// Searches documents
        /// </summary>
        /// <typeparam name="TDocument">Documents types</typeparam>
        /// <param name="index">Index to search in</param>
        /// <param name="request">Search request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Documents</returns>
        public async Task<Models.SearchResponse<TDocument>> SearchAsync<TDocument>(Func<SearchDescriptor<TDocument>, ISearchRequest> request, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                var result = await _client.SearchAsync<TDocument>(request);

                if (!result.IsValid)
                {
                    if ((result.ServerError?.Error?.Reason ?? string.Empty)
                        .StartsWith(ElasticConstants.INDEX_NOT_EXISTS_SERVER_MESSAGE))
                    {
                        var indexNameReg = Regex.Match(result.ServerError.Error.Reason, @"\[\w*\]");

                        var index = "";
                        if (indexNameReg != null
                            && indexNameReg.Success)
                        {
                            index = Regex.Replace(indexNameReg.Value, @"[\[\]]", string.Empty);
                        }

                        throw new IndexNotFoundException(index);
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

        /// <summary>
        /// Counts documents
        /// </summary>
        /// <typeparam name="TDocument">Documents types</typeparam>
        /// <param name="index">Index to count in</param>
        /// <returns>Count of documents</returns>
        public async Task<long> CountAsync<TDocument>(string index) where TDocument : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(index))
                {
                    throw new ArgumentNullException(nameof(index));
                }

                var result = await _client.CountAsync<TDocument>(s => s.Index(index));

                if (!result.IsValid)
                {
                    if ((result.ServerError?.Error?.Reason ?? string.Empty)
                        .StartsWith(ElasticConstants.INDEX_NOT_EXISTS_SERVER_MESSAGE))
                    {
                        var indexNameReg = Regex.Match(result.ServerError.Error.Reason, @"\[\w*\]");

                        throw new IndexNotFoundException(index);
                    }                    
                }

                return result.Count;
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
                throw new UnexpectedElasticException(ErrorMessages.UNEXPECTED_ERROR_COUNTING_DOCUMENTS(), ex);
            }
        }

        /// <summary>
        /// Counts documents
        /// </summary>
        /// <typeparam name="TDocument">Documents types</typeparam>
        /// <param name="request">Count request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Count of documents</returns>
        public async Task<long> CountAsync<TDocument>(Func<CountDescriptor<TDocument>, ICountRequest> request, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                var result = await _client.CountAsync<TDocument>(request);

                if (!result.IsValid)
                {
                    if ((result.ServerError?.Error?.Reason ?? string.Empty)
                        .StartsWith(ElasticConstants.INDEX_NOT_EXISTS_SERVER_MESSAGE))
                    {
                        var indexNameReg = Regex.Match(result.ServerError.Error.Reason, @"\[\w*\]");

                        var index = "";
                        if (indexNameReg != null
                            && indexNameReg.Success)
                        {
                            index = Regex.Replace(indexNameReg.Value, @"[\[\]]", string.Empty);
                        }

                        throw new IndexNotFoundException(index);
                    }
                }

                return result.Count;
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
                throw new UnexpectedElasticException(ErrorMessages.UNEXPECTED_ERROR_COUNTING_DOCUMENTS(), ex);
            }
        }
    }
}
