using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;

namespace OpenCredentialPublisher.Data.Contexts
{
    public class WalletDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {
        }


        public DbSet<AgentContextModel> AgentContexts { get; set; }

        /// <summary>
        /// OAuth 2.0 information for accessing resources.
        /// </summary>
        public DbSet<AuthorizationModel> Authorizations { get; set; }


        public DbSet<CertificateModel> Certificates { get; set; }

        /// <summary>
        /// CLRs that have been retrieved by application users.
        /// </summary>
        public DbSet<ClrModel> Clrs { get; set; }
        public DbSet<BadgrBackpackModel> BadgrBackpacks{ get; set; }
        public DbSet<BadgrAssertionModel> BadgrAssertions { get; set; }
        public DbSet<ClrSetModel> ClrSets { get; set; }

        public DbSet<ConnectionRequestModel> ConnectionRequests { get; set; }
        public DbSet<CredentialDefinition> CredentialDefinitions { get; set; }
        public DbSet<CredentialPackageModel> CredentialPackages { get; set; }
        public DbSet<CredentialRequestModel> CredentialRequests { get; set; }
        public DbSet<CredentialSchema> CredentialSchemas { get; set; }

        public DbSet<IdentityCertificateModel> IdentityCertificates { get; set; }

        /// <summary>
        /// Links that have been created by application users.
        /// </summary>
        public DbSet<LinkModel> Links { get; set; }

        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<ProvisioningTokenModel> ProvisioningTokens { get; set; }
        public DbSet<RecipientModel> Recipients { get; set; }
        public DbSet<RevocationModel> Revocations { get; set; }
        public DbSet<ShareModel> Shares { get; set; }

        /// <summary>
        /// Known resource servers.
        /// </summary>
        public DbSet<SourceModel> Sources { get; set; }

        public DbSet<VerifiableCredentialModel> VerifiableCredentials { get; set; }

        /// <summary>
        /// Log for HttpClient calls
        /// </summary>
        public DbSet<HttpClientLog> HttpClientLogs { get; set; }

