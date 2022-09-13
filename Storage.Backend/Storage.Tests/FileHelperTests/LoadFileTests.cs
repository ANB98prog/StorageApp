using Storage.Application.Common.Helpers;
using Storage.Tests.Common;
using System.Text;

namespace Storage.Tests.FileHelperTests
{
    [Collection("TestFilesCollection")]
    public class LoadFileTests :FileHelperFixture
    {
        [Fact]
        public async Task LoadFile_Success()
        {
            var filePath = Path.Combine(TestFilesDirectory, "load.txt");

            var testText = "some text";

            using (var file = File.OpenWrite(filePath))
            {
                var buff = Encoding.UTF8.GetBytes(testText);
                file.Write(buff, 0, (int)buff.Length);
            }

            var result = await FileHelper.LoadFileAsync(filePath);

            Assert.NotNull(result);

            byte[] buffer = new byte[result.Length];
            result.Read(buffer, 0, (int)result.Length);

            result.Dispose();

            var text = Encoding.UTF8.GetString(buffer);

            Assert.Equal(testText, text);

            File.Delete(filePath);
        }

        [Fact]
        public async Task LoadFile_Error_IfFileNotExists()
        {
            var filePath = Path.Combine(TestFilesDirectory, "load.txt");

            await Assert.ThrowsAsync<FileNotFoundException>(
                    async () => await FileHelper.LoadFileAsync(filePath));
        }

        [Fact]
        public async Task LoadFile_Error_IfPathDoesNotContainFileName()
        {
            var filePath = Path.Combine(TestFilesDirectory);

            await Assert.ThrowsAsync<ArgumentException>(
                    async () => await FileHelper.LoadFileAsync(filePath));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("       ")]
        public async Task LoadFile_Error_IfFilePathIsNullOrEmpty(string filePath)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                    async () => await FileHelper.LoadFileAsync(filePath));
        }
    }
}
