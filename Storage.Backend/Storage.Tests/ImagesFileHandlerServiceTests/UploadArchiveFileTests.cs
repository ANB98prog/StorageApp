using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Tests.Common;

namespace Storage.Tests.ImagesFileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class UploadArchiveFileTests : TestServicesFixture
    {
        [Fact]
        public async Task UploadArchiveFile_Success()
        {
            var dir = Path.Combine(TestFilesDirectory, "UploadArchive");
            Directory.CreateDirectory(dir);

            var files = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                var path = Path.Combine(dir, $"toarchive{i}.txt");
                files.Add(path);

                File.WriteAllText(path, i.ToString());
            }

            var archivePath = FileHelper.ArchiveFolder(dir);

            var archiveStream = File.Open(archivePath, FileMode.Open);

            var request = new UploadFileRequestModel
            {
                IsAnnotated = false,
                OriginalName = Path.GetFileName(archiveStream.Name),
                SystemName = $"{Guid.NewGuid().Trunc()}{Path.GetExtension(archiveStream.Name)}",
                Stream = archiveStream
            };

            var result = await FileHandlerService.UploadArchiveFileAsync(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            archiveStream.Dispose();
        }

        [Fact]
        public async Task UploadArchiveFile_Error_IfArchiveIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileHandlerService.UploadArchiveFileAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task UploadArchiveFile_Error_IfArchiveStreamIsNull()
        {
            var error = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileHandlerService.UploadArchiveFileAsync(new UploadFileRequestModel()
                {
                    SystemName = $"{Guid.NewGuid().Trunc()}.zip",
                }, CancellationToken.None));

            Assert.Equal("stream", error.ParamName);
        }
    }
}
