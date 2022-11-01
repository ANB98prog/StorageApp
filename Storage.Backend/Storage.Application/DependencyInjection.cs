﻿using AutoMapper;
using Elasticsearch.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Serilog;
using Storage.Application.Common;
using Storage.Application.Common.Behaviors;
using Storage.Application.Common.Services;
using Storage.Application.Interfaces;
using Storage.Domain;
using System;
using System.IO;
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

            services.AddServices();

            return services;
        }

        private static IServiceCollection AddServices(
            this IServiceCollection services)
        {
            var localStorageDir = Environment.GetEnvironmentVariable(EnvironmentVariables.LOCAL_STORAGE_DIR)
                ?? throw new ArgumentNullException("Local storage directory!");

            string temporaryFilesDir = "";

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if(env != null
                && env.Equals("Development"))
            {
                temporaryFilesDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
            }
            else
            {
                temporaryFilesDir = Environment.GetEnvironmentVariable(EnvironmentVariables.TEMPORARY_FILES_DIR)
                ?? throw new ArgumentNullException("Temporary files directory!");
            }

            services.AddTransient<IFileService>(s => new LocalFileStorageService(localStorageDir, s.GetService<ILogger>()));

            var elasticUrl = Environment.GetEnvironmentVariable(EnvironmentVariables.ELASTIC_URL)
                ?? throw new ArgumentNullException("Elastic url");
            var elasticUser = Environment.GetEnvironmentVariable(EnvironmentVariables.ELASTIC_USER)
                ?? throw new ArgumentNullException("Elastic user");
            var elasticPassword = Environment.GetEnvironmentVariable(EnvironmentVariables.ELASTIC_PASSWORD)
                ?? throw new ArgumentNullException("Elastic password");

            var settings = new ConnectionSettings(new Uri(elasticUrl))
                                .BasicAuthentication(elasticUser, elasticPassword)
                                .EnableApiVersioningHeader();

            var nestClient = new Nest.ElasticClient(settings);

            services.AddSingleton<Nest.IElasticClient>(nestClient);

            services.AddTransient<IElasticsearchClient>(s => new Elasticsearch.ElasticClient(s.GetService<Nest.IElasticClient>()));

            services.AddTransient<IStorageDataService>(s =>
                new ElasticStorageService(ElasticIndices.FILES_INDEX, s.GetService<ILogger>(), s.GetService<IMapper>(), s.GetService<IElasticsearchClient>()));

            services.AddTransient<IFileHandlerService>(s => new FileHandlerService(temporaryFilesDir, s.GetService<ILogger>(), s.GetService<IMapper>(), s.GetService<IFileService>(), s.GetService<IStorageDataService>()));

            services.AddTransient<IVideoFilesService>(s => new VideoFilesService(temporaryFilesDir, s.GetService<ILogger>(), s.GetService<IFileService>(), s.GetService<IStorageDataService>()));

            return services;
        }
    }
}
