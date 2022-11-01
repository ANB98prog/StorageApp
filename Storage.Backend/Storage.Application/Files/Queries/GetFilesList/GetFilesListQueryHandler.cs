﻿using AutoMapper;
using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using MediatR;
using Nest;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Files.Queries.GetFilesList
{
    /// <summary>
    /// Get files list query handler
    /// </summary>
    public class GetFilesListQueryHandler
        : IRequestHandler<GetFilesListQuery, FilesListVm>
    {
        /// <summary>
        /// Elastic service
        /// </summary>
        private readonly IElasticsearchClient _elasticService;

        /// <summary>
        /// Contract mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes class instance of <see cref="GetFilesListQueryHandler"/>
        /// </summary>
        /// <param name="elasticService">Elastic service</param>
        /// <param name="mapper">Contract mapper</param>
        public GetFilesListQueryHandler(IElasticsearchClient elasticService, IMapper mapper)
        {
            _elasticService = elasticService;
            _mapper = mapper;
        }

        public async Task<FilesListVm> Handle(GetFilesListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _elasticService.SearchAsync(CreateSearchQuery(request), cancellationToken);

                if (response == null)
                {
                    return new FilesListVm()
                    {
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    };
                }

                var mapped = _mapper.Map<Elasticsearch.Models.SearchResponse<BaseFile>, FilesListVm>(response);

                mapped.PageNumber = request.PageNumber;
                mapped.PageSize = request.PageSize;

                return mapped;
            }
            catch (ArgumentNullException ex)
            {
                throw new UserException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (IndexNotFoundException)
            {
                return new FilesListVm()
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (InvalidSearchRequestException ex)
            {
                throw ex;
            }
            catch (BaseServiceException ex)
            {
                throw new CommandExecutionException(ex.UserFriendlyMessage, ErrorMessages.UNEXPECTED_ERROR_WHILE_SEARCH_FILES_MESSAGE);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_SEARCH_FILES_MESSAGE);
            }
        }

        /// <summary>
        /// Creates search query for elasticsearch
        /// </summary>
        /// <param name="request">User search request</param>
        /// <returns>Search request</returns>
        /// <exception cref="InvalidSearchRequestException"></exception>
        private SearchRequest<BaseFile> CreateSearchQuery(GetFilesListQuery request)
        {
            var matchQueries = new List<QueryContainer>();
            var filterQueries = new List<QueryContainer>();

            #region Attributes region
            if (request.Attributes != null
                && request.Attributes.Any())
            {
                matchQueries.Add(new MatchQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Attributes))),
                    Query = string.Join(" ", request.Attributes),
                    Operator = Operator.And
                });
            }
            #endregion

            #region Date filter
            if (request.CreatedFrom != null
                || request.CreatedTo != null)
            {
                if(request.CreatedFrom != null 
                    && request.CreatedTo != null
                        && request.CreatedFrom > request.CreatedTo)
                {
                    throw new InvalidSearchRequestException(ErrorMessages.INVALID_SEARCH_REQUEST_FROM_DATE_GRT_THAN_TO);
                }

                var fromDate = request.CreatedFrom ?? DateTime.MinValue;
                var toDate = request.CreatedTo ?? DateTime.Now;

                filterQueries.Add(new DateRangeQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.CreatedAt))),
                    GreaterThanOrEqualTo = fromDate,
                    LessThanOrEqualTo = toDate
                });
            }
            #endregion

            #region Owner and department filter
            if (request.OwnerId != null
                && !request.OwnerId.Value.Equals(Guid.Empty))
            {
                filterQueries.Add(new TermQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.OwnerId)).Keyword()),
                    Value = request.OwnerId
                });
            }

            if (request.DepartmentOwnerId != null
                && !request.DepartmentOwnerId.Value.Equals(Guid.Empty))
            {
                filterQueries.Add(new TermQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.DepartmentOwnerId)).Keyword()),
                    Value = request.DepartmentOwnerId
                });
            }
            #endregion

            #region IsAnnotated
            if (request.IsAnnotated.HasValue)
            {
                filterQueries.Add(new TermQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.IsAnnotated))),
                    Value = request.IsAnnotated.Value
                });
            }
            #endregion

            #region Mimetypes
            if (request.MimeTypes != null 
                && request.MimeTypes.Any())
            {
                request.MimeTypes.ForEach(m => matchQueries.Add(new MatchQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.MimeType))),
                    Query = Regex.Replace(m, @"/", @"//"),
                    Operator = Operator.Or
                }));
            }
            #endregion

            var from = request.PageNumber * request.PageSize;
            var take = request.PageSize;

            var queryContainer = new QueryContainer(new BoolQuery()
            {
                Must = matchQueries,
                Filter = filterQueries
            });

            return new SearchRequest<BaseFile>(ElasticIndices.FILES_INDEX)
            {
                TrackTotalHits = true,
                Query = queryContainer,
                From = from,
                Size = take,
                Sort = new List<ISort>
                {
                    new FieldSort
                    {
                        Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Id)).Keyword()),
                        Order = SortOrder.Ascending
                    }
                }
            };
        }
    }
}
