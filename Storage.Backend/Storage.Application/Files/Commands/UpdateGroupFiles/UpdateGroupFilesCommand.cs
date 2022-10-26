using MediatR;
using Storage.Application.Files.Commands.UpdateManyFiles;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.UpdateGroupFiles
{
    public class UpdateGroupFilesCommand
        : IRequest<UpdatedManyVm>
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Files ids
        /// </summary>
        public List<Guid> FilesIds { get; set; }

        /// <summary>
        /// Attributes
        /// </summary>
        public List<string> Attributes { get; set; }
    }
}
