using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
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

[assembly: WebJobsStartup(typeof(OpenCredentialPublisher.VerityFunctionApp.Startup))]

namespace OpenCredentialPublisher.VerityFunctionApp
{
    public class Startup : IWebJobsStartup
    {

        public const string QueueTriggerStorageConfigurationSetting = "QueueStorageConnectionString";

        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.AddLogging();

            var configBuilder = new ConfigurationBuilder();
            var config = configBuilder.AddEnvironmentVariables().Build();

            var keyVaultName = config["KeyVaultName"];

            if (!String.IsNullOrEmpty(keyVaultName))
            {
                var secretClient = new SecretClient(new Uri($"https://{keyVaultName}.vault.azure.net/"),
                                                         new ManagedIdentityCredential());
                config = configBuilder
                    .AddAzureKeyVault(secretClient, new KeyVaultSecretManager())
                    .SetBasePath(Environment.CurrentDirectory)
                    .AddJsonFile("local.settings.json", true)
                    .AddEnvironmentVariables()
                    .Build();
            }
            else
            {
                config = configBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("appsettings.json", true)
                   .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                   .AddEnvironmentVariables()
                   .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
                   .Build();
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console()
            .CreateLogger();

            builder.Services.AddOptions();

            builder.Services.Configure<CredentialPublisherOptions>(config.GetSection(CredentialPublisherOptions.Section));

            var queueConfig = config.GetSection(AzureQueueOptions.Section);
            builder.Services.Configure<AzureQueueOptions>(queueConfig);

            var queueOptions = queueConfig.Get<AzureQueueOptions>();
            config[QueueTriggerStorageConfigurationSetting] = queueOptions.StorageConnectionString;
            builder.Services.AddSingleton<IConfiguration>(config);


            builder.Services.Configure<AzureBlobOptions>(config.GetSection(AzureBlobOptions.Section));

            var verityConfig = config.GetSection(VerityOptions.Section);
            builder.Services.Configure<VerityOptions>(verityConfig);

            string connectionString = config.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<WalletDbContext>(
                options => {
                    SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString,
                        options => options.EnableRetryOnFailure(
                                   maxRetryCount: 5,
                                   maxRetryDelay: TimeSpan.FromSeconds(2),
                                   errorNumbersToAdd: null)
                        );
                });

            builder.Services.AddMediatR(typeof(Startup));
            builder.Services.AddHttpClient(ClrHttpClient.Default);
            builder.Services.AddCommandQueryHandlers(typeof(ICommandHandler<>));
            builder.Services.AddCredentialMappingHandlers(typeof(ICredentialMapper<,>));
            builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            builder.Services.AddScoped<ICredentialMapperDispatcher, CredentialDispatcher>();
            builder.Services.AddScoped<CredentialService>();
            builder.Services.AddTransient<AgentContextService>();
            builder.Services.AddTransient<ConnectionRequestService>();
            builder.Services.AddTransient<CredentialDefinitionService>();
            builder.Services.AddTransient<CredentialPackageService>();
            builder.Services.AddTransient<CredentialRequestService>();
            
            builder.Services.AddTransient<CredentialSchemaService>();
            builder.Services.AddTransient<LinkService>();
            builder.Services.AddTransient<IQueueService, AzureQueueService>();
            builder.Services.AddTransient<RevocationDocumentService>();
            builder.Services.AddTransient<SchemaService>();
            builder.Services.AddTransient<WalletRelationshipService>();

            var verityOptions = verityConfig.Get<VerityOptions>();
            if (verityOptions.UseVerityApi)
            {
                builder.Services.AddTransient<IVerityIntegrationService, VerityApiService>();
                builder.Services.AddTransient<VerityRestApi.Client.Configuration>((services) => {
                    var agentContextModel = JsonSerializer.Deserialize<AgentContextModel>(verityOptions.Token);
                    return new VerityRestApi.Client.Configuration(
                        new Dictionary<string, string>(),
                        new Dictionary<string, string> { { VerityRestApi.Constants.ApiKeyHeader, agentContextModel.ApiKey } },
                        new Dictionary<string, string>(),
                        verityOptions.EndpointUrl
                        );  
                });

                builder.Services.AddTransient<IIssueCredentialApi, IssueCredentialApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new IssueCredentialApi(configuration);
                });
                builder.Services.AddTransient<IIssuerSetupApi, IssuerSetupApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new IssuerSetupApi(configuration);
                });
                builder.Services.AddTransient<IRelationshipApi, RelationshipApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new RelationshipApi(configuration);
                });
                builder.Services.AddTransient<IUpdateConfigsApi, UpdateConfigsApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new UpdateConfigsApi(configuration);
                });
                builder.Services.AddTransient<IUpdateEndpointApi, UpdateEndpointApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new UpdateEndpointApi(configuration);
                });
                builder.Services.AddTransient<IWriteCredDefApi, WriteCredDefApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new WriteCredDefApi(configuration);
                });
                builder.Services.AddTransient<IWriteSchemaApi, WriteSchemaApi>((services) => {
                    var configuration = services.GetService<VerityRestApi.Client.Configuration>();
                    return new WriteSchemaApi(configuration);
                });

            }
            else
            {
                builder.Services.AddTransient<IVerityIntegrationService, VeritySdkService>();
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
