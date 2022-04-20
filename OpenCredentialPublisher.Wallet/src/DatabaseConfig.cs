using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Interfaces;
using OpenCredentialPublisher.Shared.Models;
using System;
using System.Linq;
using System.Text.Json;

namespace OpenCredentialPublisher.ClrWallet
{
    public static class DatabaseConfig
    {

        public static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var walletDbContext = serviceScope.ServiceProvider.GetRequiredService<WalletDbContext>();
                walletDbContext.Database.Migrate();

                EnsureCredentialSchema(serviceScope.ServiceProvider, walletDbContext);

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

        public static void EnsureCredentialSchema(IServiceProvider serviceProvider, WalletDbContext context)
        {
            var schemaService = serviceProvider.GetRequiredService<CredentialSchemaService>();

            var credentialType = (ICredential)Activator.CreateInstance(typeof(ClrShareCredential));
            var schemaName = credentialType.GetSchemaName();
            var schemaArray = credentialType.ToSchemaArray();
            var schemaHash = CredentialSchemaService.SchemaHash(schemaArray);

            var credentialSchema = schemaService.GetCredentialSchemaAsync(schemaName, schemaHash).Result;
            if (credentialSchema == null)
            {
                credentialSchema = new CredentialSchema
                {
                    TypeName = typeof(ClrShareCredential).AssemblyQualifiedName,
                    Name = schemaName,
                    Hash = schemaHash,
                    Attributes = JsonSerializer.Serialize(schemaArray),
                    Version = "1.0",
                    StatusId = Data.Models.Enums.StatusEnum.Created,
                    SchemaId = "LAerk6JB8KMZSWAsrPYrFz:2:CLR URL:1.0",
                    ThreadId = "27448195-10ab-4a5c-a994-3ea14f4edcae",
                    CreatedAt = DateTime.UtcNow,
                };

                context.CredentialSchemas.Add(credentialSchema);
            }

            credentialType = (ICredential)Activator.CreateInstance(typeof(ClrAttachmentCredential));
            schemaName = credentialType.GetSchemaName();
            schemaArray = credentialType.ToSchemaArray();
            schemaHash = CredentialSchemaService.SchemaHash(schemaArray);

            credentialSchema = schemaService.GetCredentialSchemaAsync(schemaName, schemaHash).Result;
            if (credentialSchema == null)
            {
                credentialSchema = new CredentialSchema
                {
                    TypeName = typeof(ClrAttachmentCredential).AssemblyQualifiedName,
                    Name = schemaName,
                    Hash = schemaHash,
                    Attributes = JsonSerializer.Serialize(schemaArray),
                    Version = "1.0",
                    StatusId = Data.Models.Enums.StatusEnum.Created,
                    SchemaId = "LAerk6JB8KMZSWAsrPYrFz:2:CLR Machine Readable Transcript:1.0",
                    ThreadId = "cdca17d8-de3f-417b-b9bd-a38f04cb42a9",
                    CreatedAt = DateTime.UtcNow,
                };

                context.CredentialSchemas.Add(credentialSchema);
            }

            credentialType = (ICredential)Activator.CreateInstance(typeof(ClrWithPdfCredential));
            schemaName = credentialType.GetSchemaName();
            schemaArray = credentialType.ToSchemaArray();
            schemaHash = CredentialSchemaService.SchemaHash(schemaArray);

            credentialSchema = schemaService.GetCredentialSchemaAsync(schemaName, schemaHash).Result;
            if (credentialSchema == null)
            {
                credentialSchema = new CredentialSchema
                {
                    TypeName = typeof(ClrWithPdfCredential).AssemblyQualifiedName,
                    Name = schemaName,
                    Hash = schemaHash,
                    Attributes = JsonSerializer.Serialize(schemaArray),
                    Version = "1.1",
                    StatusId = Data.Models.Enums.StatusEnum.Created,
                    SchemaId = "LAerk6JB8KMZSWAsrPYrFz:2:CLR Transcript PDF:1.1",
                    ThreadId = "bcb64e16-7e1a-4386-8d78-18e144e70334",
                    CreatedAt = DateTime.UtcNow,
                };

                context.CredentialSchemas.Add(credentialSchema);
            }

            credentialType = (ICredential)Activator.CreateInstance(typeof(EmailVerificationCredential));
            schemaName = credentialType.GetSchemaName();
            schemaArray = credentialType.ToSchemaArray();
            schemaHash = CredentialSchemaService.SchemaHash(schemaArray);

            credentialSchema = schemaService.GetCredentialSchemaAsync(schemaName, schemaHash).Result;
            if (credentialSchema == null)
            {
                credentialSchema = new CredentialSchema
                {
                    TypeName = typeof(EmailVerificationCredential).AssemblyQualifiedName,
                    Name = schemaName,
                    Hash = schemaHash,
                    Attributes = JsonSerializer.Serialize(schemaArray),
                    Version = "1.0",
                    StatusId = Data.Models.Enums.StatusEnum.Created,
                    SchemaId = "BGdzXXtjonsfxLTWXwmahH:2:Email Verification Credential:1.0",
                    ThreadId = "82125374-a5a8-41e2-9e2d-1248e77bfe4d",
                    NetworkId = "sovrin-stagingnet",
                    CreatedAt = DateTime.UtcNow,
                };

                context.CredentialSchemas.Add(credentialSchema);
            }

            context.SaveChanges();
        }
    }
}