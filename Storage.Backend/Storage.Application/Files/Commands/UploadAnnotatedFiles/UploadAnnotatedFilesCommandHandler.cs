﻿using MediatR;
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

namespace Storage.Application.Files.Commands.UploadAnnotatedFiles
{
    /// <summary>
    /// Upload annotated files command handler
    /// </summary>
    public class UploadAnnotatedFilesCommandHandler
        : IRequestHandler<UploadAnnotatedFilesCommand, ManyFilesActionResponse<List<Guid>>>
    {
        /// <summary>
        /// File handler service
        /// </summary>
        private readonly IFileHandlerService _fileHandlerService;

        /// <summary>
        /// Initializes class instance of <see cref="UploadAnnotatedFilesCommandHandler"/>
        /// </summary>
        /// <param name="fileHandlerService">File handler service</param>
        public UploadAnnotatedFilesCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<ManyFilesActionResponse<List<Guid>>> Handle(UploadAnnotatedFilesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new ManyFilesActionResponse<List<Guid>>();
                var errors = new List<DeleteErrorModel>();
                var uploadedFilesIds = new List<Guid>();

                if (request != null
                    && request.Files.Any())
                {
                    foreach (var archive in request.Files)
                    {
                        try
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
                                Stream = archive.OpenReadStream(),
                            };

                            uploadedFilesIds.AddRange(await _fileHandlerService.UploadAnnotatedFileAsync(uploadRequest, request.AnnotationFormat, cancellationToken));
                        }
                        catch (FileHandlerServiceException ex)
                        {
                            errors.Add(new DeleteErrorModel
                            {
                                FilePath = archive.FileName,
                                ErrorMessage = ex.UserFriendlyMessage
                            });
                        }
                    }
                }

                response.Data = uploadedFilesIds;
                response.Errors = errors;

                return response;
            }
            catch (ArgumentNullException ex)
            {
                throw new UserException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (FileHandlerServiceException ex)
            {
                throw new CommandExecutionException(ex.UserFriendlyMessage, ex);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE);
            }
        }
    }
}
