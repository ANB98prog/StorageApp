﻿using AutoMapper;
using Serilog;
using Serilog.Sinks.File;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using FileAttributes = Storage.Application.Common.Models.FileAttributes;

namespace Storage.Application.Common.Services
{
    public class FileHandlerService : IFileHandlerService
    {
        /// <summary>
        /// Contract mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Files service
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// Directory for temporary files
        /// </summary>
        private readonly string TEMP_DIR = Path.Combine(Environment.CurrentDirectory, "temp");

        /// <summary>
        /// Initializes class instance of <see cref="FileHandlerService"/>
        /// </summary>
        /// <param name="mapper">Contract mapper</param>
        /// <param name="fileService">Files service</param>
        /// <param name="logger">Logger</param>
        public FileHandlerService(IMapper mapper, ILogger logger, IFileService fileService)
        {
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;

            if (!Directory.Exists(TEMP_DIR))
                Directory.CreateDirectory(TEMP_DIR);
        }

        public async Task<FileStream> DownloadFileAsync(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                var fileName = (!string.IsNullOrEmpty(filePath)) ?
                                    $"File name '{Path.GetFileName(filePath)}'"
                                        : string.Empty;

                _logger.Information($"Try to download file. {fileName}");

                if (string.IsNullOrEmpty(filePath))
                    throw new ArgumentNullException(nameof(filePath));

                return await _fileService.DownloadFileAsync(filePath, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILE_MESSAGE);
                throw new FileUploadingException(ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILE_MESSAGE, ex);
            }
        }

        public async Task<FileStream> DownloadManyFilesAsync(List<string> filesPath, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to download files.");

                if (filesPath != null
                        && filesPath.Count == 0)
                {
                    throw new ArgumentNullException(nameof(filesPath));
                }

                var files = await _fileService.DownloadManyFilesAsync(filesPath, cancellationToken);

                _logger.Information($"Files were successfully downloaded.");

                return files;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILES_MESSAGE);
                throw new FileUploadingException(ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILES_MESSAGE, ex);
            }
        }

        public async Task<List<Guid>> UploadArchiveFileAsync(UploadFileRequestModel file, CancellationToken cancellationToken)
        {
            try
            {
                var fileName = (file != null
                                && !string.IsNullOrEmpty(file.OriginalName)) ?
                                    $"File name '{file.OriginalName}'"
                                        : string.Empty;

                _logger.Information($"Try to upload archive file. {fileName}");

                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                var files = await LoadArchiveFilesAsync(file, cancellationToken);

                var filesIds = await UploadManyFileAsync(files, cancellationToken);

                _logger.Information($"Archive file were successfully uploaded. Files count: {filesIds.Count}");

                return filesIds;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE);
                throw new FileUploadingException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Loads archive files
        /// </summary>
        /// <param name="archive">Archive to load</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Loaded files</returns>
        private async Task<List<UploadFileRequestModel>> LoadArchiveFilesAsync(UploadFileRequestModel archive, CancellationToken cancellationToken)
        {
            var filesData = new List<UploadFileRequestModel>();

            var tempPath = Path.Combine(TEMP_DIR, archive.SystemName);

            /*Save archive in local storage*/
            await FileHelper.SaveFileAsync(archive.Stream, tempPath, cancellationToken);

            /*Unzip*/
            var unzipPath = FileHelper.UnzipFolder(tempPath);

            var files = Directory.GetFiles(unzipPath);

            /*Need to add attributes to each file*/
            if (files.Any())
            {
                foreach (var file in files)
                {
                    var stream = await FileHelper.LoadFileAsync(file);

                    var fileId = Guid.NewGuid();
                    var systemName = $"{fileId.Trunc()}{Path.GetExtension(file)}";

                    FileType fileType;

                    try
                    {
                        fileType = FileHelper.GetFileType(file);
                    }
                    catch (NotSupportedFileTypeException)
                    {
                        fileType = FileType.Unknown;
                    }

                    filesData.Add(new UploadFileRequestModel
                    {
                        Id = fileId,
                        SystemName = systemName,
                        OriginalName = Path.GetFileName(file),
                        Attributes = archive.Attributes,
                        FileExtension = Path.GetExtension(file),
                        FileType = fileType,
                        IsAnnotated = archive.IsAnnotated,
                        DepartmentOwnerId = archive.DepartmentOwnerId,
                        OwnerId = archive.OwnerId,
                        Stream = stream
                    });
                }
            }

            /*Need to remove archive*/
            FileHelper.RemoveFile(tempPath);

            return filesData;
        }

        /// <summary>
        /// Uploads file to server
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded file id</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Guid> UploadFileAsync(UploadFileRequestModel file, CancellationToken cancellationToken)
        {
            try
            {
                var fileName = (file != null
                                && !string.IsNullOrEmpty(file.OriginalName)) ?
                                    $"File name '{file.OriginalName}'"
                                        : string.Empty;

                _logger.Information($"Try to upload file. {fileName}");

                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                var result = await UploadFileToStorageAsync(file, cancellationToken);

                _logger.Information($"File was successfully uploaded.");

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE);
                throw new FileUploadingException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE, ex);
            }
        }

        public async Task<List<Guid>> UploadManyFileAsync(List<UploadFileRequestModel> files, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to upload files. Files count: {files?.Count}");

                if (files == null)
                {
                    throw new ArgumentNullException(nameof(files));
                }

                var ids = new List<Guid>();
                foreach (var file in files)
                {
                    ids.Add(await UploadFileToStorageAsync(file, cancellationToken));
                }

                _logger.Information($"Files uploaded successfully.");

                return ids;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE);
                throw new FileUploadingException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Uploads file to storage
        /// </summary>
        /// <param name="upload">Data to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotSupportedFileTypeException"></exception>
        /// <returns>File id</returns>
        private async Task<Guid> UploadFileToStorageAsync(UploadFileRequestModel upload, CancellationToken cancellationToken)
        {
            var file = new FileModel
            {
                FileName = upload.SystemName,
                Attributes = upload.IsAnnotated ?
                        new string[] { FileAttributes.Annotated.ToString() }
                            : new string[] { FileAttributes.NotAnnotated.ToString() },
                FileStream = upload.Stream
            };

            upload.FileType = FileHelper.GetFileType(upload.OriginalName);

            // Save file to storage
            var savedFilePath = await _fileService.UploadFileAsync(file, cancellationToken);

            upload.OriginalFilePath = savedFilePath;

            // Index file in db

            return upload.Id;
        }
    }
}
