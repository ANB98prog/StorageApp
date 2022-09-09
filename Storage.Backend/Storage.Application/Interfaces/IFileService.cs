using Storage.Application.Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Interfaces
{
    /// <summary>
    /// Interface for fork with files
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="file">Upload file model</param>
        /// <returns>
        /// </returns>
        public Task<string> UploadFileAsync(FileModel file, CancellationToken cancellationToken);

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>File stream</returns>
        public Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken);

        /// <summary>
        /// Download many files
        /// </summary>
        /// <param name="filesPath">Path to file</param>
        /// <returns>Zip file stream</returns>
        public Task<FileStream> DownloadManyFilesAsync(List<string> filesPath, CancellationToken cancellationToken);
    }
}
