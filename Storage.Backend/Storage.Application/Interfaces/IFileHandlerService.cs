using Storage.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    /// <summary>
    /// File handler service interface
    /// </summary>
    public interface IFileHandlerService
    {
        /// <summary>
        /// Uploads file
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded file id</returns>
        public Task<Guid> UploadFileAsync(UploadFileRequestModel file, CancellationToken cancellationToken);

        /// <summary>
        /// Downloads file
        /// </summary>
        /// <param name="filePath">File to download</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>File stream</returns>
        public Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);

        /// <summary>
        /// Uploads large file
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded files ids</returns>
        public Task<List<Guid>> UploadManyFileAsync(List<UploadFileRequestModel> files, CancellationToken cancellationToken);

        /// <summary>
        /// Uploads archived file
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded files ids</returns>
        public Task<List<Guid>> UploadArchiveFileAsync(UploadFileRequestModel file, CancellationToken cancellationToken);

        /// <summary>
        /// Downloads large file
        /// </summary>
        /// <param name="files">Files to download</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Downloading task</returns>
        public Task<FileStream> DownloadManyFilesAsync(List<string> files, CancellationToken cancellationToken);
    }
}
