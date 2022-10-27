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
        : IRequestHandler<DeleteFileCommand, DeleteFileResponseModel>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public DeleteFileCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<DeleteFileResponseModel> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            try
            {   
                var removeFileResult = await _fileHandlerService.RemoveFileAsync(request.FileId);

                return removeFileResult;
            }
            catch (ArgumentNullException ex)
            {
                throw new UserException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (NotFoundException ex)
            {
                throw ex;
            }
            catch (FileHandlerServiceException ex)
            {
                throw new CommandExecutionException(ex.UserFriendlyMessage, ex);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_FILE_REMOVE_MESSAGE);
            }
        }
    }
}
