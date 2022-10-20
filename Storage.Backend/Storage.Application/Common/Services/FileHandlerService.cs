using AutoMapper;
using Serilog;
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
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;
using FileAttributes = Storage.Application.Common.Models.FileAttributes;

namespace Storage.Application.Common.Services
{
    public partial class FileHandlerService : IFileHandlerService, IDisposable
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Contract mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Files service
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// Metadata storage service
        /// </summary>
        private readonly IStorageDataService _storageDataService;

        /// <summary>
        /// Annotation format processors
        /// </summary>
        private Dictionary<AnnotationFormats, IAnnotatedDataProcessor> _annotationsFormatsProcessor;

        /// <summary>
        /// Directory for temporary files
        /// </summary>
        private readonly string TEMP_DIR = Path.Combine(Environment.CurrentDirectory, "temp");

        /// <summary>
        /// Initializes class instance of <see cref="FileHandlerService"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="mapper">Contract mapper</param>
        /// <param name="fileService">Files service</param>
        /// <param name="storageDataService">Storage data service</param>
        public FileHandlerService(ILogger logger, IMapper mapper, IFileService fileService, IStorageDataService storageDataService)
        {
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
            _storageDataService = storageDataService;

            if (!Directory.Exists(TEMP_DIR))
                Directory.CreateDirectory(TEMP_DIR);

            _annotationsFormatsProcessor = InitializeAnnotationsProcessors();
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

                var file = await _fileService.DownloadFileAsync(filePath, cancellationToken);

                _logger.Information($"File was successfully downloaded.");

                return file;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, ErrorMessages.InvalidRequiredParameterErrorMessage(ex.Message));
                throw new FileHandlerServiceException(ErrorMessages.InvalidRequiredParameterErrorMessage(ex.Message), ex);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(ex, ErrorMessages.FileNotFoundErrorMessage(ex.FileName));
                throw new FileHandlerServiceException(ErrorMessages.FileNotFoundErrorMessage(ex.FileName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILE_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILE_MESSAGE, ex);
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
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(ex, ErrorMessages.FileNotFoundErrorMessage(ex.FileName));
                throw new FileHandlerServiceException(ErrorMessages.FileNotFoundErrorMessage(ex.FileName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILES_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_DOWNLOAD_FILES_MESSAGE, ex);
            }
        }

        public async Task<List<Guid>> UploadArchiveFileAsync(UploadFileRequestModel file, List<string> mimeTypes, CancellationToken cancellationToken)
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

                var files = await UploadArchiveFilesAsync(file, mimeTypes, cancellationToken);

                var filesIds = await UploadManyFileAsync(files, cancellationToken);

                files.ForEach(async f => await f.Stream.DisposeAsync());

                _logger.Information($"Archive file were successfully uploaded. Files count: {filesIds.Count}");

