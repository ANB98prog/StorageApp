using MediatR;
using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Storage.Application.Files.Commands.DeleteFiles
{
    /// <summary>
    /// Delete files command
    /// </summary>
    public class DeleteFilesCommand : IRequest<DeleteFilesResponseModel>
    {
        /// <summary>
        /// Files ids
        /// </summary>
        public List<string> FilesIds { get; set; }

        /// <summary>
        /// User that initializes deletion
        /// </summary>
        public Guid UserId { get; set; }
    }
}
