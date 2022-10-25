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

        public static FakeResponse AddDocumentSuccessResponse(string index, string id)
        {
            var response = new
            {
                _index = index,
                _id = id,
                result = "created"
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.Created,
            };
        }

        public static FakeResponse GetInvalidResponse()
        {
            return new FakeResponse
            {
                Body = null,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        public static FakeResponse GetIndexDeletedSuccessResponse()
        {
            var response = new
            {
                acknowledged = true
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        #region Delete document

        public static FakeResponse GetDocumentDeletedSuccessResponse(string index, string id)
        {
            var response = new
            {
                _index = index,
                _id = id,
                _version = 2,
                result = "deleted",
                _shards = new
                {
                    total = 2,
                    successful = 1,
                    failed = 0
                },
                _seq_no = 11,
                _primary_term = 2
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static FakeResponse GetDocumentDeletedNotFoundResponse(string index, string id)
        {
            var response = new
            {
                _index = index,
                _id = id,
                _version = 2,
                result = "not_found",
                _shards = new
                {
                    total = 2,
                    successful = 1,
                    failed = 0
                },
                _seq_no = 11,
                _primary_term = 2
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        public static FakeResponse GetDocumentDeletedIfItNotExistsResponse(string index, string id)
        {
            var response = new
            {
                _index = index,
                _id = id,
                _version = 2,
                result = "not_found",
                _shards = new
                {
                    total = 2,
                    successful = 1,
                    failed = 0
                },
                _seq_no = 11,
                _primary_term = 2
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        public static FakeResponse GetRefreshResponse()
        {
            var response = new
            {
                _shards = new
                {
                    total = 2,
                    successful = 1,
                    failed = 0
                }
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        public static FakeResponse GetBulkDocumentsDeletedSuccessResponse()
        {
            var response = new
            {
                deleted = 3
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        #endregion

        #region Search
        public static FakeResponse SearchSuccessfulResponse()
        {
            var response = new
            {
                hits = new
                {
                    total = new
                    {
                        value = 2,
                        relation = "eq"
                    },
                    max_score = 1.0,
                    hits = new[]
                    {
                        new 
                        {
                            _index = "indx",
                            _id = "id1",
                            _score = 1.0,
                            _source = new
                            {
                                id = "id1",
                                Title = "title1",
                                Rating = 1.0
                            }
                        },
                        new
                        {
                            _index = "indx",
                            _id = "id2",
                            _score = 1.0,
                            _source = new
                            {
                                id = "id2",
                                Title = "title2",
                                Rating = 1.0
                            }
                        }
                    }
                }
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static FakeResponse SearchEmptyResponse()
        {
            var response = new
            {
                hits = new
                {
                    total = new
                    {
                        value = 0,
                        relation = "eq"
                    },
                    max_score = 0,
                }
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static FakeResponse SearchIndexNotFoundResponse(string indexName)
        {
            var response = new
            {
                error = new
                {
                    root_cause = new[]
                    {
                        new
                        {
                            type = "index_not_found_exception",
                            reason = $"no such index [{indexName}]",
                            index = indexName
                        }
                    },
                    type = "index_not_found_exception",
                    reason = $"no such index [{indexName}]",
                    index = indexName
                },
                status = 404
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }

        public static FakeResponse GetByIdSuccessResponse(string id, string index)
        {
            var response = new
            {
                _index = index,
                _id = id,
                found = true,
                _source = new
                {
                    id = id,
                    title = "title",
                    rating = 10.0f
                }
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static FakeResponse GetByIdNotFoundResponse(string id, string index)
        {
            var response = new
            {
                _index = index,
                _id = id,
                found = false
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.NotFound,
            };
        }
        #endregion

        #region Update
        public static FakeResponse GetSuccessUpdateResponse(string id, string index)
        {
            var response = new
            {
                _index = index,
                _id = id,
                result = "updated"
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static FakeResponse GetBulkUpdateResponse(string index)
        {
            //var response = "{\r\n  \"took\": 200,\r\n  \"errors\": false,\r\n  \"items\": [\r\n    {\r\n      \"update\": {\r\n        \"_index\": \"simple\",\r\n        \"_id\": \"11\",\r\n        \"_version\": 3,\r\n        \"result\": \"updated\",\r\n        \"_shards\": {\r\n          \"total\": 2,\r\n          \"successful\": 1,\r\n          \"failed\": 0\r\n        },\r\n        \"_seq_no\": 7,\r\n        \"_primary_term\": 2,\r\n        \"status\": 200\r\n      }\r\n    },\r\n    {\r\n      \"update\": {\r\n        \"_index\": \"simple\",\r\n        \"_id\": \"22\",\r\n        \"_version\": 3,\r\n        \"result\": \"updated\",\r\n        \"_shards\": {\r\n          \"total\": 2,\r\n          \"successful\": 1,\r\n          \"failed\": 0\r\n        },\r\n        \"_seq_no\": 8,\r\n        \"_primary_term\": 2,\r\n        \"status\": 200\r\n      }\r\n    }\r\n  ]\r\n}";

            var response = new
            {
                took = 1,
                errors = false
            };

            return new FakeResponse
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)),
                StatusCode = (int)HttpStatusCode.OK,
            };
        }
        #endregion

    }
}
