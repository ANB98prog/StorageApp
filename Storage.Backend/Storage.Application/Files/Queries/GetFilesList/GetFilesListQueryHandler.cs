using AutoMapper;
using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using MediatR;
using Nest;
using Storage.Application.Common.Exceptions;
using Storage.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Files.Queries.GetFilesList
{
    public class GetFilesListQueryHandler
        : IRequestHandler<GetFilesListQuery, FilesListVm>
    {
        private readonly IElasticsearchClient _elasticService;

        private readonly IMapper _mapper;

        public GetFilesListQueryHandler(IElasticsearchClient elasticService, IMapper mapper)
        {
            _elasticService = elasticService;
            _mapper = mapper;
        }

        public async Task<FilesListVm> Handle(GetFilesListQuery request, CancellationToken cancellationToken)
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
                                            .Query(string.Join(" ", request.Attributes))
                                                .Fields(fs => fs.Fields(f => f.Attributes))
                                                .DefaultOperator(Operator.And)))
                                    .From(from)
                                    .Take(take)
                                    .Index(ElasticIndices.FILES_INDEX));

                if (response == null)
                {
                    return new FilesListVm()
                    {
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize
                    };
                }

                var count = await _elasticService.CountAsync<BaseFile>(descriptor => descriptor
                                    .Query(q => q
                                        .QueryString(queryDescriptor => queryDescriptor
                                            .Query(string.Join(" ", request.Attributes))
                                                .Fields(fs => fs.Fields(f => f.Attributes))
                                                .DefaultOperator(Operator.And)))
                                    .Index(ElasticIndices.FILES_INDEX));

                var mapped = _mapper.Map<Elasticsearch.Models.SearchResponse<BaseFile>, FilesListVm>(response);

                mapped.TotalCount = (int)count;
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
                return new FilesListVm()
                {
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                };
            }
            catch (BaseServiceException ex)
            {
                throw new UnexpectedStorageException(ex.UserFriendlyMessage, ErrorMessages.UNEXPECTED_ERROR_WHILE_SEARCH_FILES_MESSAGE);
            }
            catch (Exception ex)
            {
                throw new UnexpectedStorageException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_SEARCH_FILES_MESSAGE);
            }
        }
    }
}
