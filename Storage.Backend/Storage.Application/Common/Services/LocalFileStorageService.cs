using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Common.Services
{
    /// <summary>
    /// Local file storage service
    /// </summary>
    public class LocalFileStorageService : IFileService, IDisposable
    {
        /// <summary>
        /// Local storage directory path
        /// </summary>
        private readonly string _localStorageDir;

        private readonly string _tempDir;

        public LocalFileStorageService(string localStorageDir)
        {
            if (string.IsNullOrEmpty(localStorageDir))
                throw new ArgumentNullException(nameof(localStorageDir));

            if (!Directory.Exists(localStorageDir))
            {
                throw new DirectoryNotFoundException("Storage directory should exist!");
            }

            _localStorageDir = localStorageDir;
            _tempDir = Path.Combine(_localStorageDir, "temp");

            if (!Directory.Exists(_tempDir))
            {
                Directory.CreateDirectory(_tempDir);
            }
        }

        /// <summary>
        /// Downloads file from local storage
        /// </summary>
        /// <param name="filePath">FIle to download</param>
        /// <returns>File stream result</returns>
        /// <exception cref="FileUploadingException"></exception>
        public async Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                return await FileHelper.LoadFileAsync(filePath);
            }
            catch (ArgumentNullException ex)
            {
                throw new FileUploadingException(ex.Message, ex.InnerException);
            }
            catch(FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileUploadingException("Unexpected error occured while file downloading.", ex.InnerException);
            }
        }

        /// <summary>
        /// Downloads files from local storage
        /// </summary>
        /// <param name="filesPath">Files path</param>
        /// <param name="cancellationToken">Cancellation tocken</param>
        /// <returns>Zip file</returns>
        public async Task<FileStream> DownloadManyFilesAsync(List<string> filesPath, CancellationToken cancellationToken)
        {
            var zipFilePath = await PrepareZipFileAsync(filesPath);

            var fileStream = FileHelper.LoadFileAsync(zipFilePath);


        }

        private async Task<string> PrepareZipFileAsync(List<string> filesPath)
        {
            // Copy files to temporary directory
            var randomName = Path.GetRandomFileName();
            var tempDir = Path.Combine(_tempDir, randomName);

            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            await FileHelper.MoveFilesToAsync(filesPath, tempDir);

            var archiveFilePath = Path.Combine(_tempDir, randomName);

            archiveFilePath = FileHelper.ArchiveFolder(tempDir, archiveFilePath);

            return archiveFilePath;
        }

        /// <summary>
        /// Upload file to local storage
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns>Uploaded file path</returns>
        /// <exception cref="FileUploadingException"></exception>
        public async Task<string> UploadFileAsync(FileModel file, CancellationToken cancellationToken)
        {
            try
            {
                ValidateFile(file);

                var path = Path.Combine(_localStorageDir,
                            string.Join(Path.DirectorySeparatorChar, file.Attributes),
                                file.FileName);

                await FileHelper.SaveFileAsync(file.FileStream, path, cancellationToken);

                return path;
            }
            catch (ArgumentNullException ex)
            {
                throw new FileUploadingException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new FileUploadingException("Unexpected error occured while file uploading.", ex.InnerException);
            }
        }

        /// <summary>
        /// Validates file argument
        /// </summary>
        /// <param name="file">Argument</param>
        /// <exception cref="ArgumentNullException"></exception>
        private void ValidateFile(FileModel file)
        {
            if (file == null
                    || file.FileStream?.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }
        }

        public void Dispose()
        {
            
        }
    }
}
