using AutoMapper;
using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Files.Commands.UpdateManyFiles;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.UpdateGroupFiles
{
    public class UpdateGroupFilesCommandHandler
        : IRequestHandler<UpdateGroupFilesCommand, UpdatedManyVm>
    {
        /// <summary>
        /// File handler service
        /// </summary>
        private readonly IFileHandlerService _fileHandlerService;

        /// <summary>
        /// Contract mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes class instance of <see cref="UpdateGroupFilesCommandHandler"/>
        /// </summary>
        /// <param name="fileHandlerService">File handler service</param>
        /// <param name="mapper">Contract mapper</param>
        public UpdateGroupFilesCommandHandler(IFileHandlerService fileHandlerService, IMapper mapper)
        {
            _fileHandlerService = fileHandlerService;
            _mapper = mapper;
        }

        public async Task<UpdatedManyVm> Handle(UpdateGroupFilesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidateParameters(request);

                var updates = ConvertUpdates(request);
                var updatedResult = await _fileHandlerService.UpdateBulkFilesAsync(updates, cancellationToken);

                return _mapper.Map<UpdateBulkFilesAttributesModel, UpdatedManyVm>(updatedResult);

            }
            catch (ArgumentNullException ex)
            {
                throw new UserException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (FileHandlerServiceException ex)
            {
                throw new CommandExecutionException(ex.UserFriendlyMessage, ex);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_PREPARING_ANNOTATION_DATA_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// Validates parameters
        /// </summary>
        /// <param name="request">Parameters to validste</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileHandlerServiceException"></exception>
        private void ValidateParameters(UpdateGroupFilesCommand request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            else if (!request.FilesIds.Any())
                throw new FileHandlerServiceException(ErrorMessages.EMPTY_FILES_IDS_ERROR_MESSAGE);
            else if (!request.Attributes.Any())
                throw new FileHandlerServiceException(ErrorMessages.EMPTY_FILE_ATTRIBUTES_ERROR_MESSAGE);
        }

        /// <summary>
        /// Converts updates
        /// </summary>
        /// <param name="request">Update request</param>
        /// <returns>Converted updates</returns>
        private List<UpdateFileAttributesModel> ConvertUpdates(UpdateGroupFilesCommand request)
        {
            var convertedResult = new List<UpdateFileAttributesModel>();

            foreach(var file in request.FilesIds)
            {
                convertedResult.Add(new UpdateFileAttributesModel
                {
                    Id = file,
                    Attributes = request.Attributes
                });
            }

            return convertedResult;
        }
    }
}
