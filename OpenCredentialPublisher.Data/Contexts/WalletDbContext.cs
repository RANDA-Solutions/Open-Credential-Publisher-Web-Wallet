using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Dtos.Account;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Models.Idatafy;
using OpenCredentialPublisher.Data.Models.MSProofs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.Contexts
{
    public class WalletDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public WalletDbContext(
            DbContextOptions<WalletDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
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
        public DbSet<AssertionModel> Assertions { get; set; }
        public DbSet<EvidenceModel> Evidences { get; set; }
        public DbSet<ClrAssertion> ClrAssertions { get; set; }
        public DbSet<AchievementModel> Achievements { get; set; }
        public DbSet<AlignmentModel> Alignments { get; set; }
        public DbSet<AssociationModel> Associations { get; set; }
        public DbSet<CriteriaModel> Criterias { get; set; }
        public DbSet<EndorsementModel> Endorsements { get; set; }
        public DbSet<EndorsementClaimModel> EndorsementClaims { get; set; }
        public DbSet<Models.ClrEntities.IdentityModel> Identities { get; set; }
        public DbSet<ProfileModel> Profiles { get; set; }
        public DbSet<ResultDescriptionModel> ResultDescriptions { get; set; }
        public DbSet<ResultModel> Results { get; set; }
        public DbSet<RubricCriterionLevelModel> RubricCriterionLevels { get; set; }
        public DbSet<VerificationModel> Verifications { get; set; }
        public DbSet<AchievementAlignment> AchievementAlignments { get; set; }
        public DbSet<AchievementAssociation> AchievementAssociations { get; set; }
        public DbSet<AchievementEndorsement> AchievementEndorsements { get; set; }
        public DbSet<AssertionEvidence> AssertionEvidences { get; set; }
        public DbSet<ClrAchievement> ClrAchievements { get; set; }
        public DbSet<ClrEndorsement> ClrEndorsements { get; set; }
        public DbSet<ProfileEndorsement> ProfileEndorsements { get; set; }
        public DbSet<ResultAlignment> ResultAlignments { get; set; }
        public DbSet<ResultDescriptionAlignment> ResultDescriptionAlignments { get; set; }
        public DbSet<RubricCriterionLevelAlignment> RubricCriterionLevelAlignments { get; set; }
        
        public DbSet<ArtifactModel> Artifacts { get; set; }
        public DbSet<EvidenceArtifact> EvidenceArtifacts { get; set; }

        public DbSet<BadgrBackpackModel> BadgrBackpacks{ get; set; }
        public DbSet<BadgrAssertionModel> BadgrAssertions { get; set; }
        public DbSet<ClrSetModel> ClrSets { get; set; }

        public DbSet<ConnectionRequestModel> ConnectionRequests { get; set; }
        public DbSet<CredentialDefinition> CredentialDefinitions { get; set; }
        public DbSet<CredentialPackageModel> CredentialPackages { get; set; }
        public DbSet<CredentialRequestModel> CredentialRequests { get; set; }
        public DbSet<CredentialSchema> CredentialSchemas { get; set; }

        public DbSet<EmailVerification> EmailVerifications { get; set; }

        public DbSet<IdentityCertificateModel> IdentityCertificates { get; set; }

        /// <summary>
        /// Links that have been created by application users.
        /// </summary>
        public DbSet<LinkModel> Links { get; set; }

        public DbSet<LoginProofRequest> LoginProofRequests { get; set; }
        public DbSet<AzLoginProofGetResponseModel> MSLoginProofRequests { get; set; }
        public DbSet<AzLoginProofStatusModel> MSLoginProofStatuses { get; set; }

        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<ProofRequest> ProofRequests { get; set; }
        public DbSet<ProofRequestStep> ProofRequestSteps { get; set; }
        public DbSet<ProofResponse> ProofResponses { get; set; }

        public DbSet<ProvisioningTokenModel> ProvisioningTokens { get; set; }
        public DbSet<RecipientModel> Recipients { get; set; }
        public DbSet<RevocationModel> Revocations { get; set; }
        public DbSet<ShareModel> Shares { get; set; }

        /// <summary>
        /// Known resource servers.
        /// </summary>
        public DbSet<SourceModel> Sources { get; set; }

        public DbSet<VerifiableCredentialModel> VerifiableCredentials { get; set; }
        public DbSet<VerityThread> VerityThreads { get; set; }

        /// <summary>
        /// Log for HttpClient calls
        /// </summary>
        public DbSet<HttpClientLog> HttpClientLogs { get; set; }

        public DbSet<WalletRelationshipModel> WalletRelationships { get; set; }

        #region Idatafy
        public DbSet<SmartResume> SmartResumes { get; set; }
        #endregion

        #region Views
        public DbSet<CredentialListView> CredentialListViews { get; set; }
        public DbSet<CredentialSearchView> CredentialSearchViews { get; set; }
        public DbSet<CredentialPackageArtifactView> CredentialPackageArtifactView { get; set; }
        #endregion

        private ValueComparer<List<string>> StringValueComparer() =>
            new ValueComparer<List<string>>((l, r) => l.SequenceEqual(r),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

        private ValueComparer<List<SystemIdentifierDType>> SystemIdentifierDTypeValueComparer() =>
            new ValueComparer<List<SystemIdentifierDType>>((l, r) => l.SequenceEqual(r),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, JsonSerializer.Serialize(v, null).GetHashCode())),
                c => c.ToList());

        private ValueComparer<Dictionary<string, object>> DictionaryValueComparer() =>
            new ValueComparer<Dictionary<string, object>>
            (
                (l, r) => JsonSerializer.Serialize(l, null) == JsonSerializer.Serialize(r, null),
                v => v == null ? 0 : JsonSerializer.Serialize(v, null).GetHashCode(),
                v => JsonSerializer.Deserialize<Dictionary<string, object>>(JsonSerializer.Serialize(v, null), null)
            );

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region clr schema tables
            modelBuilder.Entity<BadgrAssertionModel>().ToTable("BadgrAssertions", schema: "cred");
            modelBuilder.Entity<BadgrBackpackModel>().ToTable("BadgrBackpacks", schema: "cred");
            modelBuilder.Entity<DiscoveryDocumentModel>().ToTable("DiscoveryDocumentModel", schema: "cred");
            modelBuilder.Entity<CredentialPackageModel>().ToTable("CredentialPackages", schema: "cred");
            modelBuilder.Entity<VerifiableCredentialModel>().ToTable("VerifiableCredentials", schema: "cred");
            modelBuilder.Entity<AchievementModel>().ToTable("Achievements", schema: "cred");
            modelBuilder.Entity<AlignmentModel>().ToTable("Alignments", schema: "cred");
            modelBuilder.Entity<ArtifactModel>().ToTable("Artifacts", schema: "cred");
            modelBuilder.Entity<AssertionModel>().ToTable("Assertions", schema: "cred");
            modelBuilder.Entity<AssociationModel>().ToTable("Associations", schema: "cred");
            modelBuilder.Entity<ClrModel>().ToTable("Clrs", schema: "cred");
            modelBuilder.Entity<ClrSetModel>().ToTable("ClrSets", schema: "cred");
            modelBuilder.Entity<CriteriaModel>().ToTable("Criteria", schema: "cred");
            modelBuilder.Entity<EndorsementClaimModel>().ToTable("EndorsementClaims", schema: "cred");
            modelBuilder.Entity<EndorsementModel>().ToTable("Endorsements", schema: "cred");
            modelBuilder.Entity<EvidenceModel>().ToTable("Evidence", schema: "cred");
            modelBuilder.Entity<Models.ClrEntities.IdentityModel>().ToTable("Identities", schema: "cred");
            modelBuilder.Entity<ProfileModel>().ToTable("Profiles", schema: "cred");
            modelBuilder.Entity<ResultDescriptionModel>().ToTable("ResultDescriptions", schema: "cred");
            modelBuilder.Entity<ResultModel>().ToTable("Results", schema: "cred");
            modelBuilder.Entity<RubricCriterionLevelModel>().ToTable("RubricCriterionLevels", schema: "cred");
            modelBuilder.Entity<VerificationModel>().ToTable("Verifications", schema: "cred");

            modelBuilder.Entity<AchievementAlignment>().ToTable("AchievementAlignments", schema: "cred");
            modelBuilder.Entity<AchievementAssociation>().ToTable("AchievementAssociations", schema: "cred");
            modelBuilder.Entity<AchievementEndorsement>().ToTable("AchievementEndorsements", schema: "cred");
            modelBuilder.Entity<AssertionEndorsement>().ToTable("AssertionEndorsements", schema: "cred");
            modelBuilder.Entity<AssertionEvidence>().ToTable("AssertionEvidence", schema: "cred");
            modelBuilder.Entity<ClrAchievement>().ToTable("ClrAchievements", schema: "cred");
            modelBuilder.Entity<ClrAssertion>().ToTable("ClrAssertions", schema: "cred");
            modelBuilder.Entity<ClrEndorsement>().ToTable("ClrEndorsements", schema: "cred");
            modelBuilder.Entity<EvidenceArtifact>().ToTable("EvidenceArtifacts", schema: "cred");
            modelBuilder.Entity<ProfileEndorsement>().ToTable("ProfileEndorsements", schema: "cred");
            modelBuilder.Entity<ResultAlignment>().ToTable("ResultAlignments", schema: "cred");
            modelBuilder.Entity<ResultDescriptionAlignment>().ToTable("ResultDescriptionAlignments", schema: "cred");
            modelBuilder.Entity<RubricCriterionLevelAlignment>().ToTable("RubricCriterionLevelAlignments", schema: "cred");
            #endregion

            #region Idatafy
            modelBuilder.Entity<SmartResume>(sr =>
            {
                sr.ToTable("SmartResumes", "idatafy");
                sr.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(true);
            });

            modelBuilder.Entity<ClrModel>()
                .HasOne(clr => clr.SmartResume)
                .WithOne()
                .HasForeignKey<SmartResume>(sr => sr.ClrId);
            #endregion

            modelBuilder.Entity<DiscoveryDocumentModel>()
                .Property(x => x.ScopesOffered)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder
                .Entity<DiscoveryDocumentModel>()
                .Property(x => x.ScopesOffered)
                .Metadata
                .SetValueComparer(StringValueComparer());

            modelBuilder.Entity<AuthorizationModel>()
                .Property(x => x.Scopes)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<AuthorizationModel>()
                .Property(x => x.Scopes)
                .Metadata
                .SetValueComparer(StringValueComparer());

            modelBuilder.Entity<AchievementModel>()
                .Property(x => x.Tags)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<AchievementModel>()
                .Property(x => x.Tags)
                .Metadata
                .SetValueComparer(StringValueComparer());

            modelBuilder.Entity<ResultDescriptionModel>()
                .Property(x => x.AllowedValues)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<ResultDescriptionModel>()
                .Property(x => x.AllowedValues)
                .Metadata
                .SetValueComparer(StringValueComparer());

            modelBuilder.Entity<VerificationModel>()
                .Property(x => x.AllowedOrigins)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<VerificationModel>()
                .Property(x => x.AllowedOrigins)
                .Metadata
                .SetValueComparer(StringValueComparer());

            modelBuilder.Entity<VerificationModel>()
                .Property(x => x.StartsWith)
                .HasConversion(v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<VerificationModel>()
                .Property(x => x.StartsWith)
                .Metadata
                .SetValueComparer(StringValueComparer());

            #region AdditionalProperties
            modelBuilder.Entity<AchievementModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<AchievementModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<AchievementModel>()
               .Property(b => b.Identifiers)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<List<SystemIdentifierDType>>(v, null)
               );

            modelBuilder
                .Entity<AchievementModel>()
                .Property(b => b.Identifiers)
                .Metadata
                .SetValueComparer(SystemIdentifierDTypeValueComparer());

            modelBuilder.Entity<AlignmentModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<AlignmentModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<AssociationModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<AssociationModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<ClrModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<ClrModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<CriteriaModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<CriteriaModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<EndorsementClaimModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<EndorsementClaimModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<EndorsementModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<EndorsementModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<ResultDescriptionModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<ResultDescriptionModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<ResultModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<ResultModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<RubricCriterionLevelModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<RubricCriterionLevelModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<VerificationModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<VerificationModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<ProfileModel>()
               .Property(b => b.PublicKey)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<CryptographicKeyDType>(v, null)
               );

            // add converter

            modelBuilder.Entity<ProfileModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<ProfileModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<ProfileModel>()
               .Property(b => b.Identifiers)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<List<SystemIdentifierDType>>(v, null)
               );

            modelBuilder
                .Entity<ProfileModel>()
                .Property(b => b.Identifiers)
                .Metadata
                .SetValueComparer(SystemIdentifierDTypeValueComparer());
            // add list converter

            modelBuilder.Entity<Models.ClrEntities.IdentityModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<Models.ClrEntities.IdentityModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<BadgrAssertionModel>()
               .Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

            modelBuilder
                .Entity<BadgrAssertionModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<AssertionModel>()
                .Property(x => x.SignedEndorsements)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null)
                );

            modelBuilder
                .Entity<AssertionModel>()
                .Property(x => x.SignedEndorsements)
                .Metadata
                .SetValueComparer(StringValueComparer());

            modelBuilder.Entity<AssertionModel>()
                .Property(x => x.AdditionalProperties)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<Dictionary<String, Object>>(v, null)
                );

            modelBuilder
                .Entity<AssertionModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<ArtifactModel>()
                .Property(x => x.AdditionalProperties)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<Dictionary<String, Object>>(v, null)
                );

            modelBuilder
                .Entity<ArtifactModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());

            modelBuilder.Entity<EvidenceModel>()
                .Property(x => x.AdditionalProperties)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<Dictionary<String, Object>>(v, null)
                );

            modelBuilder
                .Entity<EvidenceModel>()
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());
            #endregion

            modelBuilder.Entity<AssertionModel>()
                    .HasOne(a => a.ParentAssertion)
                    .WithMany(a => a.ChildAssertions)
                    .HasForeignKey(pt => pt.ParentAssertionId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClrModel>()
                .HasMany(clrModel => clrModel.Links)
                .WithOne(linkModel => linkModel.Clr)
                .HasForeignKey(linkModel => linkModel.ClrForeignKey);

            modelBuilder.Entity<ClrAssertion>()
                .HasOne(ca => ca.Clr)
                .WithMany(c => c.ClrAssertions)
                .HasForeignKey(pt => pt.ClrId);

            modelBuilder.Entity<ClrAssertion>()
                .HasOne(ca => ca.Assertion)
                .WithOne(a => a.ClrAssertion)
                .HasForeignKey<ClrAssertion>(pt => pt.AssertionId);

            modelBuilder.Entity<AssertionEvidence>()
                .HasOne(ae => ae.Assertion)
                .WithMany(a => a.AssertionEvidences)
                .HasForeignKey(pt => pt.AssertionId);

            modelBuilder.Entity<AssertionEvidence>()
                .HasOne(ae => ae.Evidence)
                .WithOne(a => a.AssertionEvidence)
                .HasForeignKey<AssertionEvidence>(pt => pt.EvidenceId);

            modelBuilder.Entity<EvidenceArtifact>()
                .HasOne(ea => ea.Evidence)
                .WithMany(e => e.EvidenceArtifacts)
                .HasForeignKey(pt => pt.EvidenceId);

            modelBuilder.Entity<EvidenceArtifact>()
                .HasOne(ea => ea.Artifact)
                .WithOne(a => a.EvidenceArtifact)
                .HasForeignKey<EvidenceArtifact>(pt => pt.ArtifactId);

            modelBuilder.Entity<ClrAchievement>()
                .HasOne(pc => pc.Clr)
                .WithMany(c => c.ClrAchievements)
                .HasForeignKey(pt => pt.ClrId);

            modelBuilder.Entity<ClrAchievement>()
                .HasOne(pc => pc.Achievement)
                .WithOne(c => c.ClrAchievement)
                .HasForeignKey<ClrAchievement>(pt => pt.AchievementId);

            modelBuilder.Entity<ClrEndorsement>()
                .HasOne(pc => pc.Clr)
                .WithMany(c => c.ClrEndorsements)
                .HasForeignKey(pt => pt.ClrId);

            modelBuilder.Entity<ClrEndorsement>()
                .HasOne(pc => pc.Endorsement)
                .WithOne(c => c.ClrEndorsement)
                .HasForeignKey<ClrEndorsement>(pt => pt.EndorsementId);

            modelBuilder.Entity<ClrModel>()
                .HasMany(cl => cl.Artifacts)
                .WithOne(ar => ar.Clr)
                .HasForeignKey(ar => ar.ClrId)
                .IsRequired(false);

            modelBuilder.Entity<AchievementAlignment>()
                .HasOne(pc => pc.Achievement)
                .WithMany(c => c.AchievementAlignments)
                .HasForeignKey(pt => pt.AchievementId);

            modelBuilder.Entity<AchievementAlignment>()
                .HasOne(pc => pc.Alignment)
                .WithOne(c => c.AchievementAlignment)
                .HasForeignKey<AchievementAlignment>(pt => pt.AlignmentId);

            modelBuilder.Entity<AchievementAssociation>()
                .HasOne(pc => pc.Achievement)
                .WithMany(c => c.AchievementAssociations)
                .HasForeignKey(pt => pt.AchievementId);

            modelBuilder.Entity<AchievementAssociation>()
                .HasOne(pc => pc.Association)
                .WithOne(c => c.AchievementAssociation)
                .HasForeignKey<AchievementAssociation>(pt => pt.AssociationId);

            modelBuilder.Entity<AchievementEndorsement>()
                .HasOne(pc => pc.Achievement)
                .WithMany(c => c.AchievementEndorsements)
                .HasForeignKey(pt => pt.AchievementId);

            modelBuilder.Entity<AchievementEndorsement>()
                .HasOne(pc => pc.Endorsement)
                .WithOne(c => c.AchievementEndorsement)
                .HasForeignKey<AchievementEndorsement>(pt => pt.EndorsementId);

            modelBuilder.Entity<AssertionEndorsement>()
                .HasOne(pc => pc.Assertion)
                .WithMany(c => c.AssertionEndorsements)
                .HasForeignKey(pt => pt.AssertionId);

            modelBuilder.Entity<AssertionEndorsement>()
                .HasOne(pc => pc.Endorsement)
                .WithOne(c => c.AssertionEndorsement)
                .HasForeignKey<AssertionEndorsement>(pt => pt.EndorsementId);

            modelBuilder.Entity<ProfileEndorsement>()
                .HasOne(pc => pc.Profile)
                .WithMany(c => c.ProfileEndorsements)
                .HasForeignKey(pt => pt.ProfileId);

            modelBuilder.Entity<ProfileEndorsement>()
                .HasOne(pc => pc.Endorsement)
                .WithOne(c => c.ProfileEndorsement)
                .HasForeignKey<ProfileEndorsement>(pt => pt.EndorsementId);

            modelBuilder.Entity<ResultAlignment>()
                .HasOne(pc => pc.Result)
                .WithMany(c => c.ResultAlignments)
                .HasForeignKey(pt => pt.ResultId);

            modelBuilder.Entity<ResultAlignment>()
                .HasOne(pc => pc.Alignment)
                .WithOne(c => c.ResultAlignment)
                .HasForeignKey<ResultAlignment>(pt => pt.AlignmentId);

            modelBuilder.Entity<ResultDescriptionAlignment>()
                .HasOne(pc => pc.ResultDescription)
                .WithMany(c => c.ResultDescriptionAlignments)
                .HasForeignKey(pt => pt.ResultDescriptionId);

            modelBuilder.Entity<ResultDescriptionAlignment>()
                .HasOne(pc => pc.Alignment)
                .WithOne(c => c.ResultDescriptionAlignment)
                .HasForeignKey<ResultDescriptionAlignment>(pt => pt.AlignmentId);

            modelBuilder.Entity<RubricCriterionLevelModel>()
                .HasOne(pc => pc.ResultDescription)
                .WithMany(c => c.RubricCriterionLevels)
                .HasForeignKey(pt => pt.ResultDescriptionId);

            modelBuilder.Entity<SourceModel>()
                .HasMany(sourceModel => sourceModel.Authorizations)
                .WithOne(authorizationModel => authorizationModel.Source)
                .HasForeignKey(authorizationModel => authorizationModel.SourceForeignKey);

            modelBuilder.Entity<AuthorizationModel>()
                .HasMany(authorizationModel => authorizationModel.Clrs)
                .WithOne(clrModel => clrModel.Authorization)
                .HasForeignKey(clrModel => clrModel.AuthorizationForeignKey);

            modelBuilder.Entity<AuthorizationModel>()
               .HasMany(authorizationModel => authorizationModel.CredentialPackages)
               .WithOne(pkg => pkg.Authorization)
               .HasForeignKey(pkg => pkg.AuthorizationForeignKey)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SourceModel>()
                .HasOne(sourceModel => sourceModel.DiscoveryDocument)
                .WithOne(documentModel => documentModel.Source)
                .HasForeignKey<DiscoveryDocumentModel>(documentModel => documentModel.SourceForeignKey);

            modelBuilder.Entity<CredentialPackageModel>()
                .Property(model => model.TypeId)
                .HasConversion<int>();            

            modelBuilder.Entity<CredentialPackageModel>()
                .HasMany(c => c.ContainedClrs)
                .WithOne(c => c.CredentialPackage)
                .HasForeignKey(c => c.CredentialPackageId)
                .IsRequired(true);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.ClrSet)
                .WithOne(c => c.ParentCredentialPackage)
                .HasForeignKey<ClrSetModel>(c => c.ParentCredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.VerifiableCredential)
                .WithOne(c => c.ParentCredentialPackage)
                .HasForeignKey<VerifiableCredentialModel>(c => c.ParentCredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.BadgrBackpack)
                .WithOne(c => c.ParentCredentialPackage)
                .HasForeignKey<BadgrBackpackModel>(c => c.ParentCredentialPackageId)
                .IsRequired(false);

            modelBuilder.Entity<CredentialPackageModel>()
                .HasOne(c => c.Authorization)
                .WithMany(a => a.CredentialPackages)
                .HasForeignKey(c => c.AuthorizationForeignKey)
                .IsRequired(false);

            modelBuilder.Entity<VerifiableCredentialModel>()
                .HasMany(c => c.ClrSets)
                .WithOne(c => c.ParentVerifiableCredential)
                .HasForeignKey("ParentVerifiableCredentialId")
                .IsRequired(false);

            modelBuilder.Entity<VerifiableCredentialModel>()
                .HasMany(c => c.Clrs)
                .WithOne(c => c.ParentVerifiableCredential)
                .HasForeignKey("ParentVerifiableCredentialId")
                .IsRequired(false);

            modelBuilder.Entity<ClrSetModel>()
                .HasMany(c => c.Clrs)
                .WithOne(c => c.ParentClrSet)
                .HasForeignKey("ParentClrSetId")
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

            modelBuilder.Entity<LoginProofRequest>()
                .Property(s => s.ProofRequestStatus)
                .HasConversion<int>();

            modelBuilder.Entity<LoginProofRequest>()
                .Property(s => s.Status)
                .HasConversion<int>();

            modelBuilder.Entity<EmailVerification>()
                .Property(s => s.Status)
                .HasConversion<int>();

            modelBuilder.Entity<EmailVerification>()
                .Property(model => model.Type)
                .HasConversion<int>();

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

            modelBuilder.Entity<AgentContextModel>(acm =>
            {
                acm.HasOne(ac => ac.ProvisioningToken).WithOne(pt => pt.AgentContext)
                    .HasForeignKey<ProvisioningTokenModel>(pt => pt.AgentContextId);
                acm.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ProofRequestStep>(prs =>
            {
                prs.Property(s => s.Id)
                    .HasConversion<int>();
                prs.HasData(
                    Enum.GetValues(typeof(ProofRequestStepEnum))
                        .Cast<ProofRequestStepEnum>()
                        .Select(s => new ProofRequestStep { Id = s, Name = s.ToString() }));
            });

            modelBuilder.Entity<ProofRequest>(pr =>
            {
                pr.HasOne(pr => pr.ProofResponse)
                    .WithOne(pr => pr.ProofRequest)
                    .HasForeignKey<ProofResponse>(pr => pr.ProofRequestId)
                    .OnDelete(DeleteBehavior.NoAction);

                pr.HasOne(pr => pr.CredentialSchema)
                    .WithMany()
                    .HasForeignKey(pr => pr.CredentialSchemaId)
                    .OnDelete(DeleteBehavior.NoAction);

                pr.Property(w => w.StepId)
                    .HasConversion<int>();

                pr.HasOne(pr => pr.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(false);

                pr.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<VerityThread>()
                .Property(w => w.FlowTypeId)
                .HasConversion<int>();

            // IsDeleted Filters, use .IgnoreQueryFilters() to include deleted items
            // IsDeleted Filters, use .IgnoreQueryFilters() to include deleted items
            modelBuilder.Entity<SourceModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<AuthorizationModel>().HasQueryFilter(x => !x.Source.IsDeleted);
            modelBuilder.Entity<DiscoveryDocumentModel>().HasQueryFilter(x => !x.Source.IsDeleted);
            modelBuilder.Entity<CredentialPackageModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ClrSetModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ClrModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ClrEndorsement>().HasQueryFilter(x => !x.Clr.IsDeleted);
            modelBuilder.Entity<ClrAchievement>().HasQueryFilter(x => !x.Clr.IsDeleted);
            modelBuilder.Entity<ClrAssertion>().HasQueryFilter(x => !x.Clr.IsDeleted);
            modelBuilder.Entity<LinkModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<VerifiableCredentialModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<BadgrBackpackModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<BadgrAssertionModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ShareModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CredentialRequestModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ConnectionRequestModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<WalletRelationshipModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<RecipientModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProvisioningTokenModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProofResponse>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MessageModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CredentialSchema>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CredentialDefinition>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<RevocationModel>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<EmailVerification>().HasQueryFilter(x => !x.IsDeleted);


            // Views
            modelBuilder.Entity<CredentialPackageArtifactView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("CredentialPackageArtifacts", "cred");
            });

            modelBuilder.Entity<CredentialListView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vw_CredentialList", "cred");
            });

            modelBuilder.Entity<CredentialSearchView>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("vw_CredentialSearch", "cred");
            });

            base.OnModelCreating(modelBuilder);
        }
        public async Task<Int32> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(true);
        }
        public override async Task<Int32> SaveChangesAsync(Boolean acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            ChangeTracker.DetectChanges();

            Timestamp(now);

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }
        private void Timestamp(DateTime now)
        {
            var addedEntities = ChangeTracker.Entries<IBaseEntity>().Where(E => E.State == EntityState.Added).ToList();

            addedEntities.ForEach(E =>
            {
                E.Entity.CreatedAt = now;
                E.Entity.ModifiedAt = now;
            });

            var editedEntities = ChangeTracker.Entries<IBaseEntity>().Where(E => E.State == EntityState.Modified).ToList();

            editedEntities.ForEach(E =>
            {
                E.Entity.ModifiedAt = now;
            });
        }
    }
}
