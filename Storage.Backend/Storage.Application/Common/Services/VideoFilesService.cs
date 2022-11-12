using OpenCvSharp;
using Serilog;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Storage.Application.Common.Services
{
    public class VideoFilesService : IVideoFilesService
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Files service
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// Metadata storage service
        /// </summary>
        private readonly IStorageDataService _storageDataService;

        /// <summary>
        /// Directory for temporary files
        /// </summary>
        private readonly string _tempDir;

        /// <summary>
        /// Initializes class instance of <see cref="VideoFilesService"/>
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="fileService">File service</param>
        /// <param name="storageDataService">Elastic storage service</param>
        public VideoFilesService(string tempDir, ILogger logger, IFileService fileService, IStorageDataService storageDataService, string ffmpegExecutablePath)
        {
            _logger = logger;
            _fileService = fileService;
            _storageDataService = storageDataService;

            if(string.IsNullOrWhiteSpace(ffmpegExecutablePath))
            {
                throw new ArgumentNullException(nameof(ffmpegExecutablePath));
            }    
            else if(!Directory.Exists(ffmpegExecutablePath))
            {
                throw new ArgumentException("FFmpeg executables path are not exists!");
            }

            // Необходимо указать путь к исполняемым файлам ffmpeg
            FFmpeg.SetExecutablesPath(ffmpegExecutablePath);

            _tempDir = tempDir;

            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
        }

        public async Task<string> SplitIntoFramesAsync(Guid videoFileId, int step = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.Information($"Try to split video into frames. Frames step: {step}");

                UploadedFileModel cuttedFilesArchivePath;

                if (videoFileId.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException("Video id");
                }

                if(step < 0)
                {
                    throw new VideoFilesServiceException(ErrorMessages.FRAMES_STEP_LESS_THAN_ZERO_ERROR_MESSAGE);
                }
                else if(step == 0)
                {
                    step = 1;
                }

                var fileInfo = await _storageDataService.GetFileInfoAsync<ExtendedFileInfoModel>(videoFileId);

                if(fileInfo == null)
                {
                    throw new VideoFilesServiceException(ErrorMessages.ItemNotFoundErrorMessage(videoFileId.ToString()));
                }

                /*Проверяем видео ли это?*/
                if (!fileInfo.MimeType.StartsWith("video"))
                {
                    throw new VideoFilesServiceException(ErrorMessages.NotVideoFileErrorMessage(fileInfo.MimeType));
                }

                /*Необходимо получить путь к файлу*/
                var filePath = _fileService.GetFileAbsolutePath(fileInfo.FilePath);

                var cutted = await CutVideoAsync(filePath, step);
                var cuttedArchive = Directory.GetParent(cutted).FullName;

                try
                {
                    using (var archive = FileHelper.ArchiveFolderStream(cutted))
                    {
                        cuttedArchive = Path.Combine(cuttedArchive, archive.Name);
                        cuttedFilesArchivePath = await _fileService.UploadTemporaryFileAsync(archive, cancellationToken);
                    }
                }
                finally
                {
                    try
                    {
                        /*Необходимо удалить файлы*/
                        FileHelper.RemoveFile(cuttedArchive);
                        FileHelper.RemoveDirectory(cutted);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_FILE_REMOVE_MESSAGE);
                    }
                }                

                _logger.Information($"Video file are successfully splited into frames.");

                return cuttedFilesArchivePath?.RelativePath ?? "";
               
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new UserException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (VideoFilesServiceException ex)
            {
                _logger.Error(ex.UserFriendlyMessage);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_VIDEO_SPLITTING);
                throw new VideoFilesServiceException(ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_VIDEO_SPLITTING, ex);
            }
        }

        /// <summary>
        /// Cuts video to frames
        /// </summary>
        /// <param name="filePath">Video file to cut</param>
        /// <param name="step">Frame step</param>
        /// <returns>Path to frames</returns>
        /// <exception cref="VideoFilesServiceException"></exception>
        private async Task<string> CutVideoAsync(string filePath, int step)
        {
            try
            {
                var pathToSave = Path.Combine(_tempDir, Path.GetRandomFileName().Replace(".", string.Empty));

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                Func<string, string> outputFileNameBuilder = (number) => { return $"\"{Path.Combine(pathToSave, $"file_{number}.png")}\""; };
                IMediaInfo info = await FFmpeg.GetMediaInfo(filePath);
                IVideoStream? videoStream = info.VideoStreams.First()?.SetCodec(VideoCodec.png);

                var splitQuery = FFmpeg.Conversions.New()
                                    .AddStream(videoStream)
                                        .ExtractEveryNthFrame(step, outputFileNameBuilder)
                                            .Build();

                await FFmpeg.Conversions.New().Start(splitQuery);

                return pathToSave;
            }
            catch (Exception ex)
            {
                _logger.Error(ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_VIDEO_SPLITTING, ex);
                throw new VideoFilesServiceException(ErrorMessages.UNEXPECTED_ERROR_OCCURED_WHILE_VIDEO_SPLITTING, ex);
            }
        }
    }
}
