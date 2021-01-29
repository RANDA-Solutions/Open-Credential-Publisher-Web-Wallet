using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Data.Contexts
{
    public class WalletDbContext : IdentityDbContext<ApplicationUser>
    {
        public WalletDbContext(DbContextOptions<WalletDbContext> options)
            : base(options)
        {}

        /// <summary>
        /// OAuth 2.0 information for accessing resources.
        /// </summary>
        public DbSet<AuthorizationModel> Authorizations { get; set; }

        public DbSet<CertificateModel> Certificates { get; set; }

        /// <summary>
        /// CLRs that have been retrieved by application users.
        /// </summary>
        public DbSet<ClrModel> Clrs { get; set; }

        public DbSet<ClrSetModel> ClrSets { get; set; }
        public DbSet<CredentialPackageModel> CredentialPackages { get; set; }

        public DbSet<IdentityCertificateModel> IdentityCertificates { get; set; }




        /// <summary>
        /// Links that have been created by application users.
        /// </summary>
        public DbSet<LinkModel> Links { get; set; }

        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<RecipientModel> Recipients { get; set; }
        public DbSet<ShareModel> Shares { get; set; }

        /// <summary>
        /// Known resource servers.
        /// </summary>
        public DbSet<SourceModel> Sources { get; set; }

        public DbSet<VerifiableCredentialModel> VerifiableCredentials { get; set; }

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
                .IsRequired();

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

            modelBuilder.Entity<VerifiableCredentialModel>().Ignore(model => model.VerifiableCredential);

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
                .Property(s => s.ShareTypeId)
                .HasConversion<int>();

            modelBuilder.Entity<MessageModel>()
                .Property(m => m.StatusId)
                .HasConversion<int>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
