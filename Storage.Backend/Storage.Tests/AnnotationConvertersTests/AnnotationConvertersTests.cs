using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Tests.AnnotationConvertersTests
{
    public class AnnotationConvertersTests
    {
        [Fact]
        public void TestConvertingFromRelativeToPixels ()
        {
            var expected = new PixelsAnnotationBbox(2014, 3324, 2065, 3354);

            var relBbox = new RelativeAnnotationBbox(0.716696f, 0.979466f, 0.017575f, 0.0088f);
            var imageInfo = new AnnotationImageInfo(2845, 3409);

            var relativeBboxes = new BoudingBox(imageInfo, relBbox);

            var convertedToPixels = relativeBboxes.PixelsAnnotation;

            Assert.Equal(expected.X1, convertedToPixels.X1);
            Assert.Equal(expected.X2, convertedToPixels.X2);
            Assert.Equal(expected.Y1, convertedToPixels.Y1);
            Assert.Equal(expected.Y2, convertedToPixels.Y2);

        }

        [Fact]
        public void TestConvertingFromPixelsToRelative()
        {
            var expected = new RelativeAnnotationBbox(0.7169f, 0.9795f, 0.0179f, 0.0088f);
            var pixels = new PixelsAnnotationBbox(2014, 3324, 2065, 3354);
                        
            var imageInfo = new AnnotationImageInfo(2845, 3409);

            var pixelsBboxes = new BoudingBox(imageInfo, pixels);

            var convertedToRelative = pixelsBboxes.RelativeAnnotation;

            Assert.Equal(expected.X.ToString("#.####"), convertedToRelative.X.ToString("#.####"));
            Assert.Equal(expected.Y.ToString("#.####"), convertedToRelative.Y.ToString("#.####"));
            Assert.Equal(expected.H.ToString("#.####"), convertedToRelative.H.ToString("#.####"));
            Assert.Equal(expected.W.ToString("#.####"), convertedToRelative.W.ToString("#.####"));

        }
    }
}
