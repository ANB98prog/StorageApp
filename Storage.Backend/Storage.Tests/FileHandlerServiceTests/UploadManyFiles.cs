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
    public class UploadManyFiles : TestServicesFixture
    {
        [Fact]
        public async Task UploadManyFiles_Success()
        {
            var ids = new List<Guid>();
            var dir = Path.Combine(TestConstants.TestFilesDirectory, "UploadMany");

            Directory.CreateDirectory(dir);

            var files = new List<UploadFileRequestModel>();

            for (int i = 0; i < 3; i++)
            {
                var path = Path.Combine(dir, $"uploadMany_{i}.txt");

                var id = Guid.NewGuid();
                var systemName = $"{id.Trunc()}.txt";

                files.Add(new UploadFileRequestModel
                {
                    Id = id,
                    SystemName = systemName,
                    OriginalName = "original.txt",
                    Stream = File.Create(path),
                });

                ids.Add(id);

                StorageDataServiceMock.Setup(s =>
                    s.AddDataToStorageAsync(It.Is<BaseFile>(s => s.Id.Equals(id))))
                        .ReturnsAsync(id);
            }

            var result = await FileHandlerService.UploadManyFileAsync(files, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);

            foreach (var id in ids)
            {
                Assert.Contains(id, result);
            }

            foreach (var file in files)
            {
                file.Stream.Dispose();
            }
        }

        [Fact]
        public async Task UploadManyFiles_Error_IfRequestIsNull()
        {
            var error = await Assert.ThrowsAsync<UserException>(async () =>
                await FileHandlerService.UploadManyFileAsync(null, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("files"), error.UserFriendlyMessage);
        }

        [Fact]
        public async Task UploadManyFiles_Error_IfFileStreamIsNull()
        {
             var error = await Assert.ThrowsAsync<UserException>(async () =>
                await FileHandlerService.UploadManyFileAsync(new List<UploadFileRequestModel>
                {
                    new UploadFileRequestModel
                    {
                        Id = Guid.NewGuid(),
                        SystemName = "some",
                        OriginalName = "name.txt"
                    }
                }, CancellationToken.None));

            Assert.Equal(ErrorMessages.EmptyRequiredParameterErrorMessage("file"), error.UserFriendlyMessage);
        }
    }
}
