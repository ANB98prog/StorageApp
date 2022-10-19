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
                { AnnotationFormats.labelMG, new LabelMGConverter(_logger)}
            };
        }

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

                var files = await UploadArchiveFilesAsync(file, Constants.ANNOTATION_MIMETYPES, cancellationToken);

                /*
                 Process annotated files
                 */
                if(_annotationsFormatsProcessor.TryGetValue(annotationFormat, out var processor))
                {
                    var annotations = await processor.ProcessAnnotatedDataAsync(files);

                    if (annotations != null
                        && annotations.Any())
                    {
                        foreach (var annotation in annotations)
                        {
                            var image = files.FirstOrDefault(f => f.Id.Equals(annotation.Key));

                            if(image != null)
                            {
                                image.Annotation = annotation.Value;
                            }
                        }
                    }
                }
                else
                {
                    throw new FileHandlerServiceException(ErrorMessages.UNSUPORTED_ANNOTATION_FORMAT_ERROR_MESSAGE);
                }

                /*
                 * Upload only images
                 */
                var filesIds = await UploadManyFileAsync(
                                        files.Where(im => Constants.IMAGES_MIMETYPES.Contains(im.MimeType)).ToList(),
                                            cancellationToken);

                files.ForEach(f => f.Stream.Dispose());

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
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE);
                throw new FileHandlerServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE, ex);
            }
            finally
            {

            }
        }
    }
}
