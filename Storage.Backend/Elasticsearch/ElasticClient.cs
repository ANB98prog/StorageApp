using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using Nest;

namespace Elasticsearch
{
    /// <summary>
    /// ElasticSearch client implementstion
    /// </summary>
    public partial class ElasticClient : IElasticsearchClient
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
        public async Task CreateIndexAsync(string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> descriptor = null, CancellationToken cancellationToken = default(CancellationToken))
        {
			try
			{
                if (string.IsNullOrWhiteSpace(indexName))
                    throw new ArgumentNullException(nameof(indexName));

                var exists = await _client.Indices.ExistsAsync(indexName, ct: cancellationToken);

                if (exists.IsValid
					&& exists.Exists)
				{
					throw new IndexCreationException(indexName, ErrorMessages.INDEX_ALREADY_EXISTS(indexName));
				}

				var result = await _client.Indices.CreateAsync(indexName, descriptor, cancellationToken);

				if (!result.IsValid)
				{
					throw new IndexCreationException(indexName, result.OriginalException);
				}
			}
            catch (ArgumentNullException ex)
            {
                throw ex;
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
		public async Task CreateIndexAsync(string indexName, CancellationToken cancellationToken = default(CancellationToken))
		{
            try
            {
                if (string.IsNullOrWhiteSpace(indexName))
                    throw new ArgumentNullException(nameof(indexName));

                var exists = await _client.Indices.ExistsAsync(indexName, ct: cancellationToken);

                if (exists.IsValid
                    && exists.Exists)
                {
                    throw new IndexCreationException(indexName, ErrorMessages.INDEX_ALREADY_EXISTS(indexName));
                }

                var result = await _client.Indices.CreateAsync(indexName, ct: cancellationToken);

                if (!result.IsValid)
                {
                    throw new IndexCreationException(indexName, result.OriginalException);
                }
            }
            catch(ArgumentNullException ex)
            {
                throw ex;
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
        /// Deletes index
        /// </summary>
        /// <param name="indexName">Index to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task DeleteIndexAsync(string indexName, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (string.IsNullOrWhiteSpace(indexName))
                    throw new ArgumentNullException(nameof(indexName));

                var exists = await _client.Indices.ExistsAsync(indexName, ct: cancellationToken);

                if (!exists.IsValid
                    || !exists.Exists)
                {
                    throw new IndexNotFoundException(indexName);
                }

                var result = await _client.Indices.DeleteAsync(indexName, ct: cancellationToken);

                if (!result.IsValid)
                {
                    throw new UnexpectedElasticException();
                }
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (IndexNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                var index = string.IsNullOrWhiteSpace(indexName)
                                        ? string.Empty : indexName;

                throw new UnexpectedElasticException(ErrorMessages.ERROR_REMOVING_INDEX(index), ex);
            }
        }
    }
}
