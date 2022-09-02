namespace Storage.WebApi.Middleware
{
    public static class CustomExceptionHandlerMiddlewareExtension
    {
        /// <summary>
        /// Includes custom exception handler middleware to pipline
        /// </summary>
        /// <param name="builder">Application buider</param>
        /// <returns>Application buider</returns>
        public static IApplicationBuilder UseCustomExceptionHandler(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }
}
