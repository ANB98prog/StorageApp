using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Application.Common.Models;
using Storage.Tests.Common;

namespace Storage.Tests.FileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class UploadArchiveFileTests : TestServicesFixture
    {
        [Fact]
        public async Task UploadArchiveFile_Success()
        {
            var dir = Path.Combine(TestConstants.TestFilesDirectory, "UploadArchive");
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

            var result = await FileHandlerService.UploadArchiveFileAsync(request, new List<string> { "text/plain" }, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            archiveStream.Dispose();
        }

        [Fact]
        public async Task UploadArchiveFile_Error_IfArchiveIsNull()
        {
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.UploadArchiveFileAsync(null, new List<string> { "plain/text" }, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("file"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task UploadArchiveFile_Error_IfArchiveStreamIsNull()
        {
            var error = await Assert.ThrowsAsync<FileHandlerServiceException>(async () =>
                await FileHandlerService.UploadArchiveFileAsync(new UploadFileRequestModel()
                {
                    SystemName = $"{Guid.NewGuid().Trunc()}.zip",
                }, new List<string> { "plain/text" }, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("stream"), error.UserFriendlyMessage);
        }
    }
}
