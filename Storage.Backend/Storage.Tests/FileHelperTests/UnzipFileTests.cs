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
    public class UnzipFileTests : FileHelperFixture
    {
        [Fact]
        public void UnzipFolder_Success()
        {
            var archivePath = Path.Combine(TestFilesDirectory, "Archive");
            Directory.CreateDirectory(archivePath);

            var unzipDir = Path.Combine(TestFilesDirectory, "Unzipped");
            Directory.CreateDirectory(unzipDir);

            var file = Path.Combine(archivePath, "test.txt");
            using (var f = File.Create(file)) { }

            var archive = FileHelper.ArchiveFolder(archivePath);

            Directory.Delete(archivePath, true);

            FileHelper.UnzipFolder(archive, unzipDir);

            var unzipFilePath = Path.Combine(unzipDir, "test.txt");

            Assert.True(File.Exists(unzipFilePath));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UnzipFolder_Error_IfArchiveFilePathNull(string archivePath)
        {
            Assert.Throws<ArgumentNullException>(() => FileHelper.UnzipFolder(archivePath, TestFilesDirectory));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void UnzipFolder_Success_IfExtractPathNull(string extractPath)
        {
            var archivePath = Path.Combine(TestFilesDirectory, "Archive");
            Directory.CreateDirectory(archivePath);

            var file = Path.Combine(archivePath, "test.txt");
            using (var f = File.Create(file)) { }

            var archive = FileHelper.ArchiveFolder(archivePath);

            Directory.Delete(archivePath, true);

            FileHelper.UnzipFolder(archive, extractPath);

            var unzipFilePath = Path.Combine(archivePath, "test.txt");

            Assert.True(File.Exists(unzipFilePath));
        }
    }
}
