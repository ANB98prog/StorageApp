using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TemporaryFilesScheduler.Schedulers;

namespace TemporaryFilesScheduler.Scheduling
{
    /// <summary>
    /// Contains scheduler DI extensions
    /// </summary>
    public static class SchedulerExtension
    {
        /// <summary>
        /// Adds scheduler services
        /// </summary>
        /// <param name="services">Service collections</param>
        /// <returns>Service collections</returns>
        public static IServiceCollection AddScheduler(this IServiceCollection services)
        {
            return services.AddSingleton<IHostedService, SchedulerHostedService>();
        }

        /// <summary>
        /// Adds scheduler services
        /// </summary>
        /// <param name="services">Service collections</param>
        /// <param name="unobservedTaskExceptionHandler">Scheduled tasks exceptions handler</param>
        /// <returns>Service collections</returns>
        public static IServiceCollection AddScheduler(this IServiceCollection services, EventHandler<UnobservedTaskExceptionEventArgs> unobservedTaskExceptionHandler)
        {
            return services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider =>
            {
                var instance = new SchedulerHostedService(serviceProvider.GetServices<IScheduledTask>());
                instance.UnobservedTaskException += unobservedTaskExceptionHandler;
                return instance;
            });
        }
    }
}
