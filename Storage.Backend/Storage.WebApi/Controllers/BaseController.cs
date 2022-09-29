using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Storage.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        private IMapper _mapper;

        protected IMapper Mapper =>
            _mapper ??= HttpContext.RequestServices.GetService<IMapper>();


        private IMediator _mediator;

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        internal Guid UserId => !User.Identity.IsAuthenticated 
                                    ? Guid.Empty 
                                        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
}
