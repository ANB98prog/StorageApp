using MediatR;
using Serilog;
using Storage.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Storage.Application.Common.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        ICurrentUserService _currentUserService;

        public LoggingBehaviour(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;

            Log.Information("Storage Request: {Name} {@UserId} {@Request}", requestName, userId, request);

            var response = await next();

            return response;
        }
    }
}