                return filesIds;

            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(ex, ErrorMessages.FileNotFoundErrorMessage(ex.FileName));
                throw new FileHandlerServiceException(ErrorMessages.FileNotFoundErrorMessage(ex.FileName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Uploads archive files
        /// </summary>
        /// <param name="archive">Archive to load</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Loaded files</returns>
        private async Task<List<UploadFileRequestModel>> UploadArchiveFilesAsync(UploadFileRequestModel archive, List<string> mimeTypes, CancellationToken cancellationToken)
        {
            var filesData = new List<UploadFileRequestModel>();

            var tempPath = Path.Combine(TEMP_DIR, archive.SystemName);

            /*Save archive in local storage*/
            await FileHelper.SaveFileAsync(archive.Stream, tempPath, cancellationToken);

            /*Unzip*/
            var unzipPath = FileHelper.UnzipFolder(tempPath);

            var files = Directory.GetFiles(unzipPath, "*.*", SearchOption.AllDirectories);

            /*Need to add attributes to each file*/
            if (files.Any())
            {
                var supportedExtensions = new List<string>();

                foreach (var mimetype in mimeTypes)
                {
                    supportedExtensions.AddRange(MimeTypes.GetMimeTypeExtensions(mimetype)); 
                }

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file).TrimStart('.');

                    if (supportedExtensions.Contains(extension))
                    {
                        var stream = await FileHelper.LoadFileAsync(file);

                        var fileId = Guid.NewGuid();
                        var systemName = $"{fileId.Trunc()}{Path.GetExtension(file)}";

                        filesData.Add(new UploadFileRequestModel
                        {
                            Id = fileId,
                            SystemName = systemName,
                            OriginalName = Path.GetFileName(file),
                            Attributes = archive.Attributes,
                            MimeType = FileHelper.GetFileMimeType(file),
                            DepartmentOwnerId = archive.DepartmentOwnerId,
                            OwnerId = archive.OwnerId,
                            Stream = stream
                        }); 
                    }
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
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILE_MESSAGE, ex);
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

                files.ForEach(async f => await f.Stream.DisposeAsync());

                _logger.Information($"Files uploaded successfully.");

                return ids;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Uploads file to storage
        /// </summary>
        /// <param name="upload">Data to upload</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>File id</returns>
        private async Task<Guid> UploadFileToStorageAsync(UploadFileRequestModel upload, CancellationToken cancellationToken)
        {
            var fileAttributes = new List<string>();

            fileAttributes.Add(upload.IsAnnotated ?
                         FileAttributes.Annotated.ToString()
                            : FileAttributes.NotAnnotated.ToString());
            fileAttributes.AddRange(FileHelper.GetFileAttributesByMimeData(upload.MimeType));
            

            if (upload.Attributes != null
                    && upload.Attributes.Any())
            {
                fileAttributes.AddRange(upload.Attributes.OrderByDescending(s => s)); 
            }

            var file = new FileModel
            {
                FileName = upload.SystemName,
                Attributes = fileAttributes.ToArray(),
                FileStream = upload.Stream
            };

            // Save file to storage
            var savedFilePath = await _fileService.UploadFileAsync(file, cancellationToken);

            upload.FilePath = savedFilePath.RelativePath;

            // Index file in db
            var indexedDataId = await _storageDataService.AddDataToStorageAsync<BaseFile>(upload);

            await upload.Stream.DisposeAsync();

            return indexedDataId;
        }

        /// <summary>
        /// Removes file
        /// </summary>
        /// <param name="id">File id to remove</param>
        /// <returns><see cref="DeleteFileModel"/></returns>
        /// <exception cref="FileHandlerServiceException"></exception>
        public async Task<DeleteFileModel> RemoveFileAsync(Guid id)
        {
            try
            {
                _logger.Information($"Try to remove file. File id: {id}");

                var result = new DeleteFileModel();

                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                var file = await _storageDataService.GetFileInfoAsync<FileInfoModel>(id);

                if(file != null
                    && !string.IsNullOrWhiteSpace(file.FilePath))
                {
                    try
                    {
                        _fileService.DeleteFile(file.FilePath);

                        var deleted = await _storageDataService.RemoveFileFromStorageAsync(id);

                        if (!deleted)
                        {
                            result.Acknowledged = false;
                            result.Error = new DeleteErrorModel
                            {
                                FileId = id,
                                ErrorMessage = ErrorMessages.RemovingItemFromStorageErrorMessage(id.ToString())
                            };
                        }
                    }                    
                    catch (LocalStorageException ex)
                    {
                        result.Acknowledged = false;
                        result.Error = new DeleteErrorModel
                        {
                            FileId = id,
                            ErrorMessage = ex.UserFriendlyMessage
                        };
                    }
                    catch (Exception ex)
                    {
                        result.Acknowledged = false;
                        result.Error = new DeleteErrorModel
                        {
                            FileId = id,
                            ErrorMessage = ErrorMessages.UNEXPECTED_ERROR_WHILE_FILE_REMOVE_MESSAGE
                        };
                    }
                }
                else
                {
                    result.Acknowledged = false;
                    result.Error = new DeleteErrorModel
                    {
                        FileId = id,
                        ErrorMessage = ErrorMessages.FILE_NOT_FOUND_ERROR_MESSAGE
                    };
                }

                if (!result.Acknowledged)
                {
                    _logger.Information($"File deleted with errors.");
                }
                else
                {
                    _logger.Information($"File deleted successfully.");
                }                

                return result;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (NotFoundException ex)
            {
                _logger.Error(ex, ex.Message);
                throw new NotFoundException(id.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE, ex);
            }
        }

        public async Task<DeleteFilesModel> RemoveFilesAsync(List<Guid> ids)
        {
            try
            {
                _logger.Information($"Try to remove files. Files count: {ids.Count}");

                var result = new DeleteFilesModel();

                if (ids.Any())
                {
                    var files = await _storageDataService.GetFilesInfoAsync<FileInfoModel>(ids);

                    if (files.Any())
                    {
                        var deletionResult = _fileService.DeleteFiles(
                                files.Select(path => path.FilePath).ToList());

                        var deleteFilesIds = new List<Guid>(ids);

                        if (deletionResult.Errors != null
                                && deletionResult.Errors.Any())
                        {
                            var errorFilesPaths = deletionResult.Errors
                                                    .Select(error => error.FilePath).ToList();
                            /*Берем файлы которые удалось удалить*/
                            deleteFilesIds = files.Where(f => 
                                                !errorFilesPaths.Contains(f.FilePath))
                                                    .Select(g => g.Id).ToList();

                            result.Errors = deletionResult.Errors
                                                    .Select(error => 
                                                        new DeleteErrorModel() 
                                                        { 
                                                            ErrorMessage = error.ErrorMessage,
                                                            FileId = files.FirstOrDefault(f => f.FilePath.Equals(error.FilePath))?.Id ?? Guid.Empty 
                                                        }).ToList();                            
                        }

                        await _storageDataService.RemoveFilesFromStorageAsync(deleteFilesIds);
                    }
                }

                return result;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_FILES_MESSAGE, ex);
            }
        }

        public void Dispose()
        {
            foreach (var processor in _annotationsFormatsProcessor)
            {
                processor.Value.Dispose();
            }
        }
    }
}
