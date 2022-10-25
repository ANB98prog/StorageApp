using AutoMapper;
using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.UpdateFile
{
    public class UpdateFileCommandHandler
        : IRequestHandler<UpdateFileCommand, UpdatedVm>
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
        /// Initializes class instance of <see cref="UpdateFileCommandHandler"/>
        /// </summary>
        /// <param name="fileHandlerService">File handler service</param>
        /// <param name="mapper">Contract mapper</param>
        public UpdateFileCommandHandler(IFileHandlerService fileHandlerService, IMapper mapper)
        {
            _fileHandlerService = fileHandlerService;
            _mapper = mapper;
        }

        public async Task<UpdatedVm> Handle(UpdateFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var preparedFilePath = await _fileHandlerService.UpdateFileAsync(_mapper.Map<UpdateFileCommand, UpdateFileAttributesModel>(request), cancellationToken);

                return new UpdatedVm(preparedFilePath.Acknowledged);

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
