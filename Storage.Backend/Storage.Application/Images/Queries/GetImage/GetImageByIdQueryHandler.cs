using AutoMapper;
using Elasticsearch.Exceptions;
using Elasticsearch.Interfaces;
using MediatR;
using Storage.Application.Common.Exceptions;
using Storage.Application.Images.Queries.Models;
using Storage.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using ErrorMessages = Storage.Application.Common.Exceptions.ErrorMessages;

namespace Storage.Application.Images.Queries.GetImage
{
    public class GetImageByIdQueryHandler
        : IRequestHandler<GetImageByIdQuery, ImageVm>
    {

        private readonly IElasticsearchClient _elasticService;

        private readonly IMapper _mapper;

        public GetImageByIdQueryHandler(IElasticsearchClient elasticService, IMapper mapper)
        {
            _elasticService = elasticService;
            _mapper = mapper;
        }

        public async Task<ImageVm> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _elasticService
                                        .GetByIdAsync<BaseFile>(ElasticIndices.FILES_INDEX, request.Id.ToString(), cancellationToken);

                if(response == null)
                {
                    throw new NotFoundException(request.Id.ToString());
                }

                return _mapper.Map<BaseFile, ImageVm>(response);
            }
            catch (ArgumentNullException ex)
            {
                throw new ServiceArgumentException(ex.Message, ErrorMessages.ArgumentNullExeptionMessage(ex.ParamName));
            }
            catch (ItemNotFoundException ex)
            {
                throw new NotFoundException(request.Id.ToString());
            }
            catch (IndexNotFoundException ex)
            {
                throw new NotFoundException(request.Id.ToString(), ex);
            }
            catch (BaseServiceException ex)
            {
                throw new UnexpectedStorageException(ex.UserFriendlyMessage, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_IMAGE_BY_ID_MESSAGE);
            }
            catch (Exception ex)
            {
                throw new UnexpectedStorageException(ex.Message, ErrorMessages.UNEXPECTED_ERROR_WHILE_GET_IMAGE_BY_ID_MESSAGE);
            }
        }
    }
}
