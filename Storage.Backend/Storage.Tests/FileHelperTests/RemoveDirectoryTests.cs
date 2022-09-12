using Storage.Application.Common.Helpers;
using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.FileHelperTests
{

    [Collection("TestFilesCollection")]
    public class RemoveDirectoryTests : FileHelperFixture
    {
        [Fact]
        public async Task RemoveDirectory_Success()
        {
            var dirToRemove = Path.Combine(TestFilesDirectory, "RemoveDir");

            Directory.CreateDirectory(dirToRemove);

            FileHelper.RemoveDirectory(dirToRemove);

            Assert.False(Directory.Exists(dirToRemove));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("       ")]
        public async Task RemoveDirectory_Error_IfEmptyPath(string path)
        {
            Assert.Throws<ArgumentNullException>(() => FileHelper.RemoveDirectory(path));
        }

        [Fact]
        public async Task RemoveDirectory_Success_IfDirNotExist()
        {
            var dirToRemove = Path.Combine(TestFilesDirectory, "RemDir");

            FileHelper.RemoveFile(dirToRemove);
        }
    }
}
