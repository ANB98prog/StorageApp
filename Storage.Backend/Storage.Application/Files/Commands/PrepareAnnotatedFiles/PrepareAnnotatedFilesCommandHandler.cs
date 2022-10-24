﻿using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Interfaces;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Files.Commands.PrepareAnnotatedFiles
{
    /// <summary>
    /// Prepare annotated files command handler
    /// </summary>
    public class PrepareAnnotatedFilesCommandHandler
        : IRequestHandler<PrepareAnnotatedFilesCommand, string>
    {
        private readonly IFileHandlerService _fileHandlerService;

        /// <summary>
        /// Initializes class instance of <see cref="PrepareAnnotatedFilesCommandHandler"/>
        /// </summary>
        /// <param name="fileHandlerService">File handler service</param>
        public PrepareAnnotatedFilesCommandHandler(IFileHandlerService fileHandlerService)
        {
            _fileHandlerService = fileHandlerService;
        }

        public async Task<string> Handle(PrepareAnnotatedFilesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var preparedFilePath = await _fileHandlerService.PrepareAnnotatedFileAsync(request.AnnotatedFilesIds, request.AnnotationFormat, cancellationToken);

                return string.Join("/", (preparedFilePath.Split(Path.DirectorySeparatorChar)));

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
