using Storage.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

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
        /// <typeparam name="T">File model type</typeparam>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded file id</returns>
        public Task<Guid> UploadFileAsync<T>(T file, CancellationToken cancellationToken) 
            where T : BaseFile;

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
        /// <typeparam name="T">File model type</typeparam>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploading task</returns>
        public Task UploadLargeFileAsync<T>(T file, CancellationToken cancellationToken)
            where T : BaseFile;

        /// <summary>
        /// Downloads large file
        /// </summary>
        /// <param name="files">Files to download</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Downloading task</returns>
        public Task<FileStream> DownloadManyFilesAsync(List<string> files, CancellationToken cancellationToken);
    }
}
