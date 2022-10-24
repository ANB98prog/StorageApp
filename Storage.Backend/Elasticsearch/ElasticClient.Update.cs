using Elasticsearch.Exceptions;
using Elasticsearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elasticsearch
{
    public partial class ElasticClient
    {
        public async Task<bool> UpdateAsync<TDocument>(string index, string documentId, TDocument document, CancellationToken cancellationToken = default) 
            where TDocument : class
        {
            if (string.IsNullOrWhiteSpace(index))
                throw new ArgumentNullException(nameof(index));
            if (string.IsNullOrWhiteSpace(documentId))
                throw new ArgumentNullException(nameof(documentId));
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

            if (!exists.IsValid
                || !exists.Exists)
            {
                throw new IndexNotFoundException(index);
            }

            var result = await _client.UpdateAsync<TDocument>(documentId, 
                                    s => s.
                                        Index(index)
                                            .Doc(document));

            if (!result.IsValid
                 && result.ServerError != null)
            {
                throw new DeleteDocumentException(ErrorMessages.ERROR_UPDATE_BY_ID_DOCUMENT(index, documentId), result.OriginalException);
            }
            else if (!result.IsValid)
            {
                return false;
            }

            await _client.Indices.RefreshAsync(index, ct: cancellationToken);

            return true;
        }

        /// <summary>
        /// Updated bulk of documents
        /// </summary>
        /// <typeparam name="TDocument">Document type</typeparam>
        /// <param name="index">Index</param>
        /// <param name="documents">Documents to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated response</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexNotFoundException"></exception>
        /// <exception cref="DeleteDocumentException"></exception>
        public async Task<UpdateManyResponse> BulkUpdateAsync<TDocument>(string index, List<TDocument> documents, CancellationToken cancellationToken = default)
            where TDocument : class
        {
            if (string.IsNullOrWhiteSpace(index))
                throw new ArgumentNullException(nameof(index));
            if (documents != null
                && !documents.Any())
                throw new ArgumentNullException(nameof(documents));

            var exists = await _client.Indices.ExistsAsync(index, ct: cancellationToken);

            if (!exists.IsValid
                || !exists.Exists)
            {
                throw new IndexNotFoundException(index);
            }

            var result = await _client.BulkAsync(update => update
                                .Index(index)
                                .Refresh(Net.Refresh.True)
                                .UpdateMany<TDocument>(documents, 
                                    (bu, d) => bu.Doc(d)));

            if (!result.IsValid
                 && result.ServerError != null)
            {
                throw new DeleteDocumentException(ErrorMessages.ERROR_BULK_UPDATE_DOCUMENTS, result.OriginalException);
            }
            else if (!result.IsValid)
            {
                return new UpdateManyResponse
                {
                    Acknowledged = false
                };
            }

            var response = new UpdateManyResponse
            {
                Acknowledged = true,
                Count = result.Items.Count
            };

            if (result.Errors)
            {
                response.ItemsWithErrors = new List<AddDocumentError>(
                                        result.ItemsWithErrors.Select(s =>
                                            new AddDocumentError
                                            {
                                                Id = s.Id,
                                                Error = s.Error.Reason
                                            }));
            }

            return response;
        }
    }
}
