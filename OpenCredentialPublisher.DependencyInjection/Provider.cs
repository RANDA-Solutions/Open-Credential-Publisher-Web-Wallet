using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.Services.Implementations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace OpenCredentialPublisher.DependencyInjection
{
    public static class Provider
    {
        public static IHostBuilder GetHost()
            => Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => GetServiceCollection(context, services));
                

        public static IServiceCollection GetServiceCollection(HostBuilderContext context, IServiceCollection services = null)
        {
               
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            services ??= new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName);
            services.AddMemoryCache();


            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<WalletDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sql => {
                        sql.EnableRetryOnFailure(5);
                    });

            });


            services.AddSingleton<HostSettings>(sp => context.Configuration.GetSection(nameof(HostSettings)).Get<HostSettings>());
            services.AddSingleton<MailSettings>(sp => context.Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>());
            services.AddSingleton<ISigningCredentialStore, IdentityCertificateService>();
            services.AddSingleton<IValidationKeysStore, IdentityCertificateService>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<SchemaService>();
            services.AddTransient<ClrService>();
            services.AddTransient<CredentialService>();
            services.AddTransient<CredentialPackageService>();
            services.AddTransient<ConnectService>();

            return services;
        }
    }
}
