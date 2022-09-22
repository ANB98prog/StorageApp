﻿using Elasticsearch.Net;
using System.IO.Compression;
using System.Text;
using HttpMethod = Elasticsearch.Net.HttpMethod;

namespace Elasticsearch.Tests.ElasticSearchCommon
{
    public class TestConnection : IConnection
    {

        private static readonly byte[] EmptyBody = Encoding.UTF8.GetBytes("");

        private readonly string _basePath = "/";

        private readonly string _contentType;

        private readonly Exception _exception;

        private readonly InMemoryHttpResponse _productCheckResponse;

        private readonly string _productHeader;

        private readonly int _statusCode;

        private IElasticFakeResponse _fakeResponse;

        public TestConnection(IElasticFakeResponse fakeResponse)
        {
            _statusCode = 200;
            _productCheckResponse = ValidProductCheckResponse();
            _fakeResponse = fakeResponse;
        }

        public virtual TResponse Request<TResponse>(RequestData requestData) where TResponse : class, IElasticsearchResponse, new()
        {
            return ReturnConnectionStatus<TResponse>(requestData);
        }

        public virtual Task<TResponse> RequestAsync<TResponse>(RequestData requestData, CancellationToken cancellationToken) where TResponse : class, IElasticsearchResponse, new()
        {
            return ReturnConnectionStatusAsync<TResponse>(requestData, cancellationToken);
        }

        void IDisposable.Dispose()
        {
            DisposeManagedResources();
        }

        public static InMemoryHttpResponse ValidProductCheckResponse(string productName = null)
        {
            var data = new
            {
                name = "es01",
                cluster_name = "elasticsearch-test-cluster",
                version = new
                {
                    number = "7.14.0",
                    build_flavor = "default",
                    build_hash = "af1dc6d8099487755c3143c931665b709de3c764",
                    build_timestamp = "2020-08-11T21:36:48.204330Z",
                    build_snapshot = false,
                    lucene_version = "8.6.0"
                },
                tagline = "You Know, for Search"
            };
            using MemoryStream memoryStream = RecyclableMemoryStreamFactory.Default.Create();
            LowLevelRequestResponseSerializer.Instance.Serialize(data, memoryStream);
            InMemoryHttpResponse inMemoryHttpResponse = new InMemoryHttpResponse
            {
                ResponseBytes = memoryStream.ToArray()
            };
            inMemoryHttpResponse.Headers.Add("X-elastic-product", new List<string> { productName ?? "Elasticsearch" });
            return inMemoryHttpResponse;
        }

        protected TResponse ReturnConnectionStatus<TResponse>(RequestData requestData, byte[] responseBody = null, int? statusCode = null, string contentType = null) where TResponse : class, IElasticsearchResponse, new()
        {
            return ReturnConnectionStatus<TResponse>(requestData, null, responseBody, statusCode, contentType);
        }

        protected TResponse ReturnConnectionStatus<TResponse>(RequestData requestData, InMemoryHttpResponse productCheckResponse, byte[] responseBody = null, int? statusCode = null, string contentType = null) where TResponse : class, IElasticsearchResponse, new()
        {
            if (_basePath.Equals(requestData.Uri.AbsolutePath, StringComparison.Ordinal) && requestData.Method == HttpMethod.GET)
            {
                return ReturnProductCheckResponse<TResponse>(requestData, statusCode, productCheckResponse);
            }

            byte[] body = _fakeResponse.GetResponseData(requestData.Uri.AbsolutePath, requestData.Method);

            PostData postData = requestData.PostData;
            if (postData != null)
            {
                using MemoryStream memoryStream = requestData.MemoryStreamFactory.Create();
                if (requestData.HttpCompression)
                {
                    using GZipStream writableStream = new GZipStream(memoryStream, CompressionMode.Compress);
                    postData.Write(writableStream, requestData.ConnectionSettings);
                }
                else
                {
                    postData.Write(memoryStream, requestData.ConnectionSettings);
                }
            }

            requestData.MadeItToResponse = true;
            int valueOrDefault = statusCode.GetValueOrDefault();
            if (!statusCode.HasValue)
            {
                valueOrDefault = _statusCode;
                statusCode = valueOrDefault;
            }

            Stream responseStream = ((body != null) ? requestData.MemoryStreamFactory.Create(body) : requestData.MemoryStreamFactory.Create(EmptyBody));
            return ResponseBuilder.ToResponse<TResponse>(requestData, _exception, statusCode, null, responseStream, _productHeader, contentType ?? _contentType ?? RequestData.DefaultJsonMimeType);
        }

        protected Task<TResponse> ReturnConnectionStatusAsync<TResponse>(RequestData requestData, CancellationToken cancellationToken, byte[] responseBody = null, int? statusCode = null, string contentType = null) where TResponse : class, IElasticsearchResponse, new()
        {
            return ReturnConnectionStatusAsync<TResponse>(requestData, null, cancellationToken, responseBody, statusCode, contentType);
        }

        protected async Task<TResponse> ReturnConnectionStatusAsync<TResponse>(RequestData requestData, InMemoryHttpResponse productCheckResponse, CancellationToken cancellationToken, byte[] responseBody = null, int? statusCode = null, string contentType = null) where TResponse : class, IElasticsearchResponse, new()
        {
            if (_basePath.Equals(requestData.Uri.AbsolutePath, StringComparison.Ordinal) && requestData.Method == HttpMethod.GET)
            {
                return ReturnProductCheckResponse<TResponse>(requestData, statusCode, productCheckResponse);
            }

            byte[] body = _fakeResponse.GetResponseData(requestData.Uri.AbsolutePath, requestData.Method);

            PostData postData = requestData.PostData;
            if (postData != null)
            {
                using MemoryStream stream = requestData.MemoryStreamFactory.Create();
                if (!requestData.HttpCompression)
                {
                    await postData.WriteAsync(stream, requestData.ConnectionSettings, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    using GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress);
                    await postData.WriteAsync(zipStream, requestData.ConnectionSettings, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
            }

            requestData.MadeItToResponse = true;
            statusCode.GetValueOrDefault();
            if (!statusCode.HasValue)
            {
                int statusCode2 = _statusCode;
                statusCode = statusCode2;
            }

            Stream responseStream = ((body != null) ? requestData.MemoryStreamFactory.Create(body) : requestData.MemoryStreamFactory.Create(EmptyBody));
            return await ResponseBuilder.ToResponseAsync<TResponse>(requestData, _exception, statusCode, null, responseStream, _productHeader, contentType ?? _contentType, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        }

        private TResponse ReturnProductCheckResponse<TResponse>(RequestData requestData, int? statusCode, InMemoryHttpResponse productCheckResponse) where TResponse : class, IElasticsearchResponse, new()
        {
            if (productCheckResponse == null)
            {
                productCheckResponse = _productCheckResponse;
            }

            productCheckResponse.Headers.TryGetValue("X-elastic-product", out var value);
            requestData.MadeItToResponse = true;
            using MemoryStream responseStream = requestData.MemoryStreamFactory.Create(productCheckResponse.ResponseBytes);
            return ResponseBuilder.ToResponse<TResponse>(requestData, _exception, statusCode ?? productCheckResponse.StatusCode, null, responseStream, value?.FirstOrDefault(), RequestData.DefaultJsonMimeType);
        }

        protected virtual void DisposeManagedResources()
        {
        }
        public void Dispose()
        {
        }
    }
}
