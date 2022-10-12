using MediatR;
using System;
using System.IO;

namespace Storage.Application.Files.Commands.DownloadFile
{
    /// <summary>
    /// Download file command
    /// </summary>
    public class DownloadFileCommand
        : IRequest<FileStream>
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// File to download
        /// </summary>
        public Guid FileId { get; set; }
    }
}
