using Storage.Application.Common.Models;
using Storage.Application.Common.Helpers;
using Storage.Tests.Common;

namespace Storage.Tests.ImagesFileHandlerServiceTests
{
    [Collection("TestServicesCollection")]
    public class UploadManyFiles : TestServicesFixture
    {
        [Fact]
        public async Task UploadManyFiles_Success()
        {
            var ids = new List<Guid>();
            var dir = Path.Combine(TestFilesDirectory, "UploadMany");

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
                    Stream = File.Create(path),
                });

                ids.Add(id);
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
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileHandlerService.UploadManyFileAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task UploadManyFiles_Error_IfFileStreamIsNull()
        {
             await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileHandlerService.UploadManyFileAsync(new List<UploadFileRequestModel>
                {
                    new UploadFileRequestModel
                    {
                        Id = Guid.NewGuid(),
                        SystemName = "some"
                    }
                }, CancellationToken.None));
        }
    }
}
