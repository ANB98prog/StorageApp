using HttpMethod = Elasticsearch.Net.HttpMethod;

namespace Elasticsearch.Tests.ElasticSearchCommon
{
    public interface IElasticFakeResponse
    {
        public byte[] GetResponseData(string url, HttpMethod method);
    }
}
