using Storage.Application.Common.Models;
using Storage.Application.Common.Helpers;
using Storage.Tests.Common;

namespace Storage.Tests.ImagesFileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class UploadFileTests : TestServicesFixture
    {
        [Fact]
        public async Task UploadFile_Success()
        {
            var dir = Path.Combine(TestConstants.TestFilesDirectory, "UploadFile");

            Directory.CreateDirectory(dir);

            File.WriteAllText(Path.Combine(dir, "upload.txt"), "text");
            var file = File.Open(Path.Combine(dir, "upload.txt"), FileMode.Open);

            var id = Guid.NewGuid();
            var request = new UploadFileRequestModel
            {
                Id = id,
                SystemName = $"{id.Trunc()}.txt",
                Stream = file,
            };

            var result = await FileHandlerService.UploadFileAsync(request, CancellationToken.None);

            Assert.Equal(id, result);
        }

        [Fact]
        public async Task UploadFile_Error_IfRequestIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
                await FileHandlerService.UploadFileAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task UploadFile_Error_IfFileStreamIsNull()
        {
            var error = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                            await FileHandlerService.UploadFileAsync(new UploadFileRequestModel
                            {
                                SystemName = $"{Guid.NewGuid().Trunc()}.txt"
                            }, CancellationToken.None));

            Assert.Equal("file", error.ParamName);
        }
    }
}
