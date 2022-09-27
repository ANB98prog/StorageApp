using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using Serilog;
using Storage.Application.Common.Exceptions;
using Storage.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Common.Services
{
    /// <summary>
    /// Service to work with Elastic storage
    /// </summary>
    /// <typeparam name="T">Documents type</typeparam>
    public class ElasticStorageService<T>
        : IStorageDataService<T> where T : class
    {
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
        /// <param name="elasticClient">Elasticsearch client</param>
        /// <param name="logger">Logger</param>
        public ElasticStorageService(IElasticsearchClient elasticClient, ILogger logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        /// <summary>
        /// Adds data to elastic storage
        /// </summary>
        /// <param name="data">Data to add</param>
        /// <returns>Item id</returns>
        public async Task<Guid> AddDataToStorageAsync(T data)
        {
            try
            {
                _logger.Information($"Try to add data to elastic. Data type: {typeof(T)}");

                if(data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                var result = await _elasticClient.AddDocumentAsync<T>(GetIndexName(nameof(T)), data);                               

                _logger.Information("Data added to elastic successfully.");

                return Guid.Parse(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new ElasticStorageServiceException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_UPLOAD_ARCHIVE_FILE_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Gets index name from class name
        /// </summary>
        /// <param name="className">Class name</param>
        /// <returns>Formated index name</returns>
        private string GetIndexName(string className)
        {
            return Regex.Replace(className, "Model", string.Empty)
                        .ToLowerInvariant();
        }
    }
}
