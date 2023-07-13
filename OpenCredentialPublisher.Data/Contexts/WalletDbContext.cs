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

        public DbSet<LoginLink> LoginLinks { get; set; }

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
            modelBuilder.Entity<AchievementAlignment>(eb =>
            {
                eb.ToTable("AchievementAlignments", schema: "cred");
                eb.HasOne(pc => pc.Achievement)
                    .WithMany(c => c.AchievementAlignments)
                    .HasForeignKey(pt => pt.AchievementId);

                eb.HasOne(pc => pc.Alignment)
                    .WithOne(c => c.AchievementAlignment)
                    .HasForeignKey<AchievementAlignment>(pt => pt.AlignmentId);
            });

            modelBuilder.Entity<AchievementAssociation>(eb =>
            {
                eb.ToTable("AchievementAssociations", schema: "cred");
                eb.HasOne(pc => pc.Achievement)
                    .WithMany(c => c.AchievementAssociations)
                    .HasForeignKey(pt => pt.AchievementId);

                eb.HasOne(pc => pc.Association)
                    .WithOne(c => c.AchievementAssociation)
                    .HasForeignKey<AchievementAssociation>(pt => pt.AssociationId);
            });

            modelBuilder.Entity<AchievementEndorsement>(eb => {
                eb.ToTable("AchievementEndorsements", schema: "cred");
                eb.HasOne(pc => pc.Achievement)
                    .WithMany(c => c.AchievementEndorsements)
                    .HasForeignKey(pt => pt.AchievementId);

                eb.HasOne(pc => pc.Endorsement)
                    .WithOne(c => c.AchievementEndorsement)
                    .HasForeignKey<AchievementEndorsement>(pt => pt.EndorsementId);
            });

            modelBuilder
                .Entity<AchievementModel>(eb =>
                {
                    eb.ToTable("Achievements", schema: "cred");
                    eb.Property(x => x.Tags)
                        .HasConversion(v => JsonSerializer.Serialize(v, null),
                            v => JsonSerializer.Deserialize<List<string>>(v, null));

                    eb.Property(x => x.Tags)
                        .Metadata
                        .SetValueComparer(StringValueComparer());

                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());


                    eb.Property(b => b.Identifiers)
                        .HasConversion(
                            v => JsonSerializer.Serialize(v, null),
                            v => JsonSerializer.Deserialize<List<SystemIdentifierDType>>(v, null)
                        );
                    eb.Property(b => b.Identifiers)
                        .Metadata
                        .SetValueComparer(SystemIdentifierDTypeValueComparer());
                });




            modelBuilder.Entity<AgentContextModel>(acm =>
            {
                acm.HasOne(ac => ac.ProvisioningToken).WithOne(pt => pt.AgentContext)
                    .HasForeignKey<ProvisioningTokenModel>(pt => pt.AgentContextId);
                acm.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder
                .Entity<AlignmentModel>(eb =>
                {
                    eb.ToTable("Alignments", schema: "cred");
                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder.Entity<ApplicationUser>(eb =>
            {
                eb.Property(a => a.CreatedDate)
                  .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<ArtifactModel>(eb =>
            {
                eb.ToTable("Artifacts", schema: "cred");
                eb.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(a => a.UserId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.NoAction);

                eb.Property(x => x.AdditionalProperties)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, null),
                        v => JsonSerializer.Deserialize<Dictionary<String, Object>>(v, null)
                    );
                eb.Property(x => x.AdditionalProperties)
                    .Metadata
                    .SetValueComparer(DictionaryValueComparer());
            });

            modelBuilder.Entity<AssertionEndorsement>(eb =>
            {
                eb.ToTable("AssertionEndorsements", schema: "cred");
                eb.HasOne(pc => pc.Assertion)
                    .WithMany(c => c.AssertionEndorsements)
                    .HasForeignKey(pt => pt.AssertionId);

                eb.HasOne(pc => pc.Endorsement)
                    .WithOne(c => c.AssertionEndorsement)
                    .HasForeignKey<AssertionEndorsement>(pt => pt.EndorsementId);
            });

            modelBuilder.Entity<AssertionEvidence>(eb =>
            {
                eb.ToTable("AssertionEvidence", schema: "cred");
                eb.HasOne(ae => ae.Assertion)
                    .WithMany(a => a.AssertionEvidences)
                    .HasForeignKey(pt => pt.AssertionId);

                eb.HasOne(ae => ae.Evidence)
                    .WithOne(a => a.AssertionEvidence)
                    .HasForeignKey<AssertionEvidence>(pt => pt.EvidenceId);
            });

            modelBuilder.Entity<AssertionModel>(eb =>
            {
                eb.ToTable("Assertions", schema: "cred");
                eb.HasOne(a => a.ParentAssertion)
                    .WithMany(a => a.ChildAssertions)
                    .HasForeignKey(pt => pt.ParentAssertionId)
                    .OnDelete(DeleteBehavior.Restrict);

                eb.Property(x => x.SignedEndorsements)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null)
                );

                eb.Property(x => x.SignedEndorsements)
                    .Metadata
                    .SetValueComparer(StringValueComparer());

                eb.Property(x => x.AdditionalProperties)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, null),
                        v => JsonSerializer.Deserialize<Dictionary<String, Object>>(v, null)
                    );

                eb.Property(x => x.AdditionalProperties)
                    .Metadata
                    .SetValueComparer(DictionaryValueComparer());
            });

            modelBuilder
                .Entity<AssociationModel>(eb =>
                {
                    eb.ToTable("Associations", schema: "cred");
                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder.Entity<AuthorizationModel>(eb =>
            {
                eb.Property(x => x.Scopes)
                    .HasConversion(v => JsonSerializer.Serialize(v, null),
                        v => JsonSerializer.Deserialize<List<string>>(v, null));

                eb.Property(x => x.Scopes)
                    .Metadata
                    .SetValueComparer(StringValueComparer());

                eb
                   .HasMany<CredentialPackageModel>()
                   .WithOne(pkg => pkg.Authorization)
                   .HasForeignKey(pkg => pkg.AuthorizationForeignKey)
                   .OnDelete(DeleteBehavior.SetNull);

                eb.HasQueryFilter(x => !x.Source.IsDeleted);
            });

            modelBuilder.Entity<BadgrAssertionModel>(eb =>
            {
                eb.ToTable("BadgrAssertions", schema: "cred");
                eb.HasOne(c => c.BadgrBackpack)
                   .WithMany(a => a.BadgrAssertions)
                   .HasForeignKey(a => a.BadgrBackpackId)
                   .IsRequired(true);

                eb.Property(b => b.AdditionalProperties)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, null),
                        v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                    );

                eb
                    .Property(x => x.AdditionalProperties)
                    .Metadata
                    .SetValueComparer(DictionaryValueComparer());

                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<BadgrBackpackModel>(eb =>
            {
                eb.ToTable("BadgrBackpacks", schema: "cred");
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ClrAchievement>(eb =>
            {
                eb.ToTable("ClrAchievements", schema: "cred");
            });

            modelBuilder.Entity<ClrAssertion>(eb =>
            {
                eb.ToTable("ClrAssertions", schema: "cred");
            });

            modelBuilder.Entity<ClrEndorsement>(eb =>
            {
                eb.ToTable("ClrEndorsements", schema: "cred");
            });

            modelBuilder.Entity<ClrModel>(eb =>
            {
                eb.ToTable("Clrs", schema: "cred");
                eb.HasOne(clr => clr.Authorization)
                    .WithMany()
                    .HasForeignKey(auth => auth.AuthorizationForeignKey);

                eb.HasMany(clr => clr.ClrAchievements)
                  .WithOne()
                  .HasForeignKey(ca => ca.ClrId);

                eb.HasMany(cl => cl.Artifacts)
                    .WithOne()
                    .HasForeignKey(ar => ar.ClrId)
                    .IsRequired(false);

                eb.HasMany(pc => pc.ClrEndorsements)
                    .WithOne()
                    .HasForeignKey(pt => pt.ClrId);

                eb.HasMany(clr => clr.ClrAssertions)
                    .WithOne()
                    .HasForeignKey(ca => ca.ClrId);

                eb.HasMany(clr => clr.Links)
                    .WithOne(linkModel => linkModel.Clr)
                    .HasForeignKey(linkModel => linkModel.ClrForeignKey);

                eb.HasOne(clr => clr.SmartResume)
                    .WithOne()
                    .HasForeignKey<SmartResume>(sr => sr.ClrId);

                eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                eb.Property(x => x.AdditionalProperties)
                    .Metadata
                    .SetValueComparer(DictionaryValueComparer());

                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ClrSetModel>(eb =>
            {
                eb.ToTable("ClrSets", schema: "cred");
                eb.HasMany(c => c.Clrs)
                    .WithOne(c => c.ParentClrSet)
                    .HasForeignKey("ParentClrSetId")
                    .IsRequired(false);

                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ConnectionRequestModel>(eb =>
            {
                eb.HasIndex(w => w.ThreadId)
                    .IsUnique();
                eb.Property(w => w.ConnectionRequestStep)
                    .HasConversion<int>();

                eb.HasQueryFilter(x => !x.IsDeleted);

            });

            modelBuilder.Entity<ConnectionRequestStep>(eb =>
            {
                eb.Property(s => s.Id)
                    .HasConversion<int>();
                eb.HasData(Enum.GetValues(typeof(ConnectionRequestStepEnum)).Cast<ConnectionRequestStepEnum>().Select(s => new ConnectionRequestStep { Id = s, Name = s.ToString() }));
            });


            modelBuilder.Entity<CredentialDefinition>(eb =>
            {
                eb.Property(m => m.StatusId)
                .HasConversion<int>();
                eb.HasQueryFilter(x => !x.IsDeleted);

            });

            modelBuilder.Entity<CredentialPackageModel>(eb =>
            {
                eb.ToTable("CredentialPackages", schema: "cred");
                eb.Property(model => model.TypeId)
                    .HasConversion<int>();
                eb.HasMany(c => c.ContainedClrs)
                    .WithOne(c => c.CredentialPackage)
                    .HasForeignKey(c => c.CredentialPackageId)
                    .IsRequired(true);
                eb.HasOne(c => c.ClrSet)
                    .WithOne(c => c.ParentCredentialPackage)
                    .HasForeignKey<ClrSetModel>(c => c.ParentCredentialPackageId)
                    .IsRequired(false);
                eb.HasOne(c => c.VerifiableCredential)
                    .WithOne(c => c.ParentCredentialPackage)
                    .HasForeignKey<VerifiableCredentialModel>(c => c.ParentCredentialPackageId)
                    .IsRequired(false);
                eb.HasOne(c => c.BadgrBackpack)
                    .WithOne(c => c.ParentCredentialPackage)
                    .HasForeignKey<BadgrBackpackModel>(c => c.ParentCredentialPackageId)
                    .IsRequired(false);
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<CredentialRequestModel>(eb =>
            {
                eb.Property(w => w.CredentialRequestStep)
                    .HasConversion<int>();
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<CredentialRequestStep>(eb =>
            {
                eb.Property(s => s.Id)
                    .HasConversion<int>();
                eb.HasData(Enum.GetValues(typeof(CredentialRequestStepEnum)).Cast<CredentialRequestStepEnum>().Select(s => new CredentialRequestStep { Id = s, Name = s.ToString() }));
            });

            modelBuilder.Entity<CredentialSchema>(eb =>
            {
                eb.Property(m => m.StatusId)
                    .HasConversion<int>();
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder
                .Entity<CriteriaModel>(eb =>
                {
                    eb.ToTable("Criteria", schema: "cred");
                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder.Entity<DiscoveryDocumentModel>(eb =>
            {
                eb.ToTable("DiscoveryDocumentModel", schema: "cred");
                eb.Property(x => x.ScopesOffered)
                    .HasConversion(v => JsonSerializer.Serialize(v, null),
                        v => JsonSerializer.Deserialize<List<string>>(v, null));

                eb.Property(x => x.ScopesOffered)
                    .Metadata
                    .SetValueComparer(StringValueComparer());
            });


            modelBuilder.Entity<EmailVerification>(eb =>
            {
                eb.Property(s => s.Status)
                    .HasConversion<int>();
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder
                .Entity<EndorsementClaimModel>(eb =>
                {
                    eb.ToTable("EndorsementClaims", schema: "cred");
                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder
                .Entity<EndorsementModel>(eb =>
                {
                    eb.ToTable("Endorsements", schema: "cred");
                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder.Entity<EvidenceArtifact>(eb => {
                eb.ToTable("EvidenceArtifacts", schema: "cred");
            });

            modelBuilder.Entity<EvidenceModel>(eb => {
                eb.ToTable("Evidence", schema: "cred");
                eb.Property(x => x.AdditionalProperties)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<Dictionary<String, Object>>(v, null)
                );

                eb
                .Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());
            });

            modelBuilder.Entity<Models.ClrEntities.IdentityModel>(eb =>
            {
                eb.ToTable("Identities", schema: "cred");
                eb.Property(b => b.AdditionalProperties)
               .HasConversion(
                   v => JsonSerializer.Serialize(v, null),
                   v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
               );

                eb.Property(x => x.AdditionalProperties)
                .Metadata
                .SetValueComparer(DictionaryValueComparer());
            });

            modelBuilder.Entity<LinkModel>(eb =>
            {
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<LoginLink>(eb =>
            {
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<LoginProofRequest>(eb =>
            {
                eb.Property(s => s.ProofRequestStatus)
                    .HasConversion<int>();

                eb.Property(s => s.Status)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<MessageModel>(eb =>
            {
                eb.Property(m => m.StatusId)
                    .HasConversion<int>();
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ProfileEndorsement>(eb => {
                eb.ToTable("ProfileEndorsements", schema: "cred");
                eb.HasOne(pc => pc.Profile)
                    .WithMany(c => c.ProfileEndorsements)
                    .HasForeignKey(pt => pt.ProfileId);

                eb.HasOne(pc => pc.Endorsement)
                    .WithOne(c => c.ProfileEndorsement)
                    .HasForeignKey<ProfileEndorsement>(pt => pt.EndorsementId);
            });

            modelBuilder.Entity<ProfileModel>(eb =>
            {
                eb.ToTable("Profiles", schema: "cred");
                eb.Property(b => b.PublicKey)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, null),
                       v => JsonSerializer.Deserialize<CryptographicKeyDType>(v, null)
                   );

                eb.Property(b => b.AdditionalProperties)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, null),
                       v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                   );

                eb.Property(x => x.AdditionalProperties)
                    .Metadata
                    .SetValueComparer(DictionaryValueComparer());

                eb.Property(b => b.Identifiers)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, null),
                       v => JsonSerializer.Deserialize<List<SystemIdentifierDType>>(v, null)
                   );

                eb.Property(b => b.Identifiers)
                    .Metadata
                    .SetValueComparer(SystemIdentifierDTypeValueComparer());
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

            modelBuilder.Entity<ProofRequestStep>(prs =>
            {
                prs.Property(s => s.Id)
                    .HasConversion<int>();
                prs.HasData(
                    Enum.GetValues(typeof(ProofRequestStepEnum))
                        .Cast<ProofRequestStepEnum>()
                        .Select(s => new ProofRequestStep { Id = s, Name = s.ToString() }));
            });

            modelBuilder.Entity<ProofResponse>(eb =>
            {
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ProvisioningTokenModel>(eb =>
            {
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<RecipientModel>(eb =>
            {
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<ResultAlignment>(eb => {
                eb.ToTable("ResultAlignments", schema: "cred");
                eb.HasOne(pc => pc.Result)
                    .WithMany(c => c.ResultAlignments)
                    .HasForeignKey(pt => pt.ResultId);

                eb.HasOne(pc => pc.Alignment)
                    .WithOne(c => c.ResultAlignment)
                    .HasForeignKey<ResultAlignment>(pt => pt.AlignmentId);
            });

            modelBuilder.Entity<ResultDescriptionAlignment>(eb =>
            {
                eb.ToTable("ResultDescriptionAlignments", schema: "cred");

                eb.HasOne(pc => pc.ResultDescription)
                    .WithMany(c => c.ResultDescriptionAlignments)
                    .HasForeignKey(pt => pt.ResultDescriptionId);

                eb.HasOne(pc => pc.Alignment)
                    .WithOne(c => c.ResultDescriptionAlignment)
                    .HasForeignKey<ResultDescriptionAlignment>(pt => pt.AlignmentId);
            });

            modelBuilder
                .Entity<ResultDescriptionModel>(eb =>
                {
                    eb.ToTable("ResultDescriptions", schema: "cred");
                    eb.Property(x => x.AllowedValues)
                        .HasConversion(v => JsonSerializer.Serialize(v, null),
                            v => JsonSerializer.Deserialize<List<string>>(v, null));

                    eb.Property(x => x.AllowedValues)
                        .Metadata
                        .SetValueComparer(StringValueComparer());

                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder
                .Entity<ResultModel>(eb =>
                {
                    eb.ToTable("Results", schema: "cred");
                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder.Entity<RevocationModel>(eb =>
            {
                eb.HasOne(r => r.Source)
                    .WithMany()
                    .HasForeignKey(s => s.SourceId);

                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<RubricCriterionLevelAlignment>(eb =>
            {
                eb.ToTable("RubricCriterionLevelAlignments", schema: "cred");
            });

            modelBuilder.Entity<RubricCriterionLevelModel>(eb =>
            {
                eb.ToTable("RubricCriterionLevels", schema: "cred");
                eb.Property(b => b.AdditionalProperties)
                   .HasConversion(
                       v => JsonSerializer.Serialize(v, null),
                       v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                   );
                eb.Property(x => x.AdditionalProperties)
                    .Metadata
                    .SetValueComparer(DictionaryValueComparer());
                eb.HasOne(pc => pc.ResultDescription)
                    .WithMany(c => c.RubricCriterionLevels)
                    .HasForeignKey(pt => pt.ResultDescriptionId);
            });

            modelBuilder.Entity<ShareTypeModel>(eb =>
            {
                eb.Property(s => s.Id)
                .HasConversion<int>();

                eb.HasData(Enum.GetValues(typeof(ShareTypeEnum)).Cast<ShareTypeEnum>().Select(s => new ShareTypeModel { Id = s, Name = s.ToString() }));
            });

            modelBuilder.Entity<SmartResume>(sr =>
            {
                sr.ToTable("SmartResumes", "idatafy");
                sr.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired(true);
            });

            modelBuilder.Entity<SourceModel>(eb =>
            {
                eb.Property(s => s.SourceTypeId)
                    .HasConversion<int>();

                eb.HasMany(sourceModel => sourceModel.Authorizations)
                    .WithOne(authorizationModel => authorizationModel.Source)
                    .HasForeignKey(authorizationModel => authorizationModel.SourceForeignKey);

                eb.HasOne(sourceModel => sourceModel.DiscoveryDocument)
                    .WithOne()
                    .HasForeignKey<DiscoveryDocumentModel>(documentModel => documentModel.SourceForeignKey);

                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder.Entity<StatusModel>(eb => {
                eb.Property(s => s.Id)
                    .HasConversion<int>();

                eb.HasData(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().Select(s => new StatusModel { Id = s, Name = s.ToString() }));
            });

            modelBuilder.Entity<VerifiableCredentialModel>(eb =>
            {
                eb.ToTable("VerifiableCredentials", schema: "cred");
                eb.HasMany(c => c.ClrSets)
                    .WithOne(c => c.ParentVerifiableCredential)
                    .HasForeignKey("ParentVerifiableCredentialId")
                    .IsRequired(false);
                eb.HasMany(c => c.Clrs)
                    .WithOne(c => c.ParentVerifiableCredential)
                    .HasForeignKey("ParentVerifiableCredentialId")
                    .IsRequired(false);

                eb.HasQueryFilter(x => !x.IsDeleted);
            });

            modelBuilder
                .Entity<VerificationModel>(eb =>
                {
                    eb.ToTable("Verifications", schema: "cred");
                    eb.Property(x => x.AllowedOrigins)
                        .HasConversion(v => JsonSerializer.Serialize(v, null),
                            v => JsonSerializer.Deserialize<List<string>>(v, null));

                    eb.Property(x => x.AllowedOrigins)
                        .Metadata
                        .SetValueComparer(StringValueComparer());

                    eb.Property(x => x.StartsWith)
                        .HasConversion(v => JsonSerializer.Serialize(v, null),
                            v => JsonSerializer.Deserialize<List<string>>(v, null));

                    eb.Property(x => x.StartsWith)
                        .Metadata
                        .SetValueComparer(StringValueComparer());

                    eb.Property(b => b.AdditionalProperties)
                       .HasConversion(
                           v => JsonSerializer.Serialize(v, null),
                           v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, null)
                       );
                    eb.Property(x => x.AdditionalProperties)
                        .Metadata
                        .SetValueComparer(DictionaryValueComparer());
                });

            modelBuilder.Entity<VerityThread>(eb =>
            {
                eb.Property(w => w.FlowTypeId)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<WalletRelationshipModel>(eb =>
            {
                eb.HasQueryFilter(x => !x.IsDeleted);
            });

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
