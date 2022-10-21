using Moq;
using Serilog;
using Storage.Application.DataConverters;
using Storage.Tests.Common;

namespace Storage.Tests.AnnotationConvertersTests
{
    public class ConvertersTestsFixture : IDisposable
    {
        private readonly Mock<ILogger> Logger;

        public readonly string PathToTestFiles;

        public readonly string PathToTempFiles;

        public ConvertersTestsFixture()
        {
            PathToTestFiles = Path.Combine(Directory.GetCurrentDirectory(), "AnnotationConvertersTests", "TestData");
            PathToTempFiles = Path.Combine(Directory.GetCurrentDirectory(), "AnnotationConvertersTests", "TempData");

            Logger = new Mock<ILogger>();
        }

        public YoloAnnotationConverter GetLabelMGConverter()
        {
            return new YoloAnnotationConverter(Logger.Object, PathToTempFiles);
        }

        public void Dispose()
        {
            TestHelper.RemoveTestData(PathToTempFiles);
        }
    }

    [CollectionDefinition("AnnotatedDataConvertersCollection")]
    public class AnnotatedDataConvertersCollection : ICollectionFixture<ConvertersTestsFixture> { }
}
