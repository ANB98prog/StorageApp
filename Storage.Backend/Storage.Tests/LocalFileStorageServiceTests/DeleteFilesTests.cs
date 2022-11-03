using Storage.Tests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.LocalFileStorageServiceTests
{
    [Collection("TestServicesCollection")]
    public class DeleteFilesTests : TestServicesFixture
    {
        /*
         Тест проверяет, что все пустые директории удаляются
         */
        [Fact]
        public async Task DownloadFile_Success()
        {
            var firstDir = Path.Combine(TestConstants.StorageDirectory, "Remove1");
            var secondDir = Path.Combine(firstDir, "Remove2");
            var secondDir2 = Path.Combine(firstDir, "Remove2_1");
            var filePath = Path.Combine(secondDir, "remove.txt");

            var fileText = "some text from deleted file";

            Directory.CreateDirectory(firstDir);
            Directory.CreateDirectory(secondDir);
            Directory.CreateDirectory(secondDir2);
            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);

            using (var file = File.OpenWrite(filePath))
            {
                var text = Encoding.UTF8.GetBytes(fileText);
                await file.WriteAsync(text);
            }

            FileService.DeleteFile(filePath);

            Assert.False(File.Exists(filePath));
            Assert.False(Directory.Exists(secondDir));
            Assert.False(Directory.Exists(secondDir2));
            Assert.False(Directory.Exists(firstDir));
            Assert.True(Directory.Exists(TestConstants.StorageDirectory));            
        }

        /*
         Тест проверяет, что директория не будет удалена если в ней остается файл
         */
        [Fact]
        public async Task DownloadFile_If_Dir_Not_Empty_Success()
        {
            var firstDir = Path.Combine(TestConstants.StorageDirectory, "NotRemove1");
            var secondDir = Path.Combine(firstDir, "NotRemove2");
            var secondDir2 = Path.Combine(firstDir, "NotRemove2_1");
            var filePath = Path.Combine(secondDir, "remove.txt");
            var notRemoveFilePath = Path.Combine(secondDir, "not_remove.txt");

            var fileText = "some text from deleted file";

            Directory.CreateDirectory(firstDir);
            Directory.CreateDirectory(secondDir);
            Directory.CreateDirectory(secondDir2);
            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);

            using (var file = File.OpenWrite(filePath))
                using (var file2 = File.OpenWrite(notRemoveFilePath))
                {
                    var text = Encoding.UTF8.GetBytes(fileText);
                    await file.WriteAsync(text);
                    await file2.WriteAsync(text);
                }

            FileService.DeleteFile(filePath);

            Assert.False(File.Exists(filePath));
            Assert.True(File.Exists(notRemoveFilePath));
            Assert.True(Directory.Exists(secondDir));
            Assert.True(Directory.Exists(secondDir2));
            Assert.True(Directory.Exists(firstDir));
            Assert.True(Directory.Exists(TestConstants.StorageDirectory));
        }

        /*
         Тест проверяет, что директория не будет удалена если в ней остается файл
         */
        [Fact]
        public async Task DownloadFile_If_Second_Dir_Not_Empty_Success()
        {
            var firstDir = Path.Combine(TestConstants.StorageDirectory, "NotRemove12");
            var secondDir = Path.Combine(firstDir, "NotRemove22");
            var secondDir2 = Path.Combine(firstDir, "NotRemove2_1");
            var filePath = Path.Combine(secondDir, "remove.txt");
            var notRemoveFilePath = Path.Combine(secondDir2, "not_remove.txt");

            var fileText = "some text from deleted file";

            Directory.CreateDirectory(firstDir);
            Directory.CreateDirectory(secondDir);
            Directory.CreateDirectory(secondDir2);
            Directory.CreateDirectory(Directory.GetParent(filePath).FullName);

            using (var file = File.OpenWrite(filePath))
            using (var file2 = File.OpenWrite(notRemoveFilePath))
            {
                var text = Encoding.UTF8.GetBytes(fileText);
                await file.WriteAsync(text);
                await file2.WriteAsync(text);
            }

            FileService.DeleteFile(filePath);

            Assert.False(File.Exists(filePath));
            Assert.True(File.Exists(notRemoveFilePath));
            Assert.False(Directory.Exists(secondDir));
            Assert.True(Directory.Exists(secondDir2));
            Assert.True(Directory.Exists(firstDir));
            Assert.True(Directory.Exists(TestConstants.StorageDirectory));

        }
    }
}
