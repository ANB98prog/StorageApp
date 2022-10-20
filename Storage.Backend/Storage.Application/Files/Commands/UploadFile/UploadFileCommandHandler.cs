using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.UploadFile
{
    public class UploadFileCommandHandler
        : IRequestHandler<UploadFileCommand, string>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public UploadFileCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidateRequest(request);

                var id = Guid.NewGuid();
                var fileSystemName = $"{id.Trunc()}{Path.GetExtension(request.File.FileName)}";

                var uploadRequest = new UploadFileRequestModel
                {
                    Id = id,
                    OwnerId = request.UserId,
                    Attributes = request.Attributes,
                    MimeType = request.MimeType,
                    OriginalName = request.File.FileName,
                    SystemName = fileSystemName,
                    Stream = request.File.OpenReadStream()
                };

                var uploadedFileId = await _fileHandlerService.UploadFileAsync(uploadRequest, cancellationToken);

                return uploadedFileId.ToString();
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
                throw new FileHandlerServiceException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE);
            }
        }

        private void ValidateRequest(UploadFileCommand request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if(request.File == null)
                throw new ArgumentNullException(nameof(request.File));
        }
    }
}
