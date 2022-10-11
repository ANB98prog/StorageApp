using AutoMapper;
using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using Elasticsearch.Models;
using MediatR;
using Nest;
using Storage.Application.Common;
using Storage.Application.Common.Exceptions;
using Storage.Application.Images.Queries.Models;
using Storage.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Images.Queries.GetImagesList
{
    public class GetImagesListQueryHandler
        : IRequestHandler<GetImagesListQuery, ImageListVm>
    {
        private readonly IElasticsearchClient _elasticService;

        private readonly IMapper _mapper;

        public GetImagesListQueryHandler(IElasticsearchClient elasticService, IMapper mapper)
        {
            _elasticService = elasticService;
            _mapper = mapper;
        }

        public async Task<ImageListVm> Handle(GetImagesListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var queryContainer = new QueryContainer()
                //{
                //    new BoolQuery
                //    {
                //        Must = new QueryStringQuery()
                //        {
                //            Fields = new Fields()
                //        }
                //    }
                //}

                var from = request.PageNumber * request.PageSize;
                var take = request.PageSize;
                var response = await _elasticService.SearchAsync<BaseFile>(descriptor => descriptor
                                    .Query(q => q
                                        .QueryString(queryDescriptor => queryDescriptor
                                            .Query(String.Join(" ", request.Attributes))
                                                .Fields(fs => fs.Fields(f => f.Attributes))
                                                .DefaultOperator(Operator.And)))
                                    .From(from)
                                    .Take(take)
                                    .Index(ElasticIndices.FILES_INDEX));

                if (response == null)
                {
                    return new ImageListVm()
                    {
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    };
                }

                var count = await _elasticService.CountAsync<BaseFile>(descriptor => descriptor
                                    .Query(q => q
                                        .QueryString(queryDescriptor => queryDescriptor
                                            .Query(String.Join(" ", request.Attributes))
                                                .Fields(fs => fs.Fields(f => f.Attributes))
                                                .DefaultOperator(Operator.And)))
                                    .Index(ElasticIndices.FILES_INDEX));

                var mapped = _mapper.Map<Elasticsearch.Models.SearchResponse<BaseFile>, ImageListVm>(response);

                mapped.Count = (int)count;
                mapped.PageNumber = request.PageNumber;
                mapped.PageSize = request.PageSize;

                return mapped;
            }
            catch (ArgumentNullException ex)
            {
                throw new ServiceArgumentException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (IndexNotFoundException)
            {
                return new ImageListVm()
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (BaseServiceException ex)
            {
                throw new UnexpectedStorageException(ex.UserFriendlyMessage, ErrorMessages.UNEXPECTED_ERROR_WHILE_SEARCH_IMAGES_MESSAGE);
            }
            catch (Exception ex)
            {
                throw new UnexpectedStorageException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_SEARCH_IMAGES_MESSAGE);
            }
        }
    }
}
