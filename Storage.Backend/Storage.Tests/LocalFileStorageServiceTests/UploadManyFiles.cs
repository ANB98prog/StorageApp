using Storage.Application.Common.Models;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.LocalFileStorageServiceTests
{
    [Collection("TestServicesCollection")]
    public class UploadManyFiles : TestServicesFixture
    {
        [Fact]
        public async Task UploadManyFiles_Success()
        {
            var files = new List<FileModel>();
            var filesNames = new List<string>();

            var path = Path.Combine(TestConstants.TestFilesDirectory, "UploadMany");

            Directory.CreateDirectory(path);

            for (int i = 0; i < 3; i++)
            {
                var fileName = Path.Combine(path, $"file_{i}.txt");
                filesNames.Add(fileName);
                File.AppendAllText(fileName, i.ToString());
            }

            for (int i = 0; i < 3; i++)
            {
                files.Add(new FileModel
                {
                    FileName = Path.GetFileName(filesNames[i]),
                    Attributes = new string[] { "Many", "NotAnnotated" },
                    FileStream = File.OpenRead(filesNames[i]),
                });
            }

            var result = await FileService.UploadManyFilesAsync(files, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(3, Directory.GetFiles(Directory.GetParent(result[0].FullPath)?.FullName).Count());

            for (int i = 0; i < 3; i++)
            {
                files[i].FileStream.Dispose();
            }
        }

        [Fact]
        public async Task UploadManyFiles_Error_IfFilesNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    await FileService.UploadManyFilesAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task UploadManyFiles_Success_IfFilesAreEmpty()
        {
            var result = await FileService.UploadManyFilesAsync(new List<FileModel>(), CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async Task UploadManyFiles_Error_IfFilesStreamsAreNull()
        {
            var files = new List<FileModel>();

            for (int i = 0; i < 3; i++)
            {
                var fileName = Path.Combine(TestConstants.TestFilesDirectory, $"file_{i}.txt");

                files.Add(new FileModel
                {
                    FileName = fileName,
                    Attributes = new string[] { "many" }
                });
            }

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                    await FileService.UploadManyFilesAsync(files, CancellationToken.None));
        }
    }
}
