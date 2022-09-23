using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch
{
    public partial class ElasticClient
    {
        /// <summary>
        /// Searches documents
        /// </summary>
        /// <typeparam name="TDocument">Documents types</typeparam>
        /// <param name="index">Index to search in</param>
        /// <param name="request">Search request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Documents</returns>
        public Task<Models.SearchResponse<TDocument>> SearchAsync<TDocument>(string index, SearchRequest<TDocument> request, CancellationToken cancellationToken = default) where TDocument : class
        {
            throw new NotImplementedException();
        }
    }
}
