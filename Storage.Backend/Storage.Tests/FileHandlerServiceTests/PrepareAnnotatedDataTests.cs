using Moq;
using Nest;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Domain;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.FileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class PrepareAnnotatedDataTests : TestServicesFixture
    {


        [Fact]
        public async Task PrepareAnnotatedData_Success()
        {
            var imageName = "test_image.jpg";
            var testImage = Path.Combine(TestConstants.TestFilesDirectory, imageName);

            using (var imageFile = File.Create(testImage))
            {
            }

            var ids = new List<Guid>() { Guid.NewGuid() };

            var filesInfo = new List<AnnotationFileInfo>()
            {
                new AnnotationFileInfo
                {
                    Id = ids.First(),
                    FilePath = testImage,
                    SystemName = imageName,
                    Annotation = new AnnotationMetadata
                    {
                        Classes = new List<AnnotatedClass>
                        {
                            new AnnotatedClass(0, "cat"),
                            new AnnotatedClass(1, "dog")
                        },
                        Annotations = new List<Annotation>
                        {
                            new Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new BoudingBox(new AnnotationImageInfo(100, 100), new PixelsAnnotationBbox(100, 100, 100, 100))
                            },
                            new Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new BoudingBox(new AnnotationImageInfo(100, 100), new PixelsAnnotationBbox(100, 100, 100, 100))
                            }
                        }
                    }
                }
            };

            StorageDataServiceMock.Setup(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(filesInfo);

            var result = await FileHandlerService.PrepareAnnotatedFileAsync(ids, Domain.AnnotationFormats.yolo, CancellationToken.None);

            Assert.NotNull(result);
            ZipArchive.IsZipFile(result);

            var archive = ZipArchive.Open(Path.Combine(TestConstants.StorageDirectory, "temp", result));

            Assert.Equal(3, archive.Entries.Count);

            Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith("classes.txt")));
            Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith(imageName)));
            Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith($"{Path.GetFileNameWithoutExtension(imageName)}.txt")));

            archive.Dispose();
        }

        [Fact]
        public async Task PrepareAnnotatedData_Three_Files_With_Same_Classes_Success()
        {
            var files = new List<string>()
            {
               Path.Combine(TestConstants.TestFilesDirectory,"test_image_1.jpg"),
               Path.Combine(TestConstants.TestFilesDirectory,"test_image_2.jpg"),
               Path.Combine(TestConstants.TestFilesDirectory,"test_image_3.jpg")
            };

            var filesInfo = new List<AnnotationFileInfo>();

            foreach (var file in files)
            {
                using (var imageFile = File.Create(file)) { }

                filesInfo.Add(new AnnotationFileInfo
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    SystemName = file,
                    Annotation = new AnnotationMetadata
                    {
                        Classes = new List<AnnotatedClass>
                        {
                            new AnnotatedClass(0, "cat"),
                            new AnnotatedClass(1, "dog")
                        },
                        Annotations = new List<Annotation>
                        {
                            new Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new BoudingBox(new AnnotationImageInfo(100, 100), new PixelsAnnotationBbox(100, 100, 100, 100))
                            },
                            new Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new BoudingBox(new AnnotationImageInfo(100, 100), new PixelsAnnotationBbox(100, 100, 100, 100))
                            }
                        }
                    }
                });
            }

            StorageDataServiceMock.Setup(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(filesInfo);

            var result = await FileHandlerService.PrepareAnnotatedFileAsync(new List<Guid> { Guid.NewGuid() }, Domain.AnnotationFormats.yolo, CancellationToken.None);

            Assert.NotNull(result);
            ZipArchive.IsZipFile(result);

            var archive = ZipArchive.Open(Path.Combine(TestConstants.StorageDirectory, "temp", result));

            Assert.Equal(7, archive.Entries.Count);

            Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith("classes.txt")));
            foreach (var file in files)
            {
                Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith(Path.GetFileName(file))));
                Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith($"{Path.GetFileNameWithoutExtension(file)}.txt")));
            }

            archive.Dispose();

            StorageDataServiceMock.Verify(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()), Times.Once);
        }

        [Fact]
        public async Task PrepareAnnotatedData_Three_Files_With_Different_Classes_Success()
        {
            var files = new List<string>()
            {
               Path.Combine(TestConstants.TestFilesDirectory,"test_image_1.jpg"),
               Path.Combine(TestConstants.TestFilesDirectory,"test_image_2.jpg"),
               Path.Combine(TestConstants.TestFilesDirectory,"test_image_3.jpg")
            };

            var filesInfo = new List<AnnotationFileInfo>();

            foreach (var file in files)
            {
                using (var imageFile = File.Create(file)) { }

                filesInfo.Add(new AnnotationFileInfo
                {
                    Id = Guid.NewGuid(),
                    FilePath = file,
                    SystemName = file,
                    Annotation = new AnnotationMetadata
                    {
                        Classes = new List<AnnotatedClass>
                        {
                            new AnnotatedClass(0, Path.GetFileName(file)),
                            new AnnotatedClass(1, "dog")
                        },
                        Annotations = new List<Annotation>
                        {
                            new Annotation
                            {
                                ClassIndex = 0,
                                Bbox = new BoudingBox(new AnnotationImageInfo(100, 100), new PixelsAnnotationBbox(100, 100, 100, 100))
                            },
                            new Annotation
                            {
                                ClassIndex = 1,
                                Bbox = new BoudingBox(new AnnotationImageInfo(100, 100), new PixelsAnnotationBbox(100, 100, 100, 100))
                            }
                        }
                    }
                });
            }

            StorageDataServiceMock.Setup(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(filesInfo);

            var result = await FileHandlerService.PrepareAnnotatedFileAsync(new List<Guid> { Guid.NewGuid() }, Domain.AnnotationFormats.yolo, CancellationToken.None);

            Assert.NotNull(result);
            ZipArchive.IsZipFile(result);

            var archive = ZipArchive.Open(Path.Combine(TestConstants.StorageDirectory, "temp", result));

            try
            {
                Assert.Equal(9, archive.Entries.Count);
                Assert.Equal(3, archive.Entries.Count(e => e.Key.EndsWith("classes.txt")));

                foreach (var file in files)
                {
                    Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith(Path.GetFileName(file))));
                    Assert.NotNull(archive.Entries.FirstOrDefault(e => e.Key.EndsWith($"{Path.GetFileNameWithoutExtension(file)}.txt")));
                }
            }
            finally
            {
                archive.Dispose();
            }

            StorageDataServiceMock.Verify(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()), Times.Once);
        }

        [Fact]
        public async Task PrepareAnnotatedData_If_Ids_Empty_Error()
        {
            var ids = new List<Guid>();

            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () => await FileHandlerService.PrepareAnnotatedFileAsync(ids, Domain.AnnotationFormats.yolo, CancellationToken.None));

            Assert.Equal(ErrorMessages.ANNOTATED_FILES_IDS_NOT_SET_ERROR_MESSAGE, error.UserFriendlyMessage);

            StorageDataServiceMock.Verify(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()), Times.Never);
        }

        [Fact]
        public async Task PrepareAnnotatedData_If_Annotation_Info_Null_Error()
        {
            var ids = new List<Guid>() { Guid.NewGuid() };

            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () => await FileHandlerService.PrepareAnnotatedFileAsync(ids, Domain.AnnotationFormats.yolo, CancellationToken.None));

            Assert.Equal(ErrorMessages.ANNOTATED_FILES_INFO_NOT_FOUND_ERROR_MESSAGE, error.UserFriendlyMessage);

            StorageDataServiceMock.Verify(s => s
                .GetFilesInfoAsync<AnnotationFileInfo>(It.IsAny<List<Guid>>()), Times.Once);
        }
    }
}
