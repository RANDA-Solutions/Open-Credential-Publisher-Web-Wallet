using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Stores;
using Infotekka.ND.IdRampAPI;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.DependencyInjection;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using OpenCredentialPublisher.Services.SignalR;
using OpenCredentialPublisher.Wallet;
using OpenCredentialPublisher.Wallet.Auth.Stores;
using OpenCredentialPublisher.Wallet.Models.Account;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
            services.Configure<SourcesSettingsOptions>(Configuration.GetSection(SourcesSettingsOptions.Section));
            services.Configure<AzureQueueOptions>(Configuration.GetSection(AzureQueueOptions.Section));
            services.Configure<AzureListenerOptions>(Configuration.GetSection(AzureListenerOptions.Section));
            services.Configure<IdRampApiOptions>(Configuration.GetSection(IdRampApiOptions.Section));

            var siteSettingsSection = Configuration.GetSection(SiteSettingsOptions.Section);
            services.Configure<SiteSettingsOptions>(siteSettingsSection);
            var siteSettingsOptions = siteSettingsSection.Get<SiteSettingsOptions>();
            Config.SpaClientUrl = siteSettingsOptions.SpaClientUrl;
            Config.AccessTokenLifetime = siteSettingsOptions.AccessTokenLifetime;
            Config.UseSlidingSessionExpiration = siteSettingsOptions.SlidingSessionExpiration;
            Config.SessionTimeout = siteSettingsOptions.SessionTimeout;

            services.Configure<VerityOptions>(Configuration.GetSection(VerityOptions.Section));
            services.Configure<HostSettings>(Configuration.GetSection(nameof(HostSettings)));
            var keyVaultSection = Configuration.GetSection(nameof(KeyVaultOptions));
            services.Configure<KeyVaultOptions>(keyVaultSection);
            var keyVaultOptions = keyVaultSection.Get<KeyVaultOptions>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
            
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                { 
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        //object responseObject = context.ModelState.Select(entry => entry.Value.Errors.Select(error => error.ErrorMessage)).Aggregate(Enumerable.Empty<string>(), (agg, val) => agg.Concat(val));
                        //return new BadRequestObjectResult(responseObject);
                        return new OkObjectResult(new ApiBadRequestResponse(context.ModelState));
                    };
                });

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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(WalletDbContext).Assembly.GetName().Name;

            services.AddDbContext<WalletDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sql => {
                        sql.EnableRetryOnFailure(5);
                     });
            });

            if (Environment.IsDevelopment())
                services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddSingleton<HostSettings>(sp => Configuration.GetSection(nameof(HostSettings)).Get<HostSettings>());
            services.AddSingleton<MailSettings>(sp => Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>());

            services.AddTransient<IEmailSender, EmailService>();
            services.AddSingleton<ISigningCredentialStore, IdentityCertificateService>();
            services.AddSingleton<IValidationKeysStore, IdentityCertificateService>();

            RegisterServices.AppServiceRegistration(services);

            services.AddCors(options => CorsConfig.CorsOptions(options, siteSettingsOptions));

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
                //options.UserInteraction.LoginUrl = $"{siteSettingsOptions.SpaClientUrl}/access/login";
                //options.UserInteraction.LogoutUrl = $"{siteSettingsOptions.SpaClientUrl}/access/logout";
                //options.UserInteraction.ErrorUrl = $"{siteSettingsOptions.SpaClientUrl}/error";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddApiAuthorization<ApplicationUser, WalletDbContext>(options =>
            {
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
            });
            

            if (Environment.IsDevelopmentOrLocalhost())
                builder.AddDeveloperSigningCredential();
            else
            {
                var keyVaultName = Configuration["KeyVaultName"];

                var certificateClient = new CertificateClient(new Uri($"https://{keyVaultOptions.KeyVaultName}.vault.azure.net/"), new DefaultAzureCredential());
                var azureResponse = certificateClient.DownloadCertificate(keyVaultOptions.KeyVaultCertificateName);
                builder.AddSigningCredential(azureResponse.Value);
            }


            var authenticationBuilder =
                services.AddAuthentication()
                    .AddIdentityServerJwt();
            

            services.Configure<JwtBearerOptions>(IdentityServerJwtConstants.IdentityServerJwtBearerScheme, options => {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if ((context.Request.Path.Value.StartsWith("/hubs")
                           )
                            && context.Request.Headers.TryGetValue("Bearer", out StringValues token)
                        )
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var te = context.Exception;
                        return Task.CompletedTask;
                    }
                };
            }
            );
            services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromSeconds(siteSettingsOptions.SessionTimeout);
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Unspecified;
                options.LoginPath = "/access/login";
                options.LogoutPath = "/access/logout";
                options.AccessDeniedPath = "/error";
            });

            var externalProvidersOptions = Configuration.GetSection(ExternalProvidersOptions.Section).Get<ExternalProvidersOptions>();

            if (externalProvidersOptions?.Configurations != null)
            {

                foreach (var configuration in externalProvidersOptions.Configurations)
                {
                    services.AddOidcStateDataFormatterCache(configuration.AuthenticationScheme);

                    authenticationBuilder.AddOpenIdConnect(configuration.AuthenticationScheme, configuration.DisplayName, options =>
                    {
                        options.Authority = configuration.AuthorityUrl;
                        options.ClientId = configuration.ClientId;
                        options.ClientSecret = configuration.ClientSecret;
                        options.ResponseType = configuration.ResponseType;
                        options.CallbackPath = "/public/account/login/callback";
                        options.SaveTokens = configuration.SaveTokens;
                        
                        options.RequireHttpsMetadata = configuration.RequireHttpsMetadata;
                        if (!string.IsNullOrEmpty(configuration.MetadataUrl))
                            options.MetadataAddress = configuration.MetadataUrl;
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                        var scopes = configuration.Scopes.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        foreach (var scope in scopes)
                            options.Scope.Add(scope);
                        options.GetClaimsFromUserInfoEndpoint = configuration.GetClaimsFromUserInfoEndpoint;

                        if (configuration.GetClaimsFromUserInfoEndpoint ||
                            !String.IsNullOrEmpty(configuration.TokenEndpointUrl) ||
                            !String.IsNullOrEmpty(configuration.AuthorizationEndpointUrl) ||
                            !String.IsNullOrEmpty(configuration.JwksUri))
                        {
                            options.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration();
                            if (!string.IsNullOrEmpty(configuration.JwksUri))
                                options.Configuration.JwksUri = configuration.JwksUri;
                            if (configuration.GetClaimsFromUserInfoEndpoint)
                                options.Configuration.UserInfoEndpoint = configuration.UserInfoEndpointUrl;
                            if (!String.IsNullOrEmpty(configuration.TokenEndpointUrl))
                                options.Configuration.TokenEndpoint = configuration.TokenEndpointUrl;
                            if (!String.IsNullOrEmpty(configuration.AuthorizationEndpointUrl))
                                options.Configuration.AuthorizationEndpoint = configuration.AuthorizationEndpointUrl;
                        }

                        options.Events.OnRedirectToIdentityProvider = async context =>
                        {
                            if (!string.IsNullOrEmpty(configuration.ReturnUrlParameter))
                            {
                                var returnUrl = context.ProtocolMessage.BuildRedirectUrl();
                                context.ProtocolMessage.RemoveParameter("redirect_uri");
                                context.ProtocolMessage.SetParameter(configuration.ReturnUrlParameter, returnUrl);
                            }
                            await Task.FromResult(0);
                        };
                    });
                }
            }

            services.AddControllers();// WithViews();

            // In production, the Angular files will be served from this directory
            if (!Environment.IsDevelopment())
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = "ClientApp/dist";
                });
            services.AddSignalR();
            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            InitializeDatabase(app);
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None
            });
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

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(CorsConfig.PolicyName);

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            if (!Environment.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ConnectionStatusHub>(ConnectionStatusHub.Endpoint);
                endpoints.MapHub<CredentialStatusHub>(CredentialStatusHub.Endpoint);
                endpoints.MapHub<ProofRequestStatusHub>(ProofRequestStatusHub.Endpoint);
            });
            app.UseWhen(x => !x.Request.Path.Value.Contains("/api/") && !x.Request.Path.Value.Contains("/hubs/"), app1 =>
             app1.UseSpa(spa =>
             {
                 spa.Options.SourcePath = "ClientApp";
                 if (Environment.IsDevelopment())
                     spa.UseProxyToSpaDevelopmentServer("https://localhost:4200");
             })
            );
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var walletDbContext = serviceScope.ServiceProvider.GetRequiredService<WalletDbContext>();
                walletDbContext.Database.Migrate();

                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ClientToEntity());
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

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                        context.SaveChanges();
                    }
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