        public DbSet<WalletRelationshipModel> WalletRelationships { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiscoveryDocumentModel>()
                .Property(x => x.ScopesOffered)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<AuthorizationModel>()
                .Property(x => x.Scopes)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<BadgrAssertionModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder.Entity<IdentityDType>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder.Entity<ClrModel>()
                .HasMany(clrModel => clrModel.Links)
                .WithOne(linkModel => linkModel.Clr)
                .HasForeignKey(linkModel => linkModel.ClrForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SourceModel>()
                .HasMany(sourceModel => sourceModel.Authorizations)
                .WithOne(authorizationModel => authorizationModel.Source)
                .HasForeignKey(authorizationModel => authorizationModel.SourceForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuthorizationModel>()
                .HasMany(authorizationModel => authorizationModel.Clrs)
                .WithOne(clrModel => clrModel.Authorization)
                .HasForeignKey(clrModel => clrModel.AuthorizationForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuthorizationModel>()
               .HasMany(authorizationModel => authorizationModel.CredentialPackages)
               .WithOne(pkg => pkg.Authorization)
               .HasForeignKey(pkg => pkg.AuthorizationForeignKey)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SourceModel>()
                .HasOne(sourceModel => sourceModel.DiscoveryDocument)
                .WithOne(documentModel => documentModel.Source)
                .HasForeignKey<DiscoveryDocumentModel>(documentModel => documentModel.SourceForeignKey)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CredentialPackageModel>()
                .Property(model => model.TypeId)
                .HasConversion<int>();

            
            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.Clr)
                .WithOne(c => c.CredentialPackage)
                .HasForeignKey<ClrModel>(c => c.CredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.ClrSet)
                .WithOne(c => c.CredentialPackage)
                .HasForeignKey<ClrSetModel>(c => c.CredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.VerifiableCredential)
                .WithOne(c => c.CredentialPackage)
                .HasForeignKey<VerifiableCredentialModel>(c => c.CredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.BadgrBackpack)
                .WithOne(c => c.CredentialPackage)
                .HasForeignKey<BadgrBackpackModel>(c => c.CredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.Authorization)
                .WithMany(a => a.CredentialPackages)
                .HasForeignKey(c => c.AuthorizationForeignKey)
                .IsRequired(false);

            modelBuilder.Entity<VerifiableCredentialModel>()
                .HasMany(c => c.ClrSets)
                .WithOne(c => c.VerifiableCredential)
                .HasForeignKey("VerifiableCredentialId")
                .IsRequired(false);

            modelBuilder.Entity<VerifiableCredentialModel>()
                .HasMany(c => c.Clrs)
                .WithOne(c => c.VerifiableCredential)
                .HasForeignKey("VerifiableCredentialId")
                .IsRequired(false);

            modelBuilder.Entity<ClrSetModel>()
                .HasMany(c => c.Clrs)
                .WithOne(c => c.ClrSet)
                .HasForeignKey("ClrSetId")
                .IsRequired(false);

            modelBuilder.Entity<BadgrAssertionModel>()
               .HasOne(c => c.BadgrBackpack)
               .WithMany(a => a.BadgrAssertions)
               .HasForeignKey(a => a.BadgrBackpackId)
               .IsRequired(true);

            modelBuilder.Entity<RevocationModel>()
                .HasOne(r => r.Source)
                .WithMany(s => s.Revocations)
                .HasForeignKey(s => s.SourceId);

            modelBuilder.Entity<StatusModel>()
                .Property(s => s.Id)
                .HasConversion<int>();

            modelBuilder.Entity<StatusModel>()
                .HasData(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().Select(s => new StatusModel { Id = s, Name = s.ToString() }));

            modelBuilder.Entity<ShareTypeModel>()
                .Property(s => s.Id)
                .HasConversion<int>();

            modelBuilder.Entity<ShareTypeModel>()
                .HasData(Enum.GetValues(typeof(ShareTypeEnum)).Cast<ShareTypeEnum>().Select(s => new ShareTypeModel { Id = s, Name = s.ToString() }));

            modelBuilder.Entity<ShareModel>()
                .Property(s => s.StatusId)
                .HasConversion<int>();

            modelBuilder.Entity<ShareModel>()
               .Property(s => s.StatusId)
               .HasConversion<int>();

            modelBuilder.Entity<SourceModel>()
                .Property(s => s.SourceTypeId)
                .HasConversion<int>();

            modelBuilder.Entity<MessageModel>()
                .Property(m => m.StatusId)
                .HasConversion<int>();

            modelBuilder.Entity<CredentialSchema>()
                .Property(m => m.StatusId)
                .HasConversion<int>();

            modelBuilder.Entity<CredentialDefinition>()
                .Property(m => m.StatusId)
                .HasConversion<int>();

            modelBuilder.Entity<ConnectionRequestStep>()
                .Property(s => s.Id)
                .HasConversion<int>();

            modelBuilder.Entity<ConnectionRequestStep>()
                .HasData(Enum.GetValues(typeof(ConnectionRequestStepEnum)).Cast<ConnectionRequestStepEnum>().Select(s => new ConnectionRequestStep { Id = s, Name = s.ToString() }));

            modelBuilder.Entity<CredentialRequestStep>()
                .Property(s => s.Id)
                .HasConversion<int>();

            modelBuilder.Entity<CredentialRequestStep>()
                .HasData(Enum.GetValues(typeof(CredentialRequestStepEnum)).Cast<CredentialRequestStepEnum>().Select(s => new CredentialRequestStep { Id = s, Name = s.ToString() }));

            modelBuilder.Entity<ConnectionRequestModel>()
                .HasIndex(w => w.ThreadId)
                .IsUnique();

            modelBuilder.Entity<ConnectionRequestModel>()
                .Property(w => w.ConnectionRequestStep)
                .HasConversion<int>();

            modelBuilder.Entity<CredentialRequestModel>()
                .Property(w => w.CredentialRequestStep)
                .HasConversion<int>();

            modelBuilder.Entity<AgentContextModel>()
                .HasOne(ac => ac.ProvisioningToken).WithOne(pt => pt.AgentContext)
                .HasForeignKey<ProvisioningTokenModel>(pt => pt.AgentContextId);

            //IsDeleted Filters, use .IgnoreQueryFilters() to include deleted items
            //modelBuilder.Entity<AuthorizationModel>().HasQueryFilter(x => !x.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }                
    }
}
