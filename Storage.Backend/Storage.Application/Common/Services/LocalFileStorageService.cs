﻿using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

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

        /// <summary>
        /// Temporary files directory
        /// </summary>
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="LocalStorageException"></exception>
        public async Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                return await FileHelper.LoadFileAsync(filePath);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            catch(FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new LocalStorageException("Unexpected error occured while file downloading.", ex.InnerException);
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
        /// <exception cref="LocalStorageException"></exception>
        public async Task<List<UploadedFileModel>> UploadManyFilesAsync(List<FileModel> files, CancellationToken cancellationToken)
        {
            try
            {
                if (files == null)
                    throw new ArgumentNullException(nameof(files));

                var filesPaths = new List<UploadedFileModel>();

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
                throw new LocalStorageException("Unexpected error occured while many files uploading.", ex.InnerException);
            }
        }


        /// <summary>
        /// Upload file to storage
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <returns>Uploaded file path</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="LocalStorageException"></exception>
        public async Task<UploadedFileModel> UploadFileAsync(FileModel file, CancellationToken cancellationToken)
        {
            try
            {
                ValidateFile(file);

                var formattedAttr = file.Attributes
                                        .ToList()
                                            .ConvertAll(s => s.ToUnderScore());

                var relativePath = Path.Combine(formattedAttr.ToArray());
                relativePath = Path.Combine(relativePath, file.FileName);

                var absolutePath = Path.Combine(_localStorageDir, relativePath);

                await FileHelper.SaveFileAsync(file.FileStream, absolutePath, cancellationToken);

                return new UploadedFileModel
                {
                    FullPath = absolutePath,
                    RelativePath = relativePath,
                };
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new LocalStorageException("Unexpected error occured while file uploading.", ex.InnerException);
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
                    || file.FileStream == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
        }

        public void Dispose()
        {
        }
    }
}
