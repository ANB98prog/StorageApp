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

namespace Storage.Application.Files.Commands.UploadManyFiles
{
    public class UploadManyFilesCommandHandler
        : IRequestHandler<UploadManyFilesCommand, List<Guid>>
    {
        private readonly IFileHandlerService _fileHandlerService;

        public UploadManyFilesCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<List<Guid>> Handle(UploadManyFilesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var files = new List<UploadFileRequestModel>();

                foreach (var file in request.Files)
                {
                    var id = Guid.NewGuid();
                    var fileSystemName = $"{id.Trunc()}{Path.GetExtension(file.FileName)}";

                    files.Add(new UploadFileRequestModel
                    {
                        Id = id,
                        OwnerId = request.UserId,
                        Attributes = request.Attributes,
                        MimeType = FileHelper.GetFileMimeType(file.FileName),
                        OriginalName = file.FileName,
                        SystemName = fileSystemName,
                        Stream = file.OpenReadStream(),
                        IsAnnotated = request.IsAnnotated,
                    });
                }

                var filesIds = await _fileHandlerService.UploadManyFileAsync(files, cancellationToken);

                return filesIds;

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
