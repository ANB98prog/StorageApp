using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.FileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class DownloadManyFilesTests : TestServicesFixture
    {
        [Fact]
        public async Task DownloadManyFiles_Success()
        {
            var dir = Path.Combine(TestConstants.TestFilesDirectory, "HandleManyDownload");

            Directory.CreateDirectory(dir);

            var files = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                var filePath = Path.Combine(dir, $"many_dw_{i}.txt");
                File.WriteAllText(filePath, i.ToString());

                files.Add(filePath);
            }

            var result = await FileHandlerService.DownloadManyFilesAsync(files, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(".zip", Path.GetExtension(result.Name));

            var unzippedPath = Path.Combine(TestConstants.TestFilesDirectory, "UnzippedMany");

            Directory.CreateDirectory(unzippedPath);

            await FileHelper.SaveFileAsync(result, Path.Combine(unzippedPath, Path.GetFileName(result.Name)), CancellationToken.None);

            unzippedPath = FileHelper.UnzipFolder(Path.Combine(unzippedPath, Path.GetFileName(result.Name)));

            Assert.True(Directory.Exists(unzippedPath));

            var filesCount = Directory.GetFiles(unzippedPath).Count();

            Assert.Equal(3, filesCount);

            result.Dispose();
        }

        [Fact]
        public async Task DownloadManyFiles_Error_IfFilesPathNull()
        {
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.DownloadManyFilesAsync(null, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("filesPath"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task DownloadManyFiles_Error_IfFilesPathEmpty()
        {
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.DownloadManyFilesAsync(new List<string>(), CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("filesPath"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task DownloadManyFiles_Error_IfFilesNotFound()
        {
            var filePath = Path.Combine(TestConstants.TestFilesDirectory, "notExisted.txt");
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.DownloadManyFilesAsync(new List<string>() { filePath }, CancellationToken.None));

            Assert.Equal(ErrorMessages.FileNotFoundErrorMessage(Path.GetFileName(filePath)), error.UserFriendlyMessage);
        }
    }
}
