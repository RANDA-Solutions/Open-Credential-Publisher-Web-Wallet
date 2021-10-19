using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.VerityFunctionApp
{
    class Program
    {
        readonly static string _storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage") ?? string.Empty;
        public static IUrlHelper _urlHelper;
        static async Task Main(string[] args)
        {

            //This is used only for creating the mvc UrlHelper required by the CredentialService/SchemaService
            BuildWebHost(args);

            var host = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddCommandLine(args);
                    configurationBuilder
                        .SetBasePath(Environment.CurrentDirectory)
                       .AddJsonFile("appsettings.json", true)
                       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
                       .AddEnvironmentVariables()
                       .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
                })
                .ConfigureFunctionsWorkerDefaults((hostBuilderContext, workerApplicationBuilder) =>
                {
                    workerApplicationBuilder.UseFunctionExecutionMiddleware();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();
                    //services.AddSingleton<CloudBlobClient>(CloudStorageAccount.Parse(_storageConnectionString).CreateCloudBlobClient());
                    Startup.Configure(context, services, _urlHelper);
                })
                .Build();

            await host.RunAsync();
        }
        private static IWebHost BuildWebHost(string[] args)
        {
            var webhost = WebHost.CreateDefaultBuilder(args)
                .SniffRouteData()
                .UseStartup<FauxStartup>()
                .Build();

            _urlHelper = webhost.GetStaticUrlHelper("https://localhost:44392/credentials");
            return webhost;
        }
    }
}
