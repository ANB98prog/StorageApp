using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Images.Commands.UploadImage
{
    public class UploadImageCommandHandler
        : IRequestHandler<UploadImageCommand, Guid>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public UploadImageCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<Guid> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidateRequest(request);

                var id = Guid.NewGuid();
                var fileSystemName = $"{id.Trunc()}{Path.GetExtension(request.ImageFile.FileName)}";

                var uploadRequest = new UploadFileRequestModel
                {
                    Id = id,
                    OwnerId = request.UserId,
                    Attributes = request.Attributes,
                    FileExtension = Path.GetExtension(request.ImageFile.FileName),
                    OriginalName = request.ImageFile.FileName,
                    SystemName = fileSystemName,
                    Stream = request.ImageFile.OpenReadStream(),
                    IsAnnotated = request.IsAnnotated,
                };

                var uploadedFileId = await _fileHandlerService.UploadFileAsync(uploadRequest, cancellationToken);

                return uploadedFileId;
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

        private void ValidateRequest(UploadImageCommand request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if(request.ImageFile == null)
                throw new ArgumentNullException(nameof(request.ImageFile));
        }
    }
}
