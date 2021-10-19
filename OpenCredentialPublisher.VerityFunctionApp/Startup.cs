using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.DependencyInjection;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.VerityFunctionApp.Dispatchers;
using OpenCredentialPublisher.VerityRestApi.Api;
using OpenCredentialPublisher.VerityRestApi.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;


namespace OpenCredentialPublisher.VerityFunctionApp
{
    public class Startup
    {

        public const string QueueTriggerStorageConfigurationSetting = "QueueStorageConnectionString";

        public static void Configure(HostBuilderContext context, IServiceCollection services, IUrlHelper urlHelper)
        {
            services.AddLogging();

            var config = context.Configuration;

            var keyVaultName = config["KeyVaultName"];

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console()
            .CreateLogger();

            services.AddOptions();

            services.Configure<CredentialPublisherOptions>(config.GetSection(CredentialPublisherOptions.Section));

            var queueConfig = config.GetSection(AzureQueueOptions.Section);
            services.Configure<AzureQueueOptions>(queueConfig);

            var queueOptions = queueConfig.Get<AzureQueueOptions>();
            config[QueueTriggerStorageConfigurationSetting] = queueOptions.StorageConnectionString;
            services.AddSingleton<IConfiguration>(config);

            //services.Configure<HostSettings>((o) => new HostSettings());

            services.Configure<AzureBlobOptions>(config.GetSection(AzureBlobOptions.Section));

            var verityConfig = config.GetSection(VerityOptions.Section);
            services.Configure<VerityOptions>(verityConfig);

            string connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<WalletDbContext>(
                options => {
                    SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString,
                        options => options.EnableRetryOnFailure(
                                   maxRetryCount: 5,
                                   maxRetryDelay: TimeSpan.FromSeconds(2),
                                   errorNumbersToAdd: null)
                        );
                });

            services.AddMediatR(typeof(Startup));
            services.AddHttpClient(ClrHttpClient.Default);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCommandQueryHandlers(typeof(ICommandHandler<>));
            services.AddCredentialMappingHandlers(typeof(ICredentialMapper<,>));
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<ICredentialMapperDispatcher, CredentialDispatcher>();
            services.AddSingleton(urlHelper);

            RegisterServices.FunctionsAppRegistration(services);

            var verityOptions = verityConfig.Get<VerityOptions>();
            if (verityOptions.UseVerityApi)
            {
                services.AddTransient<IVerityIntegrationService, VerityApiService>();
                services.AddTransient<VerityRestApi.Client.Configuration>((services) => {
                    var agentContextModel = JsonSerializer.Deserialize<AgentContextModel>(verityOptions.Token);
                    return new VerityRestApi.Client.Configuration(
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { VerityRestApi.Constants.ApiKeyHeader, agentContextModel.ApiKey } },
                        new Dictionary<string, string>(),
                        verityOptions.EndpointUrl
                        );  
                });

                services.AddTransient<IIssueCredentialApi, IssueCredentialApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new IssueCredentialApi(configuration);
                });
                services.AddTransient<IIssuerSetupApi, IssuerSetupApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new IssuerSetupApi(configuration);
                });
                services.AddTransient<IPresentProofApi, PresentProofApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new PresentProofApi(configuration);
                });
                services.AddTransient<IRelationshipApi, RelationshipApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new RelationshipApi(configuration);
                });
                services.AddTransient<IUpdateConfigsApi, UpdateConfigsApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new UpdateConfigsApi(configuration);
                });
                services.AddTransient<IUpdateEndpointApi, UpdateEndpointApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new UpdateEndpointApi(configuration);
                });
                services.AddTransient<IWriteCredDefApi, WriteCredDefApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new WriteCredDefApi(configuration);
                });
                services.AddTransient<IWriteSchemaApi, WriteSchemaApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new WriteSchemaApi(configuration);
                });

            }
            else
            {
                services.AddTransient<IVerityIntegrationService, VeritySdkService>();
            }
        }
    }

    public static class RegisterHandlers
    {
        public static void AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
            }
        }

        public static void AddCredentialMappingHandlers(this IServiceCollection services, Type handlerInterface)
        {
            var handlers = typeof(Startup).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
            }
        }
    }
}
