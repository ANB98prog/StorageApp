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
        /// Uploads many files
        /// </summary>
        /// <param name="files">Upload files model</param>
        /// <returns>
        /// Path to saved files
        /// </returns>
        public Task<List<UploadedFileModel>> UploadManyFilesAsync(List<FileModel> files, CancellationToken cancellationToken);

        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="file">Upload file model</param>
        /// <returns>
        /// Path to saved file
        /// </returns>
        public Task<UploadedFileModel> UploadFileAsync(FileModel file, CancellationToken cancellationToken);

        /// <summary>
        /// Uploads temporary file
        /// </summary>
        /// <param name="file">Upload file model</param>
        /// <returns>
        /// Path to saved file
        /// </returns>
        public Task<UploadedFileModel> UploadTemporaryFileAsync(FileStream file, CancellationToken cancellationToken);

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

        /// <summary>
        /// Downloads files from local storage as separate files
        /// </summary>
        /// <param name="filesPath">Files path</param>
        /// <returns>Files</returns>
        public Task<List<FileStream>> DownloadManyFilesSeparateAsync(List<string> filesPath);

        /// <summary>
        /// Deletes file from storage
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns></returns>
        public void DeleteFile(string filePath);

        /// <summary>
        /// Deletes files from storage
        /// </summary>
        /// <param name="filesPath">Files paths</param>
        public DeleteFilesResponseModel DeleteFiles(List<string> filesPath);
    }
}
