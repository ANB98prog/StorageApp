using Storage.Application.Common.Models;
using System.IO;

namespace Storage.Application.Interfaces
{
    /// <summary>
    /// Interface for fork with files
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Upload small file
        /// </summary>
        /// <param name="file">Upload file model</param>
        /// <returns>
        /// </returns>
        public System.Threading.Tasks.Task UploadSmallFileAsync(UploadFileModel file);

        /// <summary>
        /// Upload large file
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns></returns>
        public Storage.Domain.Task UploadLargeFileAsync(UploadFileModel file);

        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>File stream</returns>
        public FileStream DownloadFileAsync(string filePath);

        /// <summary>
        /// Prepares large file to download
        /// </summary>
        /// <param name="request">Preparing request</param>
        /// <returns>Preparing files</returns>
        public PrepareFilesResponseModel PrepareLargeFileAsync(PrepareFilesRequestModel request);
    }
}
