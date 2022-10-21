using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.DataConverters;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Common.Services
{
    public partial class FileHandlerService
    {
        /// <summary>
        /// Initializes annotations processors
        /// </summary>
        /// <returns></returns>
        private Dictionary<AnnotationFormats, IAnnotatedDataProcessor> InitializeAnnotationsProcessors()
        {
            return new Dictionary<AnnotationFormats, IAnnotatedDataProcessor>
            {
                { AnnotationFormats.yolo, new YoloAnnotationConverter(_logger, TEMP_DIR)}
            };
        }

        /// <summary>
        /// Uploads archive with annotated files
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="annotationFormat">Annotation format</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded files ids</returns>
        /// <exception cref="FileHandlerServiceException"></exception>
        public async Task<List<Guid>> UploadAnnotatedFileAsync(UploadFileRequestModel file, AnnotationFormats annotationFormat, CancellationToken cancellationToken)
        {
            try
            {
                var fileName = (file != null
                                && !string.IsNullOrEmpty(file.OriginalName)) ?
                                    $"File name '{file.OriginalName}'"
                                        : string.Empty;

                _logger.Information($"Try to upload archive file with annotated files. {fileName}");

                if (file == null)
                    throw new ArgumentNullException(nameof(file));

                if (!_annotationsFormatsProcessor.TryGetValue(annotationFormat, out var processor))
                {
                    throw new FileHandlerServiceException(ErrorMessages.UNSUPORTED_ANNOTATION_FORMAT_ERROR_MESSAGE);
                }

                var files = await UploadArchiveFilesAsync(file, Constants.ANNOTATION_MIMETYPES, cancellationToken);

                var annotations = await processor.ProcessAnnotatedDataAsync(files);

                if (annotations != null
                    && annotations.Any())
                {
                    foreach (var annotation in annotations)
                    {
                        var image = files.FirstOrDefault(f => f.Id.Equals(annotation.Key));

                        if (image != null)
                        {
                            /*
                             * Mark as annotated
                             */
                            image.IsAnnotated = true;
                            image.Annotation = annotation.Value;
                        }
                    }
                }

                /*
                 * Upload only images
                 */
                var filesIds = await UploadManyFileAsync(
                                        files.Where(im => Constants.IMAGES_MIMETYPES.Contains(im.MimeType)).ToList(),
                                            cancellationToken);

                files.ForEach(async f => await f.Stream.DisposeAsync());

                _logger.Information($"Archive with annotated files were successfully uploaded. Files count: {filesIds.Count}");

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
            catch (AnnotationConvertionException ex)
            {
                _logger.Error(ex, ex.UserFriendlyMessage);
                throw new FileHandlerServiceException(ex.UserFriendlyMessage, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE, ex);
            }
            finally
            {
                await file.Stream.DisposeAsync();
            }
        }

        /// <summary>
        /// Uploads archive with annotated files
        /// </summary>
        /// <param name="file">File to upload</param>
        /// <param name="annotationFormat">Annotation format</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Uploaded files ids</returns>
        /// <exception cref="FileHandlerServiceException"></exception>
        //public async Task<FileStream> DownloadAnnotatedFileAsync(List<Guid> filesIds, AnnotationFormats annotationFormat, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        _logger.Information($"Try to download annotated files. Files count: {filesIds}");

        //        if (filesIds.Any())
        //        {
        //            if (!_annotationsFormatsProcessor.TryGetValue(annotationFormat, out var processor))
        //            {
        //                throw new FileHandlerServiceException(ErrorMessages.UNSUPORTED_ANNOTATION_FORMAT_ERROR_MESSAGE);
        //            }

        //            var annotatedFilesInfos = await _storageDataService.GetFilesInfoAsync<AnnotationFileInfo>(filesIds);

        //            if (annotatedFilesInfos != null
        //                && annotatedFilesInfos.Any())
        //            {
        //                /*
        //                 * Необходимо разделить по группам т.к. набор классов может отличаться
        //                 */
        //            }

        //        }
        //    }
        //    catch (ArgumentNullException ex)
        //    {
        //        _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
        //        throw new FileHandlerServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        _logger.Error(ex, ErrorMessages.FileNotFoundErrorMessage(ex.FileName));
        //        throw new FileHandlerServiceException(ErrorMessages.FileNotFoundErrorMessage(ex.FileName), ex);
        //    }
        //    catch (AnnotationConvertionException ex)
        //    {
        //        _logger.Error(ex, ex.UserFriendlyMessage);
        //        throw new FileHandlerServiceException(ex.UserFriendlyMessage, ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE);
        //        throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE, ex);
        //    }
        //    finally
        //    {
        //    }
        //}
    }
}
