﻿using HttpMethod = Elasticsearch.Net.HttpMethod;

namespace Elasticsearch.Tests.Common
{
    public interface IElasticFakeResponse
    {
        public FakeResponse GetResponseData(string url, HttpMethod method);
    }
}
