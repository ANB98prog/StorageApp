using HttpMethod = Elasticsearch.Net.HttpMethod;

namespace Elasticsearch.Tests.ElasticSearchCommon
{
    public interface IElasticFakeResponse
    {
        public FakeResponse GetResponseData(string url, HttpMethod method);
    }
}
