﻿using Moq;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Tests.Common;

namespace Storage.Tests.VideoFilesServiceTests
{
    [Collection("TestServicesCollection")]
    public class CutVideoFilesTests
    {
        private TestServicesFixture _fixture;

        public CutVideoFilesTests(TestServicesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CutVideoFileToFrames_Success()
        {
            var videoService = _fixture.VideoFilesService;
            var fileId = Guid.NewGuid();

            var fileInfo = new ExtendedFileInfoModel
            {
                FilePath = "video.mp4",
                Id = fileId,
                MimeType = "video/mp4",
                SystemName = "video.mp4"
            };

            _fixture.StorageDataServiceMock.Setup(s => s
                .GetFileInfoAsync<ExtendedFileInfoModel>(fileId)).ReturnsAsync(fileInfo);

            var videoFilePath = Path.Combine(_fixture.PathToTestFiles, "video.mp4");

            _fixture.FileServiceMock.Setup(s => s
                .GetFileAbsolutePath(It.IsAny<string>()))
                .Returns(videoFilePath);

            var expectedResult = new UploadedFileModel
            {
                FullPath = videoFilePath,
                RelativePath = videoFilePath
            };

            _fixture.FileServiceMock.Setup(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var result = await videoService.SplitIntoFramesAsync(fileId, 0, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedResult.RelativePath, result);

            _fixture.StorageDataServiceMock.Verify(s => s
               .GetFileInfoAsync<ExtendedFileInfoModel>(fileId), Times.Once);
            _fixture.FileServiceMock.Verify(s => s
                .GetFileAbsolutePath(It.IsAny<string>()), Times.Once);
            _fixture.FileServiceMock.Verify(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CutVideoFileToFrames_WithSteps_Success()
        {
            var videoService = _fixture.VideoFilesService;
            var fileId = Guid.NewGuid();

            var fileInfo = new ExtendedFileInfoModel
            {
                FilePath = "video.mp4",
                Id = fileId,
                MimeType = "video/mp4",
                SystemName = "video.mp4"
            };

            _fixture.StorageDataServiceMock.Setup(s => s
                .GetFileInfoAsync<ExtendedFileInfoModel>(fileId)).ReturnsAsync(fileInfo);

            var videoFilePath = Path.Combine(_fixture.PathToTestFiles, "video.mp4");

            _fixture.FileServiceMock.Setup(s => s
                .GetFileAbsolutePath(It.IsAny<string>()))
                .Returns(videoFilePath);

            var expectedResult = new UploadedFileModel
            {
                FullPath = videoFilePath,
                RelativePath = videoFilePath
            };

            _fixture.FileServiceMock.Setup(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None))
                .ReturnsAsync(expectedResult);

            var result = await videoService.SplitIntoFramesAsync(fileId, 10, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedResult.RelativePath, result);

            _fixture.StorageDataServiceMock.Verify(s => s
               .GetFileInfoAsync<ExtendedFileInfoModel>(fileId), Times.Once);
            _fixture.FileServiceMock.Verify(s => s
                .GetFileAbsolutePath(It.IsAny<string>()), Times.Once);
            _fixture.FileServiceMock.Verify(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task CutVideoFileToFrames_IfEmptyId_Error()
        {
            var videoService = _fixture.VideoFilesService;

            var error = await Assert.ThrowsAsync<VideoFilesServiceException>( async () => 
                        await videoService.SplitIntoFramesAsync(Guid.Empty, 10, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("Video id"), error.UserFriendlyMessage);

            _fixture.StorageDataServiceMock.Verify(s => s
               .GetFileInfoAsync<ExtendedFileInfoModel>(It.IsAny<Guid>()), Times.Never);
            _fixture.FileServiceMock.Verify(s => s
                .GetFileAbsolutePath(It.IsAny<string>()), Times.Never);
            _fixture.FileServiceMock.Verify(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task CutVideoFileToFrames_IfFileInfoNotFound_Error()
        {
            var videoService = _fixture.VideoFilesService;
            var fileId = Guid.NewGuid();

            var error = await Assert.ThrowsAsync<VideoFilesServiceException>(async () =>
                        await videoService.SplitIntoFramesAsync(fileId, 10, CancellationToken.None));

            Assert.Equal(ErrorMessages.ItemNotFoundErrorMessage(fileId.ToString()), error.UserFriendlyMessage);

            _fixture.StorageDataServiceMock.Verify(s => s
               .GetFileInfoAsync<ExtendedFileInfoModel>(fileId), Times.Once);
            _fixture.FileServiceMock.Verify(s => s
                .GetFileAbsolutePath(It.IsAny<string>()), Times.Never);
            _fixture.FileServiceMock.Verify(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public async Task CutVideoFileToFrames_IfNotVideoFile_Error()
        {
            var videoService = _fixture.VideoFilesService;
            var fileId = Guid.NewGuid();

            var fileInfo = new ExtendedFileInfoModel
            {
                FilePath = "video.png",
                Id = fileId,
                MimeType = "image/png",
                SystemName = "video.png"
            };

            _fixture.StorageDataServiceMock.Setup(s => s
                .GetFileInfoAsync<ExtendedFileInfoModel>(fileId)).ReturnsAsync(fileInfo);

            var error = await Assert.ThrowsAsync<VideoFilesServiceException>(async () =>
                       await videoService.SplitIntoFramesAsync(fileId, 10, CancellationToken.None));

            Assert.Equal(ErrorMessages.NotVideoFileErrorMessage(fileInfo.MimeType), error.UserFriendlyMessage);

            _fixture.StorageDataServiceMock.Verify(s => s
               .GetFileInfoAsync<ExtendedFileInfoModel>(fileId), Times.Once);
            _fixture.FileServiceMock.Verify(s => s
                .GetFileAbsolutePath(It.IsAny<string>()), Times.Never);
            _fixture.FileServiceMock.Verify(s => s
                .UploadTemporaryFileAsync(It.IsAny<FileStream>(), CancellationToken.None), Times.Never);
        }
    }
}
