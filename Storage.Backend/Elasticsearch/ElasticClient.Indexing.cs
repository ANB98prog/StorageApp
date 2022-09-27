using Elasticsearch.Exceptions;
using Elasticsearch.Models;
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
        /// Adds document to index
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index</param>
        /// <param name="document">Document to index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexNotFoundException"></exception>
        /// <exception cref="IndexCreationException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
        /// <returns>Document id</returns>
        public async Task<string> AddDocumentAsync<TDocument>(string index, TDocument document, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));

                var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                if (!exists.IsValid
                    || !exists.Exists)
                {
                    await CreateIndexAsync(index, cancellationToken);
                }

                var result = await _client.IndexAsync(document, 
                                        i => i.Index(index), cancellationToken);

                if (!result.IsValid)
                {
                    throw new UnexpectedElasticException();
                }

                return result.Id;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (IndexNotFoundException ex)
            {
                throw ex;
            }
            catch (IndexCreationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                var indexName = string.IsNullOrWhiteSpace(index)
                                        ? string.Empty : $"'{index}'";

                throw new UnexpectedElasticException(ErrorMessages.ERROR_ADDITION_DOCUMENT(index), ex);
            }
        }

        //// <summary>
        /// Adds document to index
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="document">Document to index</param>
        /// <param name="selector">Index selector</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<string> AddDocumentAsync<TDocument>(TDocument document, Func<IndexDescriptor<TDocument>, IIndexRequest<TDocument>> selector, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                if(selector == null)
                    throw new ArgumentNullException(nameof(selector));

                var result = await _client.IndexAsync(document, selector, cancellationToken);

                if (!result.IsValid)
                {
                    throw new UnexpectedElasticException();
                }

                return result.Id;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new UnexpectedElasticException(ErrorMessages.ERROR_ADDITION_DOCUMENT(string.Empty), ex);
            }
        }

        /// <summary>
        /// Adds many documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public async Task<AddManyResponseModel> AddManyAsync<TDocument>(string index, IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                var ids = new AddManyResponseModel();

                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));

                if (documents.Any())
                {
                    var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                    if (!exists.IsValid
                        || !exists.Exists)
                    {
                        throw new IndexNotFoundException(index);
                    }

                    var result = await _client.IndexManyAsync(documents, index, cancellationToken);

                    if (!result.IsValid)
                    {
                        throw new UnexpectedElasticException();
                    }

                    ids.Ids = new List<string>(result.Items.Select(s => s.Id).ToList());

                    if (result.Errors)
                    {
                        ids.ItemsWithErrors = new List<AddDocumentError>(
                                                result.ItemsWithErrors.Select(s => 
                                                    new AddDocumentError 
                                                    {
                                                        Id = s.Id,
                                                        Error = s.Error.Reason 
                                                    }));
                    }
                }

                return ids;
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

                throw new UnexpectedElasticException(ErrorMessages.ERROR_ADDITION_DOCUMENT(index), ex);
            }
        }

        /// <summary>
        /// Indexes bulk of documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="documents">Documents to index</param>
        /// <param name="index">Index to add to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of ids</returns>
        public async Task<AddManyResponseModel> AddBulkDocuments<TDocument>(string index, IEnumerable<TDocument> documents, CancellationToken cancellationToken = default) where TDocument : class
        {
            try
            {
                var ids = new AddManyResponseModel();

                if (string.IsNullOrWhiteSpace(index))
                    throw new ArgumentNullException(nameof(index));

                if (documents.Any())
                {
                    var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

                    if (!exists.IsValid
                        || !exists.Exists)
                    {
                        throw new IndexNotFoundException(index);
                    }

                    var result = await _client.BulkAsync(b => 
                                        b.Index(index)
                                            .IndexMany<TDocument>(documents)
                                            .Timeout(ElasticConstants.REQUEST_TIMEOUT));

                    if (!result.IsValid)
                    {
                        throw new UnexpectedElasticException();
                    }

                    ids.Ids = new List<string>(result.Items.Select(s => s.Id).ToList());

                    if (result.Errors)
                    {
                        ids.ItemsWithErrors = new List<AddDocumentError>(
                                                result.ItemsWithErrors.Select(s =>
                                                    new AddDocumentError
                                                    {
                                                        Id = s.Id,
                                                        Error = s.Error.Reason
                                                    }));
                    }
                }

                return ids;
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

                throw new UnexpectedElasticException(ErrorMessages.ERROR_ADDITION_DOCUMENT(index), ex);
            }
        }
    }
}
