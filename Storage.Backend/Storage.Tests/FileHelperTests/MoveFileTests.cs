using Storage.Application.Common.Helpers;
using Storage.Tests.Common;

namespace Storage.Tests.FileHelperTests
{

    [Collection("TestFilesCollection")]
    public class MoveFileTests : FileHelperFixture
    {
        #region Single file
        [Fact]
        public async Task MoveFileTo_Success()
        {
            var origFilePath = Path.Combine(TestFilesDirectory, "orig.txt");

            using (var file = File.Create(origFilePath))
            {
            }

            var moveToDir = Path.Combine(TestFilesDirectory, "Moved");
            Directory.CreateDirectory(moveToDir);

            await FileHelper.MoveFileToAsync(origFilePath, moveToDir);

            Assert.False(File.Exists(origFilePath));
            Assert.True(File.Exists(Path.Combine(moveToDir, "orig.txt")));
        }

        [Fact]
        public async Task MoveFileTo_Success_IfDestDirNotExist()
        {
            var origFilePath = Path.Combine(TestFilesDirectory, "orig.txt");

            using (var file = File.Create(origFilePath))
            {
            }

            var moveToDir = Path.Combine(TestFilesDirectory, "MovedNotExt");

            await FileHelper.MoveFileToAsync(origFilePath, moveToDir);

            Assert.False(File.Exists(origFilePath));
            Assert.True(File.Exists(Path.Combine(moveToDir, "orig.txt")));
        }

        [Fact]
        public async Task MoveFileTo_Error_IfSourceFileNotExist()
        {
            var origFilePath = Path.Combine(TestFilesDirectory, "orig.txt");

            var moveToDir = Path.Combine(TestFilesDirectory, "Moved");
            Directory.CreateDirectory(moveToDir);

            await Assert.ThrowsAsync<FileNotFoundException>(
                async () => await FileHelper.MoveFileToAsync(origFilePath, moveToDir));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("       ")]
        public async Task MoveFileTo_Error_IfSourceFilePathEmpty(string sourceFile)
        {
            var moveToDir = Path.Combine(TestFilesDirectory, "Moved");

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await FileHelper.MoveFileToAsync(sourceFile, moveToDir));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("       ")]
        public async Task MoveFileTo_Error_IfDestFilePathEmpty(string destFilePath)
        {
            var sourceFile = Path.Combine(TestFilesDirectory, "orig.txt");

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await FileHelper.MoveFileToAsync(sourceFile, destFilePath));
        }
        #endregion

        #region Many files
        [Fact]
        public async Task MoveFilesTo_Success()
        {
            var origFilesNames = new List<string>()
            {
                "orig1.txt",
                "orig2.txt",
                "orig3.txt",
            };

            var origFilesPath = new List<string>();

            foreach (var origFile in origFilesNames)
            {
                var filePath = Path.Combine(TestFilesDirectory, origFile);
                using (var file = File.Create(filePath))
                {}
                origFilesPath.Add(filePath);
            }            

            var moveToDir = Path.Combine(TestFilesDirectory, "MovedMany");
            Directory.CreateDirectory(moveToDir);

            await FileHelper.MoveFilesToAsync(origFilesPath, moveToDir);


            foreach (var file in origFilesPath)
            {
                Assert.False(File.Exists(file)); 
            }

            foreach (var file in origFilesNames)
            {
                Assert.True(File.Exists(Path.Combine(moveToDir, file)));
            }            
        }

        [Fact]
        public async Task MoveFilesTo_Success_IfDestDirNotExist()
        {
            var origFilesNames = new List<string>()
            {
                "origm1.txt",
                "origm2.txt",
                "origm3.txt",
            };

            var origFilesPath = new List<string>();

            foreach (var origFile in origFilesNames)
            {
                var filePath = Path.Combine(TestFilesDirectory, origFile);
                using (var file = File.Create(filePath))
                { }
                origFilesPath.Add(filePath);
            }

            var moveToDir = Path.Combine(TestFilesDirectory, "MovedManyNotExt");

            await FileHelper.MoveFilesToAsync(origFilesPath, moveToDir);

            foreach (var file in origFilesPath)
            {
                Assert.False(File.Exists(file));
            }

            foreach (var file in origFilesNames)
            {
                Assert.True(File.Exists(Path.Combine(moveToDir, file)));
            }
        }

        [Fact]
        public async Task MoveFilesTo_Error_IfSourceFileNotExist()
        {
            var origFilesPath = new List<string>()
            {
                Path.Combine(TestFilesDirectory,"origmmm1.txt"),
                Path.Combine(TestFilesDirectory,"origmmm2.txt"),
                Path.Combine(TestFilesDirectory,"origmmm3.txt"),
            };

            var moveToDir = Path.Combine(TestFilesDirectory, "Moved");
            Directory.CreateDirectory(moveToDir);

            await Assert.ThrowsAsync<FileNotFoundException>(
                async () => await FileHelper.MoveFilesToAsync(origFilesPath, moveToDir));
        }

        [Fact]
        public async Task MoveFilesTo_Error_IfSourceFilePathEmpty()
        {
            var moveToDir = Path.Combine(TestFilesDirectory, "Moved");

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await FileHelper.MoveFilesToAsync(new List<string> { null }, moveToDir));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("       ")]
        public async Task MoveFilesTo_Error_IfDestFilePathEmpty(string destFilePath)
        {
            var origFilesPath = new List<string>()
            {
                Path.Combine(TestFilesDirectory,"origmmm1.txt"),
                Path.Combine(TestFilesDirectory,"origmmm2.txt"),
                Path.Combine(TestFilesDirectory,"origmmm3.txt"),
            };

            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await FileHelper.MoveFilesToAsync(origFilesPath, destFilePath));
        }
        #endregion
    }
}