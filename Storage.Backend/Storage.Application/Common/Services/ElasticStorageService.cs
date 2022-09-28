using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using Serilog;
using Storage.Application.Common.Exceptions;
using Storage.Application.Interfaces;
using System;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Common.Services
{
    /// <summary>
    /// Service to work with Elastic storage
    /// </summary>
    public class ElasticStorageService
        : IStorageDataService
    {
        /// <summary>
        /// Data index
        /// </summary>
        private readonly string _index;

        /// <summary>
        /// Elasticsearch client
        /// </summary>
        private readonly IElasticsearchClient _elasticClient;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes class instance of <see cref="ElasticStorageService"/>
        /// </summary>
        /// <param name="index">Elasticsearch index</param>
        /// <param name="elasticClient">Elasticsearch client</param>
        /// <param name="logger">Logger</param>
        public ElasticStorageService(string index, IElasticsearchClient elasticClient, ILogger logger)
        {
            _index = index;
            _elasticClient = elasticClient;
            _logger = logger;
        }

        /// <summary>
        /// Adds data to elastic storage
        /// </summary>
        /// <param name="data">Data to add</param>
        /// <typeparam name="T">Documents type</typeparam>
        /// <returns>Item id</returns>
        public async Task<Guid> AddDataToStorageAsync<T>(T data) where T : class
        {
            try
            {
                _logger.Information($"Try to add data to elastic. Data type: {typeof(T)}");

                if(data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                var result = await _elasticClient.AddDocumentAsync<T>(_index, data);                               

                _logger.Information("Data added to elastic successfully.");

                return Guid.Parse(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new ElasticStorageServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_ADD_ITEM_TO_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_ADD_ITEM_TO_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_ADD_ITEM_TO_STORAGE_MESSAGE, ex);
            }
        }
    }
}
