using Storage.Application.Common.Helpers;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.LocalFileStorageServiceTests
{
    [Collection("LocalFileStorageCollection")]
    public class DownloadManyFilesTests : LocalFileStorageServiceFixture
    {
        [Fact]
        public async Task DownloadManyFiles_Success()
        {
            var path = Path.Combine(TestFilesDirectory, "DownloadMany");

            Directory.CreateDirectory(path);

            var files = new List<string>()
            {
                Path.Combine(path, "dw.txt"),
                Path.Combine(path, "dw2.txt"),
                Path.Combine(path, "dw3.txt"),
            };

            foreach (var file in files)
            {
                using (var f = File.OpenWrite(file)) { }
            }

            var result = await FileService.DownloadManyFilesAsync(files, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(".zip", Path.GetExtension(result.Name));

            var unzippedPath = FileHelper.UnzipFolder(Path.Combine(Factory.StorageDirectory, result.Name));

            var unzippedFilesCount = Directory.GetFiles(unzippedPath).Length;

            Assert.Equal(3, unzippedFilesCount);

            result.Dispose();
        }

        [Fact]
        public async Task DownloadManyFiles_Failed_FilesAreNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () 
                => await FileService.DownloadManyFilesAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task DownloadManyFiles_Failed_FilesAreEmpty()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async ()
                => await FileService.DownloadManyFilesAsync(new List<string>(), CancellationToken.None));
        }

        [Fact]
        public async Task DownloadManyFiles_Error_IfFilesAreNotExists()
        {
            var path = Path.Combine(TestFilesDirectory, "DownloadMany");

            Directory.CreateDirectory(path);

            var files = new List<string>()
            {
                Path.Combine(path, "dw.txt"),
                Path.Combine(path, "dw2.txt"),
                Path.Combine(path, "dw3.txt"),
            };

            await Assert.ThrowsAsync<FileNotFoundException>(async () 
                => await FileService.DownloadManyFilesAsync(files, CancellationToken.None));
        }
    }
}
