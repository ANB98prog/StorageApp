using Mimetype;
using Xunit;

namespace MimeTypes
{
    public class MimetypesTests
    {
        [Fact]
        public void GetMimeTypeExtensions_Successfull()
        {
            var mimeType = "image/png";
            var expectedExtension = ".png";

            var actual = Mimetype.Mimetype.GetMimeTypeExtensions(mimeType);

            Assert.NotNull(actual);
            Assert.Single(actual);
            Assert.Equal(expectedExtension, actual.FirstOrDefault());
        }

        [Fact]
        public void GetMimeTypeExtensions_Tiff_Successfull()
        {
            var mimeType = "image/tiff";
            var expectedExtensions = new List<string> { ".tif", ".tiff" };

            var actual = Mimetype.Mimetype.GetMimeTypeExtensions(mimeType);

            Assert.NotNull(actual);
            Assert.Equal(2, actual.Count);

            for(var i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expectedExtensions[i], actual[i]); 
            }
        }

        [Fact]
        public void GetMimeTypeExtensions_With_Asterisk_Pattern_Successfull()
        {
            var mimeType = "image/*";
            var expectedExtensions = new List<string> { ".tif", ".tiff", ".bmp", ".gif", ".jpeg", ".jpg", ".png", ".svg" };

            var actual = Mimetype.Mimetype.GetMimeTypeExtensions(mimeType);

            Assert.NotNull(actual);
            Assert.Equal(8, actual.Count);

            foreach (var extention in expectedExtensions)
            {
                Assert.Contains(extention, actual);
            }
        }

        [Fact]
        public void GetMimeTypeExtensions_With_Wrong_Asterisk_Pattern_NotSuccessfull()
        {
            var mimeType = "*/some";

            var actual = Mimetype.Mimetype.GetMimeTypeExtensions(mimeType);

            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void GetMimeTypeExtensions_With_Empty_Mimetype_NotSuccessfull()
        {
            var actual = Mimetype.Mimetype.GetMimeTypeExtensions("");

            Assert.NotNull(actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void GetMimeTypeExtensions_With_Null_Mimetype_NotSuccessfull()
        {
            var actual = Mimetype.Mimetype.GetMimeTypeExtensions(null);

            Assert.NotNull(actual);
            Assert.Empty(actual);
        }
    }
}