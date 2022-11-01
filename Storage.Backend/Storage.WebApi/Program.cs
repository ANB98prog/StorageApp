using Mapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Storage.Application;
using Storage.Application.Common;
using Storage.Application.Interfaces;
using Storage.WebApi.Middleware;
using Storage.WebApi.Services;
using System.Reflection;
using System.Text.Json.Serialization;
using Constants = Storage.WebApi.Common.Constants;

namespace Storage.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariables.ASPNETCORE_ENVIRONMENT);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false,
                reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var builder = WebApplication.CreateBuilder(args);

            ConfigureLogging(environment, configuration);
            ConfigureAppServices(builder.Services, builder.Configuration);

            try
            {
                // PlugIn Serilog
                builder.Host.UseSerilog();

                var app = builder.Build();

                app.UseSerilogRequestLogging();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCustomExceptionHandler();
                app.UseCors("AllowAll");

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Unexpected error occured while app initialization");
            }
        }

        private static void ConfigureLogging(string environment, IConfigurationRoot configuration)
        {
            var elasticUrl = Environment.GetEnvironmentVariable(EnvironmentVariables.ELASTIC_URL);
            var elasticUser = Environment.GetEnvironmentVariable(EnvironmentVariables.ELASTIC_USER);
            var elasticPass = Environment.GetEnvironmentVariable(EnvironmentVariables.ELASTIC_PASSWORD);
            var appName = Environment.GetEnvironmentVariable(EnvironmentVariables.APPLICATION_NAME) ?? configuration["AppName"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
                {
                    ModifyConnectionSettings = x => x.BasicAuthentication(elasticUser, elasticPass),
                    TypeName = null,
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = $"{appName}-logs-{environment?.ToLower().Replace(".", " - ")}-{DateTime.UtcNow:MM-yyyy}"
                })
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static void ConfigureAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddApplication();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name.StartsWith("Storage."));

            services.AddAutoMapper(config =>
            {
                foreach (var assembly in assemblies)
                {
                    config.AddProfile(new AssemblyMappingProfile(assembly));
                }
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                // Application will receive this amount of data per request
                options.Limits.MaxRequestBodySize = Constants.TEN_GIGABYTE_IN_BYTES;
            });

            services.Configure<FormOptions>(opt =>
            {
                opt.MultipartBodyLengthLimit = Constants.TEN_GIGABYTE_IN_BYTES;
            });

            services.AddSingleton(Log.Logger);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });

            services.AddSwaggerGen(config =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
        }

    }
}