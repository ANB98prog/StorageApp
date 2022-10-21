using Moq;
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
    public class UploadAnnotatedDataTests : TestServicesFixture
    {
        [Fact]
        public async Task UploadAnnotatedDataInLabelMGFormat_Success()
        {
            var fileName = "bird_one_class.rar";
            var annotatedFile = Path.Combine(PathToTestFiles, fileName);

            var file = new UploadFileRequestModel
            {
                Attributes = new List<string> { "annotated" },
                Id = Guid.NewGuid(),
                MimeType = "image/jpeg",
                OriginalName = fileName,
                SystemName = fileName,
                Stream = File.OpenRead(annotatedFile),
            };

            StorageDataServiceMock.Setup(s => s
                .AddDataToStorageAsync<BaseFile>(It.IsAny<BaseFile>()))
                    .ReturnsAsync(Guid.NewGuid());

            var result = await FileHandlerService.UploadAnnotatedFileAsync(file, Domain.AnnotationFormats.yolo, CancellationToken.None);

            Assert.NotNull(result);

            StorageDataServiceMock.Verify(s => s
                .AddDataToStorageAsync<BaseFile>(It.IsAny<BaseFile>()), Times.Once);
        }

        [Fact]
        public async Task UploadAnnotatedDataInLabelMGFormat_Two_Images_Success()
        {
            var fileName = "two_images.rar";
            var annotatedFile = Path.Combine(PathToTestFiles, fileName);

            var file = new UploadFileRequestModel
            {
                Attributes = new List<string> { "annotated" },
                Id = Guid.NewGuid(),
                MimeType = "image/jpeg",
                OriginalName = fileName,
                SystemName = fileName,
                Stream = File.OpenRead(annotatedFile),
            };

            StorageDataServiceMock.Setup(s => s
                .AddDataToStorageAsync<BaseFile>(It.IsAny<BaseFile>()))
                    .ReturnsAsync(Guid.NewGuid());

            var result = await FileHandlerService.UploadAnnotatedFileAsync(file, Domain.AnnotationFormats.yolo, CancellationToken.None);

            Assert.NotNull(result);

            StorageDataServiceMock.Verify(s => s
                .AddDataToStorageAsync<BaseFile>(It.IsAny<BaseFile>()), Times.Exactly(2));
        }
    }
}
