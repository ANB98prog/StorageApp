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
                    ContentType = request.ImageFile.ContentType,
                    Stream = request.ImageFile.OpenReadStream(),
                    IsAnnotated = request.IsAnnotated,
                };

                var uploadedFileId = await _fileHandlerService.UploadFileAsync(uploadRequest, cancellationToken);

                return uploadedFileId;
            }
            catch (ArgumentNullException ex)
            {
                throw new FileUploadingException(ex.Message, $"Parameter '{ex.ParamName}' is not set!");
            }
            catch (FileUploadingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileUploadingException(ex.Message, $"Unexpected error occured while file uploading.");
            }
        }
    }
}
