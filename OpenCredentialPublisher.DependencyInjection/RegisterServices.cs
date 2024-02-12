using Infotekka.ND.IdRampAPI;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.DependencyInjection
{
    public static class RegisterServices
    {
        public static IServiceCollection AppServiceRegistration(IServiceCollection services = null)
        {
            CommonServices(services);

            services.AddTransient<AuthorizationsService>();
            services.AddTransient<AzLoginProofService>();
            services.AddTransient<AzureListenerService>();
            services.AddTransient<BadgrService>();
            services.AddTransient<OpenBadge2dot1Service>();
            services.AddTransient<ClrDetailService>();
            services.AddTransient<ClrService>();
            services.AddTransient<ClrsService>();
            services.AddTransient<ConnectService>();
            
            services.AddTransient<CredentialService>();
            services.AddTransient<DownloadService>();
            services.AddTransient<EmailHelperService>();
            services.AddTransient<EmailService>();
            services.AddTransient<EmailVerificationService>();
            services.AddTransient<IdRampApiService>();
            services.AddTransient<LogHttpClientService>();
            services.AddTransient<LoginLinkService>();
            services.AddTransient<LoginProofService>();
            services.AddTransient<ProfileImageService>();
            services.AddTransient<RecipientService>();
            services.AddTransient<RevocationService>();
            services.AddTransient<SearchService>();
            services.AddTransient<ForgetMeService>();
            return services;
        }

        public static IServiceCollection FunctionsAppRegistration(IServiceCollection services = null)
        {
            CommonServices(services);

            services.AddScoped<CredentialService>();
            
            return services;
        }

        private static IServiceCollection CommonServices(IServiceCollection services = null)
        {
            services.AddTransient<AgentContextService>();
            services.AddTransient<AzureBlobStoreService>();

            services.AddTransient<CredentialDefinitionService>();
            services.AddTransient<CredentialPackageService>();
            services.AddTransient<CredentialRequestService>();
            services.AddTransient<CredentialSchemaService>();
            services.AddTransient<ETLService>();
            services.AddTransient<IdatafyService>();
            services.AddTransient<LinkService>();
            services.AddTransient<IQueueService, AzureQueueService>();
            services.AddTransient<RevocationDocumentService>();
            services.AddTransient<SchemaService>();
            services.AddTransient<VerityThreadService>();

            return services;
        }
    }
}
