using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.FileHelperTests
{
    [Collection("TestFilesCollection")]
    public class GetFileTypeTests
    {
        [Theory]
        #region Image
        [InlineData(FileType.Image, "C://fake/image.png")]
        [InlineData(FileType.Image, "C://fake/image.gif")]
        [InlineData(FileType.Image, "C://fake/image.jpg")]
        [InlineData(FileType.Image, "C://fake/image.jpeg")]
        [InlineData(FileType.Image, "C://fake/image.bmp")]
        [InlineData(FileType.Image, "C://fake/image.tif")]
        [InlineData(FileType.Image, "C://fake/image.tiff")]
        #endregion
        #region Text
        [InlineData(FileType.Text, "C://fake/file.txt")]
        [InlineData(FileType.Text, "C://fake/file.docx")]
        #endregion
        #region Video
        [InlineData(FileType.Video, "C://fake/file.mp4")]
        [InlineData(FileType.Video, "C://fake/file.avi")]
        [InlineData(FileType.Video, "C://fake/file.mpg")]
        [InlineData(FileType.Video, "C://fake/file.mpeg")]
        [InlineData(FileType.Video, "C://fake/file.wmv")]
        #endregion
        #region Video
        [InlineData(FileType.Audio, "C://fake/file.mp3")]
        [InlineData(FileType.Audio, "C://fake/file.wav")]
        [InlineData(FileType.Audio, "C://fake/file.wma")]
        [InlineData(FileType.Audio, "C://fake/file.mid")]
        [InlineData(FileType.Audio, "C://fake/file.midi")]
        [InlineData(FileType.Audio, "C://fake/file.aiff")]
        [InlineData(FileType.Audio, "C://fake/file.au")]
        #endregion
        public void GetFileType_Success(FileType expected, string filePath)
        {
            var result = FileHelper.GetFileType(filePath);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetFileType_Error_NotSupportedType()
        {
            var result = FileHelper.GetFileType("C://fake/file.ll");

            Assert.Equal(FileType.Unknown, result);
        }
    }
}
