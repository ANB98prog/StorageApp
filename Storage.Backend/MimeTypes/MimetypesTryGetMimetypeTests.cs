using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimeTypes
{
    public class MimetypesTryGetMimetypeTests
    {
        [Fact]
        public void TryGetMimeType_Successfull()
        {
            var expectedMimeType = "image/png";
            var extension = ".png";

            var result = Mimetype.Mimetype.TryGetMimeType(extension, out var mimeType);

            Assert.True(result);
            Assert.NotNull(mimeType);
            Assert.Equal(expectedMimeType, mimeType);
        }

        [Fact]
        public void TryGetMimeType_FileName_Successfull()
        {
            var expectedMimeType = "image/tiff";
            var extension = "image.tiff";

            var result = Mimetype.Mimetype.TryGetMimeType(extension, out var mimeType);

            Assert.True(result);
            Assert.NotNull(mimeType);
            Assert.Equal(expectedMimeType, mimeType);
        }

        [Theory]
        [InlineData("image.poll")]
        [InlineData("")]
        [InlineData(null)]
        public void TryGetMimeType_Invalid_extension_NotSuccessfull(string extension)
        {
            var result = Mimetype.Mimetype.TryGetMimeType(extension, out var mimeType);

            Assert.False(result);
            Assert.Null(mimeType);
        }
    }
}
