using Storage.Application.Common.Exceptions;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.LocalFileStorageServiceTests
{
    [Collection("TestServicesCollection")]
    public class DownloadFileTests : TestServicesFixture
    {
        [Fact]
        public async Task DownloadFile_Success()
        {
            var filePath = Path.Combine(TestConstants.TestFilesDirectory, "Download", "download.txt");

            var fileText = "some text from downloaded file";

            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);

            using (var file = File.OpenWrite(filePath))
            {
                var text = Encoding.UTF8.GetBytes(fileText);
                await file.WriteAsync(text);
            }

            var result = await FileService.DownloadFileAsync(filePath, CancellationToken.None);

            Assert.NotNull(result);

            var buffer = new byte[result.Length];
            await result.ReadAsync(buffer);

            result.Dispose();

            Assert.Equal(fileText, Encoding.UTF8.GetString(buffer));
        }

        [Fact]
        public async Task DownloadFile_Error_IfFileNotExists()
        {
            var filePath = Path.Combine(TestConstants.TestFilesDirectory, "Download", "download.txt");

            var error = await Assert.ThrowsAsync<FileNotFoundException>(async () => 
                await FileService.DownloadFileAsync(filePath, CancellationToken.None));

            Assert.Equal($"Could not find file: '{filePath}'", error.Message);

        }

        [Fact]
        public async Task DownloadFile_Error_IfFileIsADirectory()
        {
            var filePath = Path.Combine(TestConstants.TestFilesDirectory, "Download");

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await FileService.DownloadFileAsync(filePath, CancellationToken.None));

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task DownloadFile_Error_IfFilePathAreEmpty(string filePath)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileService.DownloadFileAsync(filePath, CancellationToken.None));

        }
    }
}
