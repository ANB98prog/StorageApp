using Elasticsearch.Tests.ElasticSearchCommon;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Elasticsearch.Tests.Common
{
    public static class ElasticTestHelper
    {
        public static FakeResponse GetIndexExistsResponse()
        {
            var response = new
            {
                Exists = true,
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static FakeResponse GetIndexNotExistsResponse()
        {
            var response = new
            {
                Exists = false,
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        public static FakeResponse GetIndexCreateSuccessResponse(string index)
        {
            var response = new
            {
                acknowledged = true,
                shards_acknowledged = true,
                index = "test"
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
    }
}
