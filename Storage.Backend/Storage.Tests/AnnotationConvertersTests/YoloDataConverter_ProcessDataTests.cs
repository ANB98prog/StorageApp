using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Application.DataConverters;
using Storage.Tests.Common;
using Storage.Tests.ElasticStorageTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.AnnotationConvertersTests
{
    [Collection("AnnotatedDataConvertersCollection")]
    public class YoloDataConverterTests
    {
        private readonly ConvertersTestsFixture _fixture;

        public YoloDataConverterTests(ConvertersTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ConvertAnnotatedData_One_Class_Success()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "bird_one_class"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var result = await converter.ProcessAnnotatedDataAsync(uploadData);

            Assert.NotNull(result);
            Assert.Single(result);

            var first = result.FirstOrDefault();

            Assert.Equal(uploadData.FirstOrDefault(u => Path.GetExtension(u.OriginalName).Equals(".jpg")).Id, first.Key);
            Assert.Single(first.Value.Classes);

            Assert.Equal("bird", first.Value.Classes[0].ClassName);

            Assert.Single(first.Value.Annotations);

            var annotation = first.Value.Annotations.FirstOrDefault();

            Assert.Equal(0, annotation.ClassIndex);
            Assert.Equal(41, annotation.Bbox.PixelsAnnotation.X1);
            Assert.Equal(93, annotation.Bbox.PixelsAnnotation.X2);
            Assert.Equal(5, annotation.Bbox.PixelsAnnotation.Y1);
            Assert.Equal(48, annotation.Bbox.PixelsAnnotation.Y2);

        }

        [Fact]
        public async Task ConvertAnnotatedData_Three_Classes_Success()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "bird_three_classes"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var result = await converter.ProcessAnnotatedDataAsync(uploadData);

            Assert.NotNull(result);
            Assert.Single(result);

            var first = result.FirstOrDefault();

            Assert.Equal(uploadData.FirstOrDefault(u => Path.GetExtension(u.OriginalName).Equals(".jpg")).Id, first.Key);
            Assert.Equal(3, first.Value.Classes.Count);

            Assert.Equal(3, first.Value.Annotations.Count());

            var annotation_1 = first.Value.Annotations[0];

            Assert.Equal(2, annotation_1.ClassIndex);
            Assert.Equal(41, annotation_1.Bbox.PixelsAnnotation.X1);
            Assert.Equal(93, annotation_1.Bbox.PixelsAnnotation.X2);
            Assert.Equal(5, annotation_1.Bbox.PixelsAnnotation.Y1);
            Assert.Equal(48, annotation_1.Bbox.PixelsAnnotation.Y2);

            var annotation_2 = first.Value.Annotations[1];
            Assert.Equal(0, annotation_2.ClassIndex);
            Assert.Equal(128, annotation_2.Bbox.PixelsAnnotation.X1);
            Assert.Equal(227, annotation_2.Bbox.PixelsAnnotation.X2);
            Assert.Equal(108, annotation_2.Bbox.PixelsAnnotation.Y1);
            Assert.Equal(209, annotation_2.Bbox.PixelsAnnotation.Y2);

            var annotation_3 = first.Value.Annotations[2];
            Assert.Equal(1, annotation_3.ClassIndex);
            Assert.Equal(84, annotation_3.Bbox.PixelsAnnotation.X1);
            Assert.Equal(255, annotation_3.Bbox.PixelsAnnotation.X2);
            Assert.Equal(166, annotation_3.Bbox.PixelsAnnotation.Y1);
            Assert.Equal(307, annotation_3.Bbox.PixelsAnnotation.Y2);

        }

        [Fact]
        public async Task ConvertAnnotatedData_Two_Images_Success()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "two_images"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var result = await converter.ProcessAnnotatedDataAsync(uploadData);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            var first = result.FirstOrDefault();
            Assert.Equal(3, first.Value.Classes.Count);
            Assert.Equal(3, first.Value.Annotations.Count);

            var second = result.LastOrDefault();
            Assert.Equal(3, second.Value.Classes.Count);
            Assert.Single(second.Value.Annotations);
        }

        [Fact]
        public async Task ConvertAnnotatedData_Null_Files_Success()
        {
            var converter = _fixture.GetLabelMGConverter();
            var result = await converter.ProcessAnnotatedDataAsync(null);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ConvertAnnotatedData_Empty_Files_Success()
        {
            var converter = _fixture.GetLabelMGConverter();
            var result = await converter.ProcessAnnotatedDataAsync(new List<UploadFileRequestModel>());

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ConvertAnnotatedData_No_Classes_Error()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "no_classes"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var errorResult = await Assert.ThrowsAsync<AnnotationConvertionException>(async () => await converter.ProcessAnnotatedDataAsync(uploadData));

            Assert.Equal(ConvertersErrorMessages.CLASSES_FILE_NOT_FOUND_ERROR_MESSAGE, errorResult.UserFriendlyMessage);
        }

        [Fact]
        public async Task ConvertAnnotatedData_No_Classes_In_File_Success()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "no_classes_in_file"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var result = await converter.ProcessAnnotatedDataAsync(uploadData);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ConvertAnnotatedData_No_Images_Success()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "no_images"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var result = await converter.ProcessAnnotatedDataAsync(uploadData);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ConvertAnnotatedData_No_Annotation_file_Success()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "no_annotation_files"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var result = await converter.ProcessAnnotatedDataAsync(uploadData);

            Assert.Empty(result);
        }

        [Fact]
        public async Task ConvertAnnotatedData_Invalid_Annotation_Coordinate_Error()
        {
            var files = Directory.GetFiles(Path.Combine(_fixture.PathToTestFiles, "invalid_coords"));

            var uploadData = new List<UploadFileRequestModel>();

            foreach (var file in files)
            {
                var stream = File.OpenRead(file);

                uploadData.Add(new UploadFileRequestModel
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    OriginalName = Path.GetFileName(file),
                    Stream = stream
                });
            }

            var converter = _fixture.GetLabelMGConverter();

            var errorResult = await Assert.ThrowsAsync<AnnotationConvertionException>(async () => await converter.ProcessAnnotatedDataAsync(uploadData));

            Assert.Equal(ConvertersErrorMessages.ANNOTATION_COORDINATES_COVERTION_ERROR_MESSAGE, errorResult.UserFriendlyMessage);
        }
    }

}
