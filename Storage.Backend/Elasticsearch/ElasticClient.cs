using Elasticsearch.Exceptions;
using Nest;

namespace Elasticsearch
{
    /// <summary>
    /// ElasticSearch client implementstion
    /// </summary>
    public class ElasticClient : IElasticsearchClient
    {
        /// <summary>
        /// Elastic client
        /// </summary>
		private readonly IElasticClient _client;

        /// <summary>
        /// Initializes class instance of <see cref="ElasticClient"/>
        /// </summary>
        /// <param name="client">Elastic client</param>
		public ElasticClient(IElasticClient client)
		{
			_client = client;
		}

        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <param name="descriptor">Description of index</param>
        /// <exception cref="IndexCreationException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
        public async Task CreateIndexAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> descriptor = null)
        {
			try
			{
				var exists = await _client.Indices.ExistsAsync(indexName);

                if (exists.IsValid
					&& exists.Exists)
				{
					throw new IndexCreationException(indexName, ErrorMessages.INDEX_ALREADY_EXISTS(indexName));
				}

				var result = await _client.Indices.CreateAsync(indexName, descriptor);

				if (!result.IsValid)
				{
					throw new IndexCreationException(indexName, result.OriginalException);
				}
			}
			catch (IndexCreationException ex)
			{
				throw ex;
			}
			catch (Exception ex)
			{
				var index = string.IsNullOrWhiteSpace(indexName) 
										? string.Empty : indexName;

                throw new UnexpectedElasticException(ErrorMessages.ERROR_CREATING_INDEX(index), ex);
			}
        }

        /// <summary>
        /// Creates index
        /// </summary>
        /// <param name="indexName">Index name</param>
        /// <exception cref="IndexCreationException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
		public async Task CreateIndexAsync(string indexName)
		{
            try
            {
                var exists = await _client.Indices.ExistsAsync(indexName);

                if (exists.IsValid
                    && exists.Exists)
                {
                    throw new IndexCreationException(indexName, ErrorMessages.INDEX_ALREADY_EXISTS(indexName));
                }

                var result = await _client.Indices.CreateAsync(indexName);

                if (!result.IsValid)
                {
                    throw new IndexCreationException(indexName, result.OriginalException);
                }
            }
            catch (IndexCreationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                var index = string.IsNullOrWhiteSpace(indexName)
                                        ? string.Empty : indexName;

                throw new UnexpectedElasticException(ErrorMessages.ERROR_CREATING_INDEX(index), ex);
            }
        }
	}
}
