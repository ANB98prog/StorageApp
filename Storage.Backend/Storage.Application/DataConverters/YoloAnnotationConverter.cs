using ImageMagick;
using Serilog;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.DataConverters
{
    public class YoloAnnotationConverter : IAnnotatedDataProcessor
    {
        private ILogger _logger;

        private readonly string _tempDir;

        private readonly string[] ImagesExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".gif", ".tif", ".tiff", ".svg"
        };

        public AnnotationFormats AnnotationFormat => AnnotationFormats.yolo;

        public YoloAnnotationConverter(ILogger logger, string tempDir)
        {
            _logger = logger;
            _tempDir = Path.Combine(tempDir, "annotation");

            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);
        }

        /// <summary>
        /// Processes annotated data
        /// </summary>
        /// <param name="files">Files data</param>
        /// <returns>Processed data</returns>
        /// <exception cref="AnnotationConvertionException"></exception>
        public async Task<Dictionary<Guid, AnnotationMetadata>> ProcessAnnotatedDataAsync(List<UploadFileRequestModel> files)
        {
            try
            {
                var annotation = new Dictionary<Guid, AnnotationMetadata>();

                if (files != null
                    && files.Any())
                {
                    /*Получаем классы*/
                    var classesFile = files.FirstOrDefault(f => f.OriginalName.ToLower().Equals(ConvertersConstants.CLASSES_FILE_NAME));

                    if (classesFile == null)
                    {
                        throw new AnnotationConvertionException(ConvertersErrorMessages.CLASSES_FILE_NOT_FOUND_ERROR_MESSAGE);
                    }

                    var classes = await GetClassesFromFileAsync(classesFile.Stream);

                    if (classes.Any())
                    {
                        var imagesFiles = files.Where(f =>
                                               ImagesExtensions.Contains(Path.GetExtension(f.OriginalName)));

                        foreach (var image in imagesFiles)
                        {
                            var bountingBoxesFiles = files.FirstOrDefault(b =>
                                                    b.OriginalName.Equals($"{Path.GetFileNameWithoutExtension(image.OriginalName)}.txt"));

                            if (bountingBoxesFiles == null)
                                continue;

                            var bboxes = await GetBoundingBoxesFromFileAsync(bountingBoxesFiles.Stream);

                            var imageInfo = GetImageInfo(image.Stream);

                            var annotations = GetAnnotations(imageInfo, bboxes);

                            annotation.Add(image.Id, new AnnotationMetadata
                            {
                                Classes = classes,
                                Annotations = annotations
                            });
                        }
                    }
                }

                return annotation;
            }
            catch (AnnotationConvertionException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new AnnotationConvertionException(ConvertersErrorMessages.UNEXPECTED_ERROR_WHILE_CONVERT_ANNOTATION_DATA, ex);
            }
        }

        #region Private methods
        /// <summary>
        /// Gets classes from file
        /// </summary>
        /// <param name="classesStream">File stream</param>
        /// <returns>Annotation classes</returns>
        /// <exception cref="AnnotationConvertionException"></exception>
        private async Task<List<AnnotatedClass>> GetClassesFromFileAsync(Stream classesStream)
        {
            try
            {
                var classes = new List<AnnotatedClass>();

                _logger.Information("Try to get annotation classes.");

                var bytes = new byte[classesStream.Length];

                await classesStream.ReadAsync(bytes);

                var classesFromFile = Encoding.UTF8.GetString(bytes)
                    .Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < classesFromFile.Length; i++)
                {
                    classes.Add(new AnnotatedClass(i, classesFromFile[i]));
                }

                _logger.Information($"Annotation classes were got. Classes count: '{classes.Count}'");

                return classes;
            }
            catch (AnnotationConvertionException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new AnnotationConvertionException(ConvertersErrorMessages.UNEXPECTED_ERROR_WHILE_GET_CLASSES, ex);
            }
        }

        /// <summary>
        /// Gets classes and bboxes from file
        /// </summary>
        /// <param name="dataStream">Annotation data file stream</param>
        /// <returns>Annotation metadata</returns>
        /// <exception cref="AnnotationConvertionException"></exception>
        private async Task<string[]> GetBoundingBoxesFromFileAsync(Stream dataStream)
        {
            try
            {
                _logger.Information("Try to get bounding boxes.");

                var bytes = new byte[dataStream.Length];

                await dataStream.ReadAsync(bytes);

                var bboxes = Encoding.UTF8.GetString(bytes)
                    .Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

                _logger.Information($"Bounding boxes were got. Bboxes count: '{bboxes.Length}'");

                return bboxes;
            }
            catch (AnnotationConvertionException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new AnnotationConvertionException(ConvertersErrorMessages.UNEXPECTED_ERROR_WHILE_GET_BBOXES, ex);
            }
        }

        /// <summary>
        /// Gets image file info
        /// </summary>
        /// <param name="imageStream">Image file stream</param>
        /// <returns>Image file info</returns>
        private AnnotationImageInfo GetImageInfo(Stream imageStream)
        {
            var copy = new MemoryStream();
            imageStream.CopyTo(copy);
            imageStream.Position = 0;
            copy.Position = 0;
            var imageFile = new MagickImageInfo(copy);

            return new AnnotationImageInfo(imageFile.Width, imageFile.Height);
        }

        /// <summary>
        /// Gets image annotation data
        /// </summary>
        /// <param name="imageInfo">Image info</param>
        /// <param name="bboxes">Bounting boxes</param>
        /// <returns>Annotation data</returns>
        /// <exception cref="AnnotationConvertionException"></exception>
        private List<Annotation> GetAnnotations(AnnotationImageInfo imageInfo, string[] bboxes)
        {
            var annotations = new List<Annotation>();

            foreach (var bbox in bboxes)
            {
                var splited = bbox.Split(" ");

                /*
                 * 'class index' 'X' 'Y' 'W' 'H'
                 */
                if (splited.Length == 5)
                {
                    try
                    {
                        int classIndex = int.Parse(splited[0]);
                        float x = float.Parse(splited[1], CultureInfo.InvariantCulture);
                        float y = float.Parse(splited[2], CultureInfo.InvariantCulture);
                        float w = float.Parse(splited[3], CultureInfo.InvariantCulture);
                        float h = float.Parse(splited[4], CultureInfo.InvariantCulture);

                        annotations.Add(new Annotation()
                        {
                            ClassIndex = classIndex,
                            Bbox = new BoudingBox(imageInfo, new RelativeAnnotationBbox(x, y, w, h))
                        });
                    }
                    catch (Exception ex)
                    {
                        throw new AnnotationConvertionException(ConvertersErrorMessages.ANNOTATION_COORDINATES_COVERTION_ERROR_MESSAGE, ex);
                    }
                }
            }

            return annotations;
        }
        #endregion

        /// <summary>
        /// Converts annotated data
        /// </summary>
        /// <param name="annotationInfo">Annotation info</param>
        /// <param name="groupName">Annotation files group</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Path to directory with annotated files info</returns>
        /// <remarks>
        /// groupName is needed if annotated files are from different data sets and has different classes
        /// </remarks>
        /// <exception cref="AnnotationConvertionException"></exception>
        public async Task<string?> ConvertAnnotatedDataAsync(List<AnnotationFileInfo> annotationInfo, string groupName, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information($"Try to convert annotation data to {AnnotationFormat} format.");

                string annotationPath = null;

                if (annotationInfo != null
                    && annotationInfo.Any())
                {
                    annotationPath = Path.Combine(_tempDir, groupName);

                    if (!Directory.Exists(annotationPath))
                        Directory.CreateDirectory(annotationPath);

                    var classes = annotationInfo
                                    .FirstOrDefault()
                                        .Annotation.Classes
                                            .Select(c => c.ClassName);

                    if (classes != null 
                        && classes.Any())
                    {
                        await FileHelper.SaveFileAsync(string.Join(Environment.NewLine, classes), Path.Combine(annotationPath, ConvertersConstants.CLASSES_FILE_NAME), cancellationToken);

                        foreach (var item in annotationInfo)
                        {
                            if (item.Annotation.Annotations.Any())
                            {
                                var annotationText = new StringBuilder();
                                foreach (var a in item.Annotation.Annotations)
                                {
                                    annotationText.AppendLine($"{a.ClassIndex} {a.Bbox.RelativeAnnotation}");
                                }

                                await FileHelper.SaveFileAsync(
                                        annotationText.ToString(),
                                            Path.Combine(annotationPath,
                                                $"{Path.GetFileNameWithoutExtension(item.Name)}.txt"),
                                                    cancellationToken);
                            }
                            else
                            {
                                _logger.Warning($"Annotation data for {item.Name} with id {item.Id} are not found.");
                            }
                        } 
                    }

                    _logger.Information("Data successfully converted.");

                }
                else
                {
                    _logger.Warning("Classes are not found. Cannot convert.");
                }

                return annotationPath;
            }
            catch (AnnotationConvertionException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new AnnotationConvertionException(ConvertersErrorMessages.UNEXPECTED_ERROR_WHILE_CONVERT_ANNOTATION_DATA, ex);
            }
        }

        public void Dispose()
        {
        }
    }
}
