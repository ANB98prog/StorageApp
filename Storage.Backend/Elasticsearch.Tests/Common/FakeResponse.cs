namespace Elasticsearch.Tests.Common
{
    public class FakeResponse
    {
        public byte[] Body { get; set; }

        public int? StatusCode { get; set; }
    }
}
