using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.LocalFileStorageServiceTests
{
    [Collection("LocalFileStorageCollection")]
    public class DownloadFileTests : LocalFileStorageServiceFixture
    {
        [Fact]
        public async Task DownloadFile_Success()
        {
            var filePath = Path.Combine(TestFilesDirectory, "Download", "download.txt");

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
            var filePath = Path.Combine(TestFilesDirectory, "Download", "download.txt");

            await Assert.ThrowsAsync<FileNotFoundException>(async () => 
                await FileService.DownloadFileAsync(filePath, CancellationToken.None));

        }

        [Fact]
        public async Task DownloadFile_Error_IfFileIsADirectory()
        {
            var filePath = Path.Combine(TestFilesDirectory, "Download");

            await Assert.ThrowsAsync<FileNotFoundException>(async () =>
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
