using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Models;
using Storage.Tests.Common;

namespace Storage.Tests.FileHandlerServiceTests
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

            stream.Dispose();

            var result = await FileHandlerService.DownloadFileAsync(fileToDownloadPath.FullPath, CancellationToken.None);

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
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>( async () =>
                await FileHandlerService.DownloadFileAsync(filePath, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("filePath"), error.Message);
        }

        [Fact]
        public async Task DownloadFile_Error_IfFilePathNotContainsFileName()
        {
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.DownloadFileAsync(TestConstants.TestFilesDirectory, CancellationToken.None));

            Assert.Equal(ErrorMessages.InvalidRequiredParameterErrorMessage("Path does not contain file name! (Parameter 'filePath')"), error.Message);
        }

        [Fact]
        public async Task DownloadFile_Error_IfFileNotFound()
        {
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.DownloadFileAsync(Path.Combine(TestConstants.TestFilesDirectory, "test.txt"), CancellationToken.None));

            Assert.Equal(ErrorMessages.FileNotFoundErrorMessage("test.txt"), error.Message);
        }
    }
}
