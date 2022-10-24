using MediatR;
using Storage.Domain;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.PrepareAnnotatedFiles
{
    /// <summary>
    /// Prepare annotated files command
    /// </summary>
    public class PrepareAnnotatedFilesCommand
        : IRequest<string>
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Annotated files ids
        /// </summary>
        public List<Guid> AnnotatedFilesIds { get; set; }

        /// <summary>
        /// Annotation format
        /// </summary>
        public AnnotationFormats AnnotationFormat { get; set; }
    }
}
