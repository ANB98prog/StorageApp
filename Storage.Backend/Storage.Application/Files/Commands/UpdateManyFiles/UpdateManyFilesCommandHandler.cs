using AutoMapper;
using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.UpdateManyFiles
{
    public class UpdateManyFilesCommandHandler
        : IRequestHandler<UpdateManyFilesCommand, UpdatedManyVm>
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
        /// Initializes class instance of <see cref="UpdateManyFilesCommandHandler"/>
        /// </summary>
        /// <param name="fileHandlerService">File handler service</param>
        /// <param name="mapper">Contract mapper</param>
        public UpdateManyFilesCommandHandler(IFileHandlerService fileHandlerService, IMapper mapper)
        {
            _fileHandlerService = fileHandlerService;
            _mapper = mapper;
        }

        public async Task<UpdatedManyVm> Handle(UpdateManyFilesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var updatedResult = await _fileHandlerService.UpdateBulkFilesAsync(_mapper.Map<List<FileUpdateData>, List<UpdateFileAttributesModel>>(request.Updates), cancellationToken);

                return _mapper.Map<UpdateBulkFilesAttributesModel, UpdatedManyVm>(updatedResult);

            }
            catch (ArgumentNullException ex)
            {
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (FileHandlerServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_PREPARING_ANNOTATION_DATA_ERROR_MESSAGE);
            }
        }
    }
}
