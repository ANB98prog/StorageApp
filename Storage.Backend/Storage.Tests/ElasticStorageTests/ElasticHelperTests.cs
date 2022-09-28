using Storage.Application.Common.Helpers;

namespace Storage.Tests.ElasticStorageTests
{
    public class ElasticHelperTests
    {
        [Theory]
        [InlineData("DataModel", "data")]
        [InlineData("Model", "model")]
        [InlineData("DataFlowModel", "data_flow")]
        [InlineData("model", "model")]
        [InlineData("data_flow", "data_flow")]
        [InlineData("CamelCaseModel", "camel_case")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void GetFormattedIndexNameTests_Success(string inputIndex, string expected)
        {
            var actual = ElasticHelper.GetFormattedIndexName(inputIndex);

            Assert.Equal(expected, actual);
        }
    }
}
