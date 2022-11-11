using AutoMapper;
using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using Serilog;
using Storage.Application.Common.Exceptions;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Common.Services
{
    /// <summary>
    /// Service to work with Elastic storage
    /// </summary>
    public partial class ElasticStorageService
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
        /// Contract mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes class instance of <see cref="ElasticStorageService"/>
        /// </summary>
        /// <param name="index">Elasticsearch index</param>
        /// <param name="logger">Logger</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="elasticClient">Elasticsearch client</param>
        public ElasticStorageService(string index, ILogger logger, IMapper mapper, IElasticsearchClient elasticClient)
        {
            _index = index;
            _elasticClient = elasticClient;
            _logger = logger;
            _mapper = mapper;
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

                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                if (!await _elasticClient.IndexExistsAsync(_index))
                {
                    await _elasticClient
                            .CreateIndexAsync(_index, d => d
                                .Map<BaseFile>(m => m
                                    .Properties(ps => ps
                                        .Keyword(k => k
                                            .Name(n => n.MimeType)))
                                            .AutoMap()));
                }

                var result = await _elasticClient.AddDocumentAsync<T>(_index, data);

                _logger.Information("Data added to elastic successfully.");

                return Guid.Parse(result);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new UserException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
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

        public async Task<T> GetFileInfoAsync<T>(Guid id) where T : class
        {
            try
            {
                _logger.Information($"Try to get file info from elastic. File id: '{id}'");

                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                var result = await _elasticClient.GetByIdAsync<T>(_index, id.ToString());

                if (result == null)
                {
                    _logger.Information($"Info for file with id: '{id}' not found!");
                }
                else
                {
                    _logger.Information("Info successfully got.");
                }

                return result;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new UserException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (ItemNotFoundException ex)
            {
                _logger.Error(ex, ex.Message);
                throw new NotFoundException(id.ToString());
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE, ex);
            }
        }

        public async Task<List<T>> GetFilesInfoAsync<T>(List<Guid> ids) where T : class
        {
            try
            {
                _logger.Information($"Try to get files info from elastic. Files count: '{ids.Count}'");

                var infos = new List<T>();

                if (ids.Any())
                {
                    infos = await _elasticClient
                                .GetManyByIdsAsync<T>(_index, ids.Select(id => id.ToString())
                                    .ToList());

                    _logger.Information($"Files info successfully got. Files found: '{infos.Count}'");
                }

                return infos;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new UserException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEMS_INFO_FROM_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEMS_INFO_FROM_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEMS_INFO_FROM_STORAGE_MESSAGE, ex);
            }
        }

        /// <summary>
        /// Removes file from storage
        /// </summary>
        /// <param name="id">Item id</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexNotFoundException"></exception>
        /// <exception cref="DeleteDocumentException"></exception>
        /// <exception cref="UnexpectedElasticException"></exception>
        /// <returns>Acknowledged</returns>
        public async Task<bool> RemoveFileFromStorageAsync(Guid id)
        {
            try
            {
                _logger.Information($"Try to remove file from elastic. File id: '{id}'");

                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(id));
                }

                var removed = await _elasticClient.DeleteDocumentAsync(_index, id.ToString());

                if (!removed)
                {
                    _logger.Information($"File with id: '{id}' not removed!");
                }
                else
                {
                    _logger.Information("File successfully removed.");
                }

                return removed;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new UserException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (DeleteDocumentException ex)
            {
                _logger.Error(ex, ErrorMessages.RemovingItemFromStorageErrorMessage(id.ToString()));
                throw new ElasticStorageServiceException(ErrorMessages.RemovingItemFromStorageErrorMessage(id.ToString()), ex);
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE, ex);
            }
        }

        public async Task<bool> RemoveFilesFromStorageAsync(List<Guid> ids)
        {
            try
            {
                _logger.Information($"Try to remove files from elastic. Files count: '{ids.Count}'");


                if (ids != null
                    && ids.Any())
                {
                    await _elasticClient.DeleteBulkByIdAsync(_index, ids.Select(i => i.ToString()));
                }

                return true;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName));
                throw new UserException(ErrorMessages.EmptyRequiredParameterErrorMessage(ex.ParamName), ex);
            }
            catch (DeleteDocumentException ex)
            {
                _logger.Error(ex, ErrorMessages.RemovingItemFromStorageErrorMessage(ids.ToString()));
                throw new ElasticStorageServiceException(ErrorMessages.RemovingItemFromStorageErrorMessage(ids.ToString()), ex);
            }
            catch (UnexpectedElasticException ex)
            {
                _logger.Error(ex, ex.UserfriendlyMessage);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE, ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE);
                throw new ElasticStorageServiceException(ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_ITEM_INFO_FROM_STORAGE_MESSAGE, ex);
            }
        }
    }
}
