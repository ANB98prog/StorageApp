using Storage.Application.Common.Models;
using Storage.Application.Common.Helpers;
using Storage.Tests.Common;
using Storage.Application.Common.Exceptions;
using Moq;
using Storage.Domain;
using Task = System.Threading.Tasks.Task;

namespace Storage.Tests.FileHandlerServiceTests
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
                OriginalName = "name.txt",
                SystemName = $"{id.Trunc()}.txt",
                Stream = file,
            };

            StorageDataServiceMock.Setup(s =>
                s.AddDataToStorageAsync(It.IsAny<BaseFile>()))
                .ReturnsAsync(id);

            var result = await FileHandlerService.UploadFileAsync(request, CancellationToken.None);

            Assert.Equal(id, result);
        }

        [Fact]
        public async Task UploadFile_Error_IfRequestIsNull()
        {
            var error = await Assert.ThrowsAsync<UserException>( async () =>
                await FileHandlerService.UploadFileAsync(null, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("file"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task UploadFile_Error_IfFileStreamIsNull()
        {
            var error = await Assert.ThrowsAsync<UserException>(async () =>
                            await FileHandlerService.UploadFileAsync(new UploadFileRequestModel
                            {
                                OriginalName = "name.txt",
                                SystemName = $"{Guid.NewGuid().Trunc()}.txt"
                            }, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("file"), error.UserFriendlyMessage);
        }
    }
}
