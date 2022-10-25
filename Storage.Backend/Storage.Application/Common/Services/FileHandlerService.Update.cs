using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Common.Services
{
    public partial class FileHandlerService
    {
        /// <summary>
        /// Updates file
        /// </summary>
        /// <param name="update">Update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Update action result</returns>
        /// <exception cref="FileHandlerServiceException"></exception>
        public async Task<UpdatedFileAttributesResponseModel> UpdateFileAsync(UpdateFileAttributesModel update, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to update file. File id: {update.Id}");

                ValidateUpdateParameters(update);

                var result = await _storageDataService.UpdateFileAttributesAsync(update, cancellationToken);

                if(result != null
                    && result.Acknowledged)
                {
                    _logger.Information($"File has been successfully updated.");
                }else
                {
                    _logger.Information($"File has not been updated.");
                }

                return result;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (FileHandlerServiceException ex)
            {
                _logger.Error(ex.UserFriendlyMessage);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Validates input parameters
        /// </summary>
        /// <param name="update">Parameters</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileHandlerServiceException"></exception>
        private void ValidateUpdateParameters(UpdateFileAttributesModel update)
        {
            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }
            else if (update.Id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("Id");
            }
            else if (update.Attributes == null
                || !update.Attributes.Any())
            {
                throw new FileHandlerServiceException(ErrorMessages.EMPTY_FILE_ATTRIBUTES_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Updates bulk of files
        /// </summary>
        /// <param name="updates">Updates</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Update action result</returns>
        /// <exception cref="FileHandlerServiceException"></exception>
        public async Task<UpdateBulkFilesAttributesModel> UpdateBulkFilesAsync(List<UpdateFileAttributesModel> updates, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to update bulk of files. Files count: {updates.Count}");

                ValidateBulkUpdateParameters(updates);

                var result = await _storageDataService.UpdateBulkFilesAttributesAsync(updates, cancellationToken);

                if (result != null
                    && result.Acknowledged)
                {
                    _logger.Information($"Files has been successfully updated.");
                }
                else
                {
                    _logger.Information($"Files has not been updated.");
                }

                return result;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (FileHandlerServiceException ex)
            {
                _logger.Error(ex.UserFriendlyMessage);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Validates list input parameters
        /// </summary>
        /// <param name="updates">List of parametes</param>
        /// <exception cref="FileHandlerServiceException"></exception>
        private void ValidateBulkUpdateParameters(List<UpdateFileAttributesModel> updates)
        {
            if(updates == null
                || !updates.Any())
            {
                throw new FileHandlerServiceException(ErrorMessages.EMPTY_FILE_ATTRIBUTES_ERROR_MESSAGE);
            }

            foreach (var update in updates)
            {
                ValidateUpdateParameters(update);
            }
        }
    }
}
