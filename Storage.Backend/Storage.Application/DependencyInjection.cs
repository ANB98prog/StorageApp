using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Storage.Application.Common.Behaviors;
using System.Reflection;

namespace Storage.Application
{
    /// <summary>
    /// Application dependency injection extension
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application dependencies
        /// </summary>
        /// <param name="services">Services collection</param>
        /// <returns>Services collection</returns>
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            return services;
        }
    }
}
