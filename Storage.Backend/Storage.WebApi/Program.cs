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
using TemporaryFilesScheduler.Schedulers;
using TemporaryFilesScheduler.Scheduling;
using Constants = Storage.WebApi.Common.Constants;
using ILogger = Serilog.ILogger;

namespace Storage.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureLogging(builder.Configuration);
            ConfigureAppServices(builder.Services, builder.Configuration);
            ConfigureScheduledTasks(builder.Services, builder.Configuration);

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

        private static void ConfigureLogging(IConfigurationRoot configuration)
        {
            var environment = configuration[EnvironmentVariables.ASPNETCORE_ENVIRONMENT] 
                ?? throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT");
            var elasticUrl = configuration[EnvironmentVariables.ELASTIC_URL];
            var elasticUser = configuration[EnvironmentVariables.ELASTIC_USER];
            var elasticPass = configuration[EnvironmentVariables.ELASTIC_PASSWORD];
            var appName = configuration[EnvironmentVariables.APPLICATION_NAME] ?? configuration["AppName"];

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.File("StorageWebApi-.txt", rollingInterval: RollingInterval.Day)
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

            services.AddApplication(configuration);

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

        private static void ConfigureScheduledTasks(IServiceCollection services, IConfiguration configuration)
        {
            string temporaryFilesDir = "";

            var env = configuration[EnvironmentVariables.ASPNETCORE_ENVIRONMENT];

            if (env != null
                && env.Equals("Development"))
            {
                temporaryFilesDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            }
            else
            {
                temporaryFilesDir = configuration[EnvironmentVariables.TEMPORARY_FILES_DIR]
                ?? throw new ArgumentNullException("Temporary files directory!");
            }

            Log.Information("Create Scheduled tasks");

            var schedule = Constants.REMOVE_DEFAULT_TIME;

            if(!string.IsNullOrWhiteSpace(EnvironmentVariables.TEMPORARY_FILES_REMOVE_SCHEDULE_TIME) 
                    && TimeSpan.TryParse(configuration[EnvironmentVariables.TEMPORARY_FILES_REMOVE_SCHEDULE_TIME], out var parsedSchedule))
            {
                schedule = parsedSchedule;
            }

            var maxFileAge = Constants.REMOVE_DEFAULT_MAX_FILE_AGE;

            if(!string.IsNullOrWhiteSpace(EnvironmentVariables.TEMPORARY_FILE_MAX_AGE) 
                    && TimeSpan.TryParse(configuration[EnvironmentVariables.TEMPORARY_FILE_MAX_AGE], out var parsedMaxAge))
            {
                maxFileAge = parsedMaxAge;
            }

            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask>(s => new TempFilesRemoveScheduler(Log.Logger, schedule, maxFileAge, temporaryFilesDir));
            services.AddScheduler((sender, args) =>
            {
                Log.Logger.Error($"Scheduler error: {args.Exception.Message}", args.Exception);
                args.SetObserved();
            });
        }
    }
}