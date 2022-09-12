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
    public class ArchiveDirectoryTest : FileHelperFixture
    {
        [Fact]
        public async Task ArchiveDir_Success()
        {
            var dirToArchive = Path.Combine(TestFilesDirectory, "Archive");

            Directory.CreateDirectory(dirToArchive);

            var zipPath = FileHelper.ArchiveFolder(dirToArchive, TestFilesDirectory);

            Assert.True(File.Exists(zipPath));
            Assert.Equal(".zip", Path.GetExtension(zipPath));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ArchiveDir_Error_IfDirToArcIsEmpty(string dirToArchive)
        {
            Assert.Throws<ArgumentNullException>(() => FileHelper.ArchiveFolder(dirToArchive, TestFilesDirectory));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ArchiveDir_Success_IfArchivePathEmpty(string archivePath)
        {
            var dirToArchive = Path.Combine(TestFilesDirectory, "Archive");

            Directory.CreateDirectory(dirToArchive);

            var zipPath = FileHelper.ArchiveFolder(dirToArchive, archivePath);
           
            Assert.True(File.Exists(zipPath));
            Assert.Equal(Directory.GetParent(dirToArchive).FullName, Path.GetDirectoryName(zipPath));
            Assert.Equal(".zip", Path.GetExtension(zipPath));
        }
    }
}
