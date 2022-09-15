﻿using Storage.Application.Common.Models;
using Storage.Tests.Common;

namespace Storage.Tests.ImagesFileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class DownloadFileTests : TestServicesFixture
    {
        [Fact]
        public async Task DownloadFile_Success()
        {
            var dir = Path.Combine(TestConstants.TestFilesDirectory, "HandlerDownload");

            Directory.CreateDirectory(dir);

            var filePath = Path.Combine(dir, "download.txt");
            File.WriteAllText(filePath, "download");

            var stream = File.Open(filePath, FileMode.Open);

            var fileToDownloadPath = await FileService.UploadFileAsync(
                new FileModel
                {
                    Attributes = new string[] { "handler" },
                    FileName = "download.txt",
                    FileStream = stream
                }, CancellationToken.None);

            var result = await FileHandlerService.DownloadFileAsync(fileToDownloadPath, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("download.txt", Path.GetFileName(result.Name));

            result.Dispose();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task DownloadFile_Error_IfFilePathNullOrEmpty(string filePath)
        {
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
                await FileHandlerService.DownloadFileAsync(filePath, CancellationToken.None));
        }

        [Fact]
        public async Task DownloadFile_Error_IfFilePathNotContainsFileName()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await FileHandlerService.DownloadFileAsync(TestConstants.TestFilesDirectory, CancellationToken.None));
        }

        [Fact]
        public async Task DownloadFile_Error_IfFileNotFound()
        {
            await Assert.ThrowsAsync<FileNotFoundException>(async () =>
                await FileHandlerService.DownloadFileAsync(Path.Combine(TestConstants.TestFilesDirectory, "test.txt"), CancellationToken.None));
        }
    }
}
