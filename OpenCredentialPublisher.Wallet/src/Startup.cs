using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Stores;
using IdentityServer4.EntityFramework.DbContexts;
using System.Linq;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity.UI.Services;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Services.SignalR;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using IdentityServer4;

namespace OpenCredentialPublisher.ClrWallet
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AzureBlobOptions>(Configuration.GetSection(AzureBlobOptions.Section));
            services.Configure<AzureQueueOptions>(Configuration.GetSection(AzureQueueOptions.Section));
            services.Configure<AzureListenerOptions>(Configuration.GetSection(AzureListenerOptions.Section));
            services.Configure<SiteSettingsOptions>(Configuration.GetSection(SiteSettingsOptions.Section));
            services.Configure<VerityOptions>(Configuration.GetSection(VerityOptions.Section));

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            var verityOptions = Configuration.GetSection(VerityOptions.Section).Get<VerityOptions>();

            if (verityOptions.UseAzureListener)
            {
                services.AddHostedService<VerityResponseBackgroundService>();
            }

            services.AddHostedService<VerityIssuerSetupBackgroundService>();

            if (Environment.IsDevelopmentOrLocalhost())
            {
                // Accept any server certificate

                services.AddHttpClient(ClrHttpClient.Default, c => { })
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    });
            }
            else
            {
                services.AddHttpClient(ClrHttpClient.Default);
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(WalletDbContext).Assembly.GetName().Name;

            services.AddDbContext<WalletDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sql => {
                        sql.MigrationsAssembly(migrationsAssembly);
                        sql.EnableRetryOnFailure(5);
                    });

            });
            services.AddMemoryCache();


            services.AddSingleton<HostSettings>(sp => Configuration.GetSection(nameof(HostSettings)).Get<HostSettings>());
            services.AddSingleton<MailSettings>(sp => Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>());

            services.AddTransient<IEmailSender, EmailService>();
            services.AddSingleton<ISigningCredentialStore, IdentityCertificateService>();
            services.AddSingleton<IValidationKeysStore, IdentityCertificateService>();

            services.AddTransient<AuthorizationsService>();
            services.AddTransient<AzureListenerService>();
            services.AddTransient<ClrService>();
            services.AddTransient<CredentialPackageService>();
            services.AddTransient<CredentialService>();
            services.AddTransient<ConnectService>();
            services.AddTransient<ConnectionRequestService>();
            services.AddTransient<CredentialDefinitionService>();
            services.AddTransient<CredentialRequestService>();
            services.AddTransient<CredentialSchemaService>();
            services.AddTransient<EmailHelperService>();
            services.AddTransient<LinkService>();
            services.AddTransient<LogHttpClientService>();
            services.AddTransient<BadgrService>();
            services.AddTransient<ProfileImageService>();
            services.AddTransient<RevocationDocumentService>();
            services.AddTransient<RevocationService>();
            services.AddTransient<IQueueService, AzureQueueService>();
            services.AddTransient<SchemaService>();
            services.AddTransient<WalletRelationshipService>();

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(
                    options => {
                        options.SignIn.RequireConfirmedAccount = true;
                    })
                .AddEntityFrameworkStores<WalletDbContext>()
                .AddDefaultTokenProviders();


            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Account/Login";
                options.UserInteraction.LogoutUrl = "/Account/Logout";
            })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddAspNetIdentity<ApplicationUser>();
            var siteSettings = Configuration.GetSection(SiteSettingsOptions.Section).Get<SiteSettingsOptions>();

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/");
                options.Conventions.AllowAnonymousToPage("/Privacy");
                options.Conventions.AllowAnonymousToPage("/ReleaseNotes");
                options.Conventions.AllowAnonymousToPage("/Terms");
                options.Conventions.AllowAnonymousToFolder("/Account");
                options.Conventions.AllowAnonymousToFolder("/Links");
            })
                .AddRazorRuntimeCompilation();

            services.AddControllers();

            services.AddSignalR();
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);

            app.UseForwardedHeaders();
            var basePath = Configuration[OpenCredentialPublisher.ClrWallet.Configuration.BasePath];
            if (!string.IsNullOrEmpty(basePath))
            {
                Log.Debug($"Found base path '{basePath}'.");

                app.UsePathBase(basePath);
                app.Use((context, next) =>
                {
                    if (string.IsNullOrEmpty(context.Request.PathBase))
                    {
                        context.Request.PathBase = new PathString(basePath);
                    }

                    return next();
                });
            }

            if (Environment.IsDevelopmentOrLocalhost())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
                endpoints.MapHub<ConnectionStatusHub>(ConnectionStatusHub.Endpoint);
                endpoints.MapHub<CredentialStatusHub>(CredentialStatusHub.Endpoint);
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<WalletDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.ApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }

    public static class WebHostExtensions
    {
        public static bool IsDevelopmentOrLocalhost(this IHostEnvironment environment)
        {
            return environment.IsDevelopment() || environment.IsEnvironment(Constants.Localhost);
        }
    }

    public static class Configuration
    {
        /// <summary>
        /// </summary>
        public static string BasePath = "BasePath";
    }


}
