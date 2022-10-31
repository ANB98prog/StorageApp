using AutoMapper;
using Elasticsearch;
using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using MediatR;
using Nest;
using Storage.Application.Common;
using Storage.Application.Common.Exceptions;
using Storage.Application.Common.Helpers;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Files.Queries.Attributes
{
    public class GetAttributesListQueryHandler
        : IRequestHandler<GetAttributesListQuery, AttributesListVm>
    {
        /// <summary>
        /// Elastic service
        /// </summary>
        private readonly IElasticsearchClient _elasticService;

        /// <summary>
        /// Initializes class instance of <see cref="GetAttributesListQueryHandler"/>
        /// </summary>
        /// <param name="elasticService">Elastic service</param>
        /// <param name="mapper">Contract mapper</param>
        public GetAttributesListQueryHandler(IElasticsearchClient elasticService)
        {
            _elasticService = elasticService;
        }

        public async Task<AttributesListVm> Handle(GetAttributesListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var attributes = new AttributesListVm()
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };

                var response = await _elasticService.SearchAsync(CreateSearchQuery(request), cancellationToken);

                if (response == null)
                {
                    return attributes;
                }

                var skip = request.PageNumber * request.PageSize;
                var take = request.PageSize;

                if (request.Query != null)
                {
                    var highlights = new List<string>();

                    foreach (var h in response.Documents)
                    {
                        if (h.Highlight != null)
                        {
                            highlights.AddRange(h.Highlight);
                        }
                    }

                    highlights = highlights.Distinct().ToList();

                    attributes.Attributes = highlights
                                                .Skip(skip)
                                                    .Take(take)
                                                        .ToList();

                    attributes.TotalCount = highlights.Count();
                }
                else
                {
                    var attrs = new List<string>();

                    foreach (var agg in response.Aggregations)
                    {
                        attrs.AddRange(agg.Value);
                    }

                    attributes.Attributes = attrs.Skip(skip)
                                                    .Take(take)
                                                        .ToList();

                    attributes.TotalCount = attrs.Count();
                }

                

                return attributes;
            }
            catch (ArgumentNullException ex)
            {
                throw new UserException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (IndexNotFoundException)
            {
                return new AttributesListVm()
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
                throw new CommandExecutionException(ex.UserFriendlyMessage, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_LIST_OF_ATTRIBUTES);
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_LIST_OF_ATTRIBUTES);
            }
        }

        /// <summary>
        /// Creates search query for elasticsearch
        /// </summary>
        /// <param name="request">User search request</param>
        /// <returns>Search request</returns>
        /// <exception cref="InvalidSearchRequestException"></exception>
        private SearchRequest<BaseFile> CreateSearchQuery(GetAttributesListQuery request)
        {
            var shouldQueries = new List<QueryContainer>();
            var mustQueries = new List<QueryContainer>();

            var queryContainer = new QueryContainer();

            AggregationDictionary aggs = null;
            BoolQuery boolQuery = new BoolQuery();


            if (request.Query != null 
                && !string.IsNullOrWhiteSpace(request.Query))
            {
                shouldQueries.Add(new MatchPhrasePrefixQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Attributes))),
                    Query = request.Query
                });
                shouldQueries.Add(new FuzzyQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Attributes))),
                    Value = request.Query,
                    Boost = 1,
                    Fuzziness = Fuzziness.EditDistance(2)
                });

                boolQuery.Should = shouldQueries;
            }
            else
            {
                queryContainer = new QueryContainer(new MatchAllQuery());

                aggs = new AggregationDictionary
                {
                    {
                        "uniq_attrs", new CompositeAggregation("uniq_attrs")
                        {
                            Size = ElasticConstants.MAX_AGGREGATION_ITEMS_PER_REQUEST,
                            Sources = new List<ICompositeAggregationSource>
                            {
                                new TermsCompositeAggregationSource(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Attributes)))
                                {
                                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Attributes)).Keyword())
                                }
                            }
                        }
                    }
                };
            }

            if (request.IsAnnotated.HasValue)
            {
                mustQueries.Add(new MatchQuery
                {
                    Field = new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.IsAnnotated))),
                    Query = request.IsAnnotated.Value.ToString().ToLower()
                });

                boolQuery.Must = mustQueries;
            }

            if(boolQuery.Must != null
                || boolQuery.Should != null)
            {
                queryContainer = new QueryContainer(boolQuery);
            }            

            return new SearchRequest<BaseFile>(ElasticIndices.FILES_INDEX)
            {
                TrackTotalHits = true,
                Query = queryContainer,
                Source = false,
                Highlight = new Highlight
                {
                    PreTags = new[] {""},
                    PostTags = new[] {""},
                    Fields = new FluentDictionary<Field, IHighlightField>()
                                .Add(new Field(ElasticHelper.GetFormattedPropertyName(nameof(BaseFile.Attributes))), new HighlightField())
                },
                Aggregations = aggs
            };
        }
    }
}
