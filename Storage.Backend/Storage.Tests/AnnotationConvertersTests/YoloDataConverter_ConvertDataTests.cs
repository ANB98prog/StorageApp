using Storage.Application.Common.Models;
using Storage.Application.DataConverters;

namespace Storage.Tests.AnnotationConvertersTests
{
    [Collection("AnnotatedDataConvertersCollection")]
    public class YoloDataConverter_ConvertDataTests
    {
        private readonly ConvertersTestsFixture _fixture;

        public YoloDataConverter_ConvertDataTests(ConvertersTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ConvertData_One_Image_Success()
        {
            var imageFileName = "first.jpg";

            var annotationDataInfo = new List<AnnotationFileInfo>()
            {
                new AnnotationFileInfo()
                {
                    Id = Guid.NewGuid(),
                    FilePath = "some path",
                    SystemName = imageFileName,
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "first"),
                            new Domain.AnnotatedClass(1, "second"),
                            new Domain.AnnotatedClass(2, "third"),
                        },
                        Annotations = new List<Domain.Annotation>
                        {
                            new Domain.Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 2,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            }
                        }
                    }
                }
            };

            var converter = _fixture.GetLabelMGConverter();

            var groupName = "succes_1_2";
            var result = await converter.ConvertAnnotatedDataAsync(annotationDataInfo, groupName, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.EndsWith(groupName));

            var files = Directory.GetFiles(result);

            Assert.Equal(2, files.Length);

            Assert.Contains(files, f => f.EndsWith(ConvertersConstants.CLASSES_FILE_NAME));
            Assert.Contains(files, f => f.EndsWith($"{Path.GetFileNameWithoutExtension(imageFileName)}.txt"));

            var classes = File.ReadAllLines(files.FirstOrDefault(f => f.EndsWith(ConvertersConstants.CLASSES_FILE_NAME)));
            Assert.Equal(3, classes.Length);

            var bboxes = File.ReadAllLines(files.FirstOrDefault(f => f.EndsWith($"{Path.GetFileNameWithoutExtension(imageFileName)}.txt")));
            Assert.Equal(3, bboxes.Length);
        }

        [Fact]
        public async Task ConvertData_Two_Image_Success()
        {
            var firstImage = "first.jpg";
            var secondImage = "second.jpg";

            var annotationDataInfo = new List<AnnotationFileInfo>()
            {
                new AnnotationFileInfo()
                {
                    Id = Guid.NewGuid(),
                    FilePath = "some path",
                    SystemName = firstImage,
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "first"),
                            new Domain.AnnotatedClass(1, "second"),
                            new Domain.AnnotatedClass(2, "third"),
                        },
                        Annotations = new List<Domain.Annotation>
                        {
                            new Domain.Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 2,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            }
                        }
                    }
                },
                new AnnotationFileInfo()
                {
                    Id = Guid.NewGuid(),
                    FilePath = "some path",
                    SystemName = secondImage,
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>
                        {
                            new Domain.AnnotatedClass(0, "first"),
                            new Domain.AnnotatedClass(1, "second"),
                            new Domain.AnnotatedClass(2, "third"),
                        },
                        Annotations = new List<Domain.Annotation>
                        {
                            new Domain.Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 2,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            }
                        }
                    }
                }
            };

            var converter = _fixture.GetLabelMGConverter();

            var groupName = "succes_1";
            var result = await converter.ConvertAnnotatedDataAsync(annotationDataInfo, groupName, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.EndsWith(groupName));

            var files = Directory.GetFiles(result);

            Assert.Equal(3, files.Length);

            Assert.Contains(files, f => f.EndsWith(ConvertersConstants.CLASSES_FILE_NAME));
            Assert.Contains(files, f => f.EndsWith($"{Path.GetFileNameWithoutExtension(firstImage)}.txt"));

            var classes = File.ReadAllLines(files.FirstOrDefault(f => f.EndsWith(ConvertersConstants.CLASSES_FILE_NAME)));
            Assert.Equal(3, classes.Length);

            var bboxes_first = File.ReadAllLines(files.FirstOrDefault(f => f.EndsWith($"{Path.GetFileNameWithoutExtension(firstImage)}.txt")));
            Assert.Equal(3, bboxes_first.Length);

            var bboxes_second = File.ReadAllLines(files.FirstOrDefault(f => f.EndsWith($"{Path.GetFileNameWithoutExtension(secondImage)}.txt")));
            Assert.Equal(3, bboxes_second.Length);
        }

        [Fact]
        public async Task ConvertData_If_No_Classes_Success()
        {
            var firstImage = "first.jpg";

            var annotationDataInfo = new List<AnnotationFileInfo>()
            {
                new AnnotationFileInfo()
                {
                    Id = Guid.NewGuid(),
                    FilePath = "some path",
                    SystemName = firstImage,
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>(),
                        Annotations = new List<Domain.Annotation>
                        {
                            new Domain.Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            },
                            new Domain.Annotation
                            {
                                ClassIndex = 2,
                                Bbox = new Domain.BoudingBox(
                                    new Domain.AnnotationImageInfo(100, 100),
                                    new Domain.RelativeAnnotationBbox(0.71f,0.97f,0.017f,0.088f))
                            }
                        }
                    }
                }
            };

            var converter = _fixture.GetLabelMGConverter();

            var groupName = "succes_1_1";
            var result = await converter.ConvertAnnotatedDataAsync(annotationDataInfo, groupName, CancellationToken.None);

            Assert.True(result.EndsWith(groupName));

            var files = Directory.GetFiles(result);

            Assert.Empty(files);

            Assert.DoesNotContain(files, f => f.EndsWith(ConvertersConstants.CLASSES_FILE_NAME));
            Assert.DoesNotContain(files, f => f.EndsWith($"{Path.GetFileNameWithoutExtension(firstImage)}.txt"));
        }

        [Fact]
        public async Task ConvertData_If_Annotation_Info_Are_Empty()
        {
            var firstImage = "first.jpg";

            var annotationDataInfo = new List<AnnotationFileInfo>()
            {
                new AnnotationFileInfo()
                {
                    Id = Guid.NewGuid(),
                    FilePath = "some path",
                    SystemName = firstImage,
                    Annotation = new Domain.AnnotationMetadata
                    {
                        Classes = new List<Domain.AnnotatedClass>()
                        {
                            new Domain.AnnotatedClass(0, "class")
                        },
                        Annotations = new List<Domain.Annotation>()
                    }
                }
            };

            var converter = _fixture.GetLabelMGConverter();

            var groupName = "succes_1";
            var result = await converter.ConvertAnnotatedDataAsync(annotationDataInfo, groupName, CancellationToken.None);

            Assert.True(result.EndsWith(groupName));
        }

        [Fact]
        public async Task ConvertData_If_Annotation_Metadata_Are_Empty()
        {
            var annotationDataInfo = new List<AnnotationFileInfo>();

            var converter = _fixture.GetLabelMGConverter();

            var groupName = "succes_1";
            var result = await converter.ConvertAnnotatedDataAsync(annotationDataInfo, groupName, CancellationToken.None);

            Assert.Null(result);
        }
    }
}
