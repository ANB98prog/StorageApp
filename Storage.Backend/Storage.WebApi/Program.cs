using Microsoft.AspNetCore.Server.Kestrel.Core;
using Storage.Application;
using Storage.WebApi.Common;
using Storage.WebApi.Middleware;

namespace Storage.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureAppServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            app.UseCustomExceptionHandler();
            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureAppServices(IServiceCollection services, IConfiguration configuration)
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