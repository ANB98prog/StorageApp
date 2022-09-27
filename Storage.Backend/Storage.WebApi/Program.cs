using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Storage.Application;
using Storage.WebApi.Common;
using Storage.WebApi.Middleware;
using ILogger = Serilog.ILogger;

namespace Storage.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .WriteTo.File("StorageWebAppLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ConfigureAppServices(builder.Services, builder.Configuration, Log.Logger);

            try
            {
                var app = builder.Build();

                app.UseCustomExceptionHandler();
                app.UseCors("AllowAll");

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error occured while app initialization");
            }
        }

        private static void ConfigureAppServices(IServiceCollection services, IConfiguration configuration, ILogger Logger)
        {
            services.AddControllers();
            services.AddApplication();

            //services.AddAutoMapper(config =>
            //{
            //    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            //    config.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
            //});

            services.Configure<KestrelServerOptions>(options =>
            {
                // Application will receive whis amount of data per request
                options.Limits.MaxRequestBodySize = Constants.ONE_GIGABYTE_IN_BYTES;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
        }

    }
}