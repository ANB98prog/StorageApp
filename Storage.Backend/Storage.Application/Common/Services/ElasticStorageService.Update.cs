using Elasticsearch.Exceptions;
using Elasticsearch.Models;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Common.Services
{
    public partial class ElasticStorageService
    {
        /// <summary>
        /// Updates file's attributes
        /// </summary>
        /// <param name="update">Update model</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated result</returns>
        /// <exception cref="ElasticStorageServiceException"></exception>
        public async Task<UpdatedFileAttributesResponseModel> UpdateFileAttributesAsync(UpdateFileAttributesModel update, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to update file attributes in elastic. Update file id: {update.Id}");

                if (update == null)
                {
                    throw new ArgumentNullException(nameof(update));
                }

                var result = await _elasticClient.UpdateAsync(_index, update.Id.ToString(), update, cancellationToken);

                _logger.Information("File's attributes were successfully updated.");

                return new UpdatedFileAttributesResponseModel(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new ElasticStorageServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPDATE_ITEM_IN_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPDATE_ITEM_IN_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPDATE_ITEM_IN_STORAGE_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Updates bulk files attributes
        /// </summary>
        /// <param name="updates">Bulk updates</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated result</returns>
        /// <exception cref="ElasticStorageServiceException"></exception>
        public async Task<UpdateBulkFilesAttributesModel> UpdateBulkFilesAttributesAsync(List<UpdateFileAttributesModel> updates, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to update bulk files attributes in elastic. Updates count: {updates.Count}");

                if (updates == null)
                {
                    throw new ArgumentNullException(nameof(updates));
                }

                var result = await _elasticClient.BulkUpdateAsync(_index, updates, cancellationToken);

                _logger.Information("Files attributes were successfully updated.");

                return _mapper.Map<UpdateManyResponse, UpdateBulkFilesAttributesModel>(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new ElasticStorageServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPDATE_ITEMS_IN_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPDATE_ITEMS_IN_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPDATE_ITEMS_IN_STORAGE_MESSAGE, ex);
            }
        }

    }
}
