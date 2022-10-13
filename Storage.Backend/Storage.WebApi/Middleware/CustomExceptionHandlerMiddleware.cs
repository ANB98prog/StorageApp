using Storage.Application.Common.Exceptions;
using Storage.WebApi.Common.Exceptions;
using System.Net;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;

namespace Storage.WebApi.Middleware
{
    /// <summary>
    /// Custom exception handler middleware
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) => 
            _next = next;

        /// <summary>
        /// Invokes next request in middleware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles exception during middleware
        /// </summary>
        /// <param name="context">Request context</param>
        /// <param name="exception">Exception while processing</param>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    var errmsg = validationException.Errors.Select(msg => msg.ErrorMessage);
                    result = JsonSerializer.Serialize(new UserfriendlyException(errmsg));
                    break;
                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new UserfriendlyException(notFoundException.UserFriendlyMessage));
                    break;
                case InvalidSearchRequestException invalidSearchRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new UserfriendlyException(invalidSearchRequestException.UserFriendlyMessage));
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if(result == string.Empty)
            {
                result = JsonSerializer.Serialize(new UserfriendlyException(exception.Message));
            }

            return context.Response.WriteAsync(result);
        }
    }
}
