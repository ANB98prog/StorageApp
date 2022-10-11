using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.DeleteFile
{
    public class DeleteFileCommandHandler
        : IRequestHandler<DeleteFileCommand, DeleteFileModel>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public DeleteFileCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<DeleteFileModel> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            try
            {   
                var removeFileResult = await _fileHandlerService.RemoveFileAsync(request.FileId);

                return removeFileResult;
            }
            catch (ArgumentNullException ex)
            {
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (FileHandlerServiceException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_FILE_REMOVE_MESSAGE);
            }
        }
    }
}
