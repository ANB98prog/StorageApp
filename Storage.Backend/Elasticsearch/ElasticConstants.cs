using Nest;
using System;

namespace Elasticsearch
{
    /// <summary>
    /// Elastic client constants
    /// </summary>
    public static class ElasticConstants
    {
        /// <summary>
        /// Request timeout
        /// </summary>
        public static Time REQUEST_TIMEOUT = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Maximum items count per request
        /// </summary>
        public static int MAX_ITEMS_PER_REQUEST = 1000;

        /// <summary>
        /// Index not found server message
        /// </summary>
        public const string INDEX_NOT_EXISTS_SERVER_MESSAGE = "no such index";

        /// <summary>
        /// Keyword property type
        /// </summary>
        public const string KEYWORD_PROPERTY = "keyword";
    }
}
