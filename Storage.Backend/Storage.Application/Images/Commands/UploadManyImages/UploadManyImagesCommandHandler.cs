using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Images.Commands.UploadManyImages
{
    public class UploadManyImagesCommandHandler
        : IRequestHandler<UploadManyImagesCommand, List<Guid>>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public UploadManyImagesCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<List<Guid>> Handle(UploadManyImagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var id = Guid.NewGuid();
                var fileSystemName = $"{id.Trunc()}{Path.GetExtension(request.ImagesZipFile.FileName)}";

                var uploadRequest = new UploadFileRequestModel
                {
                    Id = id,
                    OwnerId = request.UserId,
                    Attributes = request.Attributes,
                    FileExtension = Path.GetExtension(request.ImagesZipFile.FileName),
                    OriginalName = request.ImagesZipFile.FileName,
                    SystemName = fileSystemName,
                    Stream = request.ImagesZipFile.OpenReadStream(),
                    IsAnnotated = request.IsAnnotated,
                };

                var uploadedFilesIds = await _fileHandlerService.UploadArchiveFileAsync(uploadRequest, cancellationToken);

                return uploadedFilesIds;
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
    }
}
