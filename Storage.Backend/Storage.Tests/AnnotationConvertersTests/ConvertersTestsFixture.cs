using Moq;
using Serilog;
using Storage.Application.DataConverters;

namespace Storage.Tests.AnnotationConvertersTests
{
    public class ConvertersTestsFixture : IDisposable
    {
        private readonly Mock<ILogger> Logger;

        public readonly string PathToTestFiles;

        public ConvertersTestsFixture()
        {
            PathToTestFiles = Path.Combine(Directory.GetCurrentDirectory(), "AnnotationConvertersTests", "TestData");
            Logger = new Mock<ILogger>();
        }

        public LabelMGConverter GetLabelMGConverter()
        {
            return new LabelMGConverter(Logger.Object);
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("AnnotatedDataConvertersCollection")]
    public class AnnotatedDataConvertersCollection : ICollectionFixture<ConvertersTestsFixture> { }
}
