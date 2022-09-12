using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <param name="filePath">File to download</param>
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
                throw new ArgumentNullException(ex.Message, ex.InnerException);
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
            if(filesPath == null
                || !filesPath.Any())
            {
                throw new ArgumentNullException(nameof(filesPath));
            }

            var zipFilePath = await PrepareZipFileAsync(filesPath);

            var fileStream = await FileHelper.LoadFileAsync(zipFilePath);

            //FileHelper.RemoveFile(zipFilePath);

            return fileStream;
        }

        private async Task<string> PrepareZipFileAsync(List<string> filesPath)
        {
            // Copy files to temporary directory
            var randomName = Path.GetRandomFileName().Replace(".", string.Empty);
            var tempDir = Path.Combine(_tempDir, randomName);

            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);

            await FileHelper.CopyFilesToAsync(filesPath, tempDir);

            // Archives files
            var archiveFilePath = FileHelper.ArchiveFolder(tempDir);

            FileHelper.RemoveDirectory(tempDir);

            return archiveFilePath;
        }

        /// <summary>
        /// Uploads many files to storage
        /// </summary>
        /// <param name="file">Files to upload</param>
        /// <returns>Uploaded files path</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileUploadingException"></exception>
        public async Task<List<string>> UploadManyFilesAsync(List<FileModel> files, CancellationToken cancellationToken)
        {
            try
            {
                var filesPaths = new List<string>();

                foreach (var file in files)
                {
                    filesPaths.Add(
                        await UploadFileAsync(file, cancellationToken));
                }

                return filesPaths;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FileUploadingException("Unexpected error occured while many files uploading.", ex.InnerException);
            }
        }


        /// <summary>
        /// Upload file to storage
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns>Uploaded file path</returns>
        /// <exception cref="ArgumentNullException"></exception>
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
                throw ex;
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
