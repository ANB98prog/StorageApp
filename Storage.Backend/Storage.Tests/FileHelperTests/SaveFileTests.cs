using Storage.Application.Common.Helpers;
using Storage.Tests.Common;

namespace Storage.Tests.FileHelperTests
{
    [Collection("TestFilesCollection")]
    public class SaveFileTests : FileHelperFixture
    {
        [Fact]
        public async Task SaveFile_Success()
        {
            var file = Path.Combine(TestFilesDirectory, "some_save.txt");

            var testText = "some test text";

            File.WriteAllText(file, testText);

            var stream = File.OpenRead(file);

            var saveTo = Path.Combine(TestFilesDirectory, "saved.txt");

            await FileHelper.SaveFileAsync(stream,
                        saveTo, CancellationToken.None);

            stream.Dispose();

            Assert.True(File.Exists(Path.Combine(TestFilesDirectory, "saved.txt")));

            File.Delete(file);
            File.Delete(saveTo);
        }

        [Fact]
        public async Task SaveFile_Error_IfStreamAreNull()
        {
            var saveTo = Path.Combine(TestFilesDirectory, "saved.txt");
            Stream st = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await FileHelper.SaveFileAsync(st,
                        saveTo, CancellationToken.None));

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("           ")]
        [InlineData(" ")]
        public async Task SaveFile_Error_IfFilePathAreEmpty(string filePath)
        {
            Stream st = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await FileHelper.SaveFileAsync(st,
                        filePath, CancellationToken.None));

        }

        [Fact]
        public async Task SaveFile_Error_IfFilePathNotContainsFileName()
        {
            var saveTo = Path.Combine(TestFilesDirectory, "saved.txt");
            Stream st = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await FileHelper.SaveFileAsync(st,
                        saveTo, CancellationToken.None));

        }
    }
}
