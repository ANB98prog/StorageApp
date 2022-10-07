using MediatR;
using Storage.Application.Common.Models;
using System;

namespace Storage.Application.Files.Commands.DeleteFile
{
    /// <summary>
    /// Delete file command
    /// </summary>
    public class DeleteFileCommand : IRequest<DeleteFileModel>
    {
        /// <summary>
        /// User that make request
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// File id to remove
        /// </summary>
        public Guid FileId { get; set; }
    }
}
