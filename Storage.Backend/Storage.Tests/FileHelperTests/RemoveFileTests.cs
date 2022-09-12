using Storage.Application.Common.Helpers;
using Storage.Tests.Common;

namespace Storage.Tests.FileHelperTests
{
    [Collection("TestFilesCollection")]
    public class RemoveFileTests : TestBase
    {
        [Fact]
        public async Task RemoveFile_Success()
        {
            var fileToRemove = Path.Combine(TestFilesDirectory, "remove.txt");

            using (var file = File.Create(fileToRemove))
            {
            }

            FileHelper.RemoveFile(fileToRemove);

            Assert.False(File.Exists(fileToRemove));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("       ")]
        public async Task RemoveFile_Error_IfEmptyPath(string path)
        {
             Assert.Throws<ArgumentNullException>( () => FileHelper.RemoveFile(path));
        }

        [Fact]
        public async Task RemoveFile_Success__IfFileNotExist()
        {
            var fileToRemove = Path.Combine(TestFilesDirectory, "remove.txt");

            FileHelper.RemoveFile(fileToRemove);
        }
    }
}
