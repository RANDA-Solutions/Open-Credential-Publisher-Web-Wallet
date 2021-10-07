using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Constants;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.Utils;
using OpenCredentialPublisher.Services.Implementations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataUtility
{
    class Program
    {
        private static bool _keepGoing = true;
        private static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static IConfiguration _config;
        private static ServiceProvider _provider;
        private static CredentialService _credentialService;
        private static ETLService _etlService;
        private static SchemaService _schemaService;
        private static  WalletDbContext _context;
        private static DataTasks _dataTasks;
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

            #region OldDataTasks
            /*
            //2021-07-20ish
            await _dataTasks.ExtractAssertionsAsync();
            */
            #endregion

            //2021-08-04
            await _dataTasks.PopulateAssertionNamesAsync();

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
                .MinimumLevel.Error()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            var keyVaultSection = _config.GetSection(nameof(KeyVaultOptions));
            services.Configure<KeyVaultOptions>(keyVaultSection);
            var keyVaultOptions = keyVaultSection.Get<KeyVaultOptions>();

            // Add access to generic IConfiguration
            services.AddSingleton(_config);

            services.AddSingleton(_urlHelper);

            services.AddHttpContextAccessor();
            services.AddTransient<SchemaService>();
            services.AddTransient<ETLService>();
            services.AddTransient<CredentialService>();
            services.AddTransient<DataTasks>();

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
            _dataTasks = _provider.GetService<DataTasks>();
            _schemaService = _provider.GetService<SchemaService>();
            _etlService = _provider.GetService<ETLService>();
            _credentialService = _provider.GetService<CredentialService>();
            _context = _provider.GetService<WalletDbContext>();
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
