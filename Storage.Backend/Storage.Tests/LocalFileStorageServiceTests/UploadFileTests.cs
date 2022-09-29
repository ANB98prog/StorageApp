using Storage.Application.Common.Models;
using Storage.Tests.Common;
using System.Text;
using FileAttributes = Storage.Application.Common.Models.FileAttributes;

namespace Storage.Tests.LocalFileStorageServiceTests
{
    [Collection("TestServicesCollection")]
    public class UploadFileTests : TestServicesFixture
    {
        [Fact]
        public async Task UploadFile_Success()
        {
            var file = Path.Combine(TestConstants.TestFilesDirectory, "upload.txt");

            UploadedFileModel resultPath = new UploadedFileModel();

            var text = "some text";

            using (var f = File.Open(file, FileMode.Create))
            {
                var buffer = Encoding.UTF8.GetBytes(text);
                f.Write(buffer);
            }

            var fileToUpload = File.Open(file, FileMode.Open);

            var model = new FileModel
            {
                Attributes = new string[] { FileAttributes.NotAnnotated.ToString() },
                FileName = Path.GetFileName(file),
                FileStream = fileToUpload
            };

            resultPath = await FileService.UploadFileAsync(model, CancellationToken.None);

            fileToUpload.Dispose();

            Assert.NotNull(resultPath);

            var resultText = "";

            using (var ff = File.OpenRead(resultPath.FullPath))
            {
                var buffer = new byte[ff.Length];
                ff.Read(buffer, 0, buffer.Length);
                resultText = Encoding.UTF8.GetString(buffer);
            }

            Assert.Equal(text, resultText);
        }

        [Fact]
        public async Task UploadFile_Failed_IfFileModelNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileService.UploadFileAsync(null, CancellationToken.None));
        }

        [Fact]
        public async Task UploadFile_Failed_IfFileStreamIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await FileService.UploadFileAsync(new FileModel
                {
                    Attributes= new string[] { FileAttributes.NotAnnotated.ToString() },
                    FileName = "some.txt",
                    FileStream = null
                }, CancellationToken.None));
        }
    }
}
