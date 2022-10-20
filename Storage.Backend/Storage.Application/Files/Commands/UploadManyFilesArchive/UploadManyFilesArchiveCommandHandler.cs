using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.UploadManyFilesArchive
{
    public class UploadManyFilesArchiveCommandHandler
        : IRequestHandler<UploadManyFilesArchiveCommand, List<Guid>>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public UploadManyFilesArchiveCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<List<Guid>> Handle(UploadManyFilesArchiveCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var uploadedFilesIds = new List<Guid>();

                if (request != null
                    && request.Files.Any())
                {
                    foreach (var archive in request.Files)
                    {
                        var id = Guid.NewGuid();
                        var fileSystemName = $"{id.Trunc()}{Path.GetExtension(archive.FileName)}";

                        var uploadRequest = new UploadFileRequestModel
                        {
                            Id = id,
                            OwnerId = request.UserId,
                            Attributes = request.Attributes,
                            OriginalName = archive.FileName,
                            SystemName = fileSystemName,
                            Stream = archive.OpenReadStream()
                        };

                        uploadedFilesIds.AddRange(await _fileHandlerService.UploadArchiveFileAsync(uploadRequest, request.MimeTypes, cancellationToken));
                    } 
                }

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
