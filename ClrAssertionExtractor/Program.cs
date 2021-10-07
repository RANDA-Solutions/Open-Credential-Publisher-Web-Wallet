using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Data.Constants;
using OpenCredentialPublisher.Data.Utils;
using System;
using System.Reflection;
using Serilog;
using System.Diagnostics;
using OpenCredentialPublisher.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Implementations;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Models;
using System.Linq;
using System.Collections.Generic;
using OpenCredentialPublisher.Data.Options;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;

namespace ClrAssertionExtractor
{
    class Program
    {
        private static bool _keepGoing = true;
        private static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static IConfiguration _config;
        private static ServiceProvider _provider;
        private static CredentialService _credentialService;
        private static SchemaService _schemaService;
        private static IUrlHelper _urlHelper;
        public static async Task Main(string[] args)
        {
            /********************************************************************************************************************
             * This app extracts all legacy CLR Assertions & persists them in the database to facilitate the first interim 
             * version of the access optimized data structure. Uses secrets file for db connection
             * for use after Migration - 20210618202418_AngularConversion.cs
             ********************************************************************************************************************/
            ConsoleUtil.ConsoleLine(MethodBase.GetCurrentMethod().DeclaringType.Namespace, Configuration.ConsoleColors.Name);

            ServiceCollection services = new ServiceCollection();

            //This is used only for creating the mvc UrlHelper required by the CredentialService/SchemaService
            BuildWebHost(args);

            Init(services);

            var pkgIds = await _credentialService.GetPackageUniverseIdsAsync();
            foreach (var pkgId in pkgIds)
            {
                ConsoleUtil.ConsoleLine($"Extracting PackageId: {pkgId}", Configuration.ConsoleColors.Milestone);
                var clrs = await _credentialService.GetPackageClrsWithClrAssertionsAsync(pkgId);
                foreach (var clr in clrs.Where(c => c.ClrAssertions.Count == 0)) // if ClrAssertion recs exist, clr should already be OK
                {
                    var clrAssertions = new List<ClrAssertion>();
                    ConsoleUtil.ConsoleWrite($"Extracting CLR: {clr.Id}...", Configuration.ConsoleColors.InProgress);
                    var rawClr = CredentialsUtil.GetRawClr(clr);
                    clrAssertions.AddRange(_credentialService.GetNotPersistedClrAssertions(clr, rawClr));
                    ConsoleUtil.ConsoleWrite($"Extracted  CLR: {clr.Id} - {clrAssertions.Count} assertions", Configuration.ConsoleColors.Default);
                    ConsoleUtil.ConsoleNewLine();
                    if (clrAssertions.Count > 0)
                    {
                        await _credentialService.AddClrAssertionsAsync(clr, clrAssertions);
                        ConsoleUtil.ConsoleLine($"Saved ClrAssertions: {clrAssertions.Count} assertions", Configuration.ConsoleColors.Default);
                    }
                }
            }
            ConsoleUtil.ConsoleLine($"Completed Execution!", Configuration.ConsoleColors.Success);
        }
        private static void Init(IServiceCollection services)
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Program>()
                .Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(_config)
            .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            var keyVaultSection = _config.GetSection(nameof(KeyVaultOptions));
            services.Configure<KeyVaultOptions>(keyVaultSection);
            var keyVaultOptions = keyVaultSection.Get<KeyVaultOptions>();

            // Add access to generic IConfiguration
            services.AddSingleton(_config);

            services.AddSingleton(_urlHelper);

            services.AddTransient<SchemaService>();
            services.AddTransient<CredentialService>();

            var connectionString = _config.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(WalletDbContext).Assembly.GetName().Name;

            services.AddDbContext<WalletDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sql => {
                        sql.EnableRetryOnFailure(5);
                    });
            });

            _provider = services.BuildServiceProvider();

            _schemaService = _provider.GetService<SchemaService>();
            _credentialService = _provider.GetService<CredentialService>();
        }
        private static IWebHost BuildWebHost(string[] args)
        {
            var webhost = WebHost.CreateDefaultBuilder(args)
                .SniffRouteData()
                .UseStartup<Startup>()
                .Build();

            _urlHelper = webhost.GetStaticUrlHelper("https://localhost:44392/credentials");
            return webhost;
        }
    }
}
