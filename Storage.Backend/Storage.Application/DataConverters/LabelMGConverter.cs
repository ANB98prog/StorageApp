using ImageMagick;
using Serilog;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Application.DataConverters
{
    public class LabelMGConverter : IAnnotatedDataProcessor
    {
        private ILogger _logger;

        private readonly string[] ImagesExtensions = new string[]
        {
            ".jpg", ".jpeg", ".png", ".gif", ".tif", ".tiff", ".svg"
        };

        public AnnotationFormats AnnotationFormat => AnnotationFormats.labelMG;

        public LabelMGConverter(ILogger logger)
        {
            _logger = logger;
        }

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
                _logger.Error(ex, ex.UserFriendlyMessage);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ConvertersErrorMessages.UNEXPECTED_ERROR_WHILE_CONVERT_ANNOTATION_DATA);
                throw new AnnotationConvertionException(ConvertersErrorMessages.UNEXPECTED_ERROR_WHILE_CONVERT_ANNOTATION_DATA);
            }
        }

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
                    .Split(new string[]{ Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

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
            var imageFile = new MagickImageInfo(imageStream);

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
                if(splited.Length == 5)
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

        public void ConvertAnnotatedData(List<BaseFile> files, AnnotationFormats requiredFormat)
        {
            throw new NotImplementedException();
        }
    }
}
