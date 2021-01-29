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
            services.AddSingleton<ISigningCredentialStore, IdentityCertificateService>();
            services.AddSingleton<IValidationKeysStore, IdentityCertificateService>();
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<SchemaService>();
            services.AddTransient<ClrService>();
            services.AddTransient<CredentialService>();
            services.AddTransient<CredentialPackageService>();
            services.AddTransient<ConnectService>();

            services.AddIdentity<ApplicationUser, IdentityRole>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<WalletDbContext>()
                .AddDefaultTokenProviders();

            //services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, AdditionalUserClaimsPrincipalFactory>();

            var builder = services.AddIdentityServer()
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

            //if (Environment.IsDevelopment())
            //    builder.AddDeveloperSigningCredential();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name =
                        $"{CookieAuthenticationDefaults.CookiePrefix}OpenCredentialPublisher.ClrWallet";
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                });
                
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
            app.UseCookiePolicy();
            app.UseCors();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
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

    public static class ClrHttpClient
    {
        public static string Default = "default";
    }

    public static class Configuration
    {
        /// <summary>
        /// </summary>
        public static string BasePath = "BasePath";
    }


}
