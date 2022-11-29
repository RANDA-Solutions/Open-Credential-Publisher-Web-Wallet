using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Extensions;
using Schema.NET;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class ETLService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;
        private readonly ILogger<ETLService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<HostSettings> _hostSettings;

        public ETLService(SchemaService schemaService, WalletDbContext context, IHttpContextAccessor httpContextAccessor, IOptions<HostSettings> hostSettings, ILogger<ETLService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _schemaService = schemaService;            
            _context = context;
            _hostSettings = hostSettings;
            _logger = logger;
        }
        public async Task<CredentialResponse> ProcessJson(ControllerBase controller, string userId, string json, AuthorizationModel authorization)
        {
            var modelState = controller.ModelState;
            SchemaResult schemaResult;
            var credentialResponse = new CredentialResponse();
            if (json.Contains("https://www.w3.org/2018/credentials/v1"))
            {
                schemaResult = await _schemaService.ValidateSchemaAsync<VerifiableCredential>(controller.Request, json);
                if (schemaResult.IsValid)
                    return await ProcessVerifiableCredential(userId, null, json, authorization);
            }
            else if (json.Contains("CLRSet"))
            {
                schemaResult = await _schemaService.ValidateSchemaAsync<ClrSetDType>(controller.Request, json);
                if (schemaResult.IsValid)
                    await SaveClrSetPackageAsync(modelState, json, authorization);
            }
            else
            {
                schemaResult = await _schemaService.ValidateSchemaAsync<ClrDType>(controller.Request, json);
                if (schemaResult.IsValid)
                    await SaveClrPackageAsync(modelState, json, authorization);
            }

            if (!schemaResult.IsValid)
            {
                credentialResponse.ErrorMessages = schemaResult.ErrorMessages;
            }

            if (!modelState.IsValid)
            {
                var modelErrors = new List<string>();
                foreach (var ms in modelState.Values)
                {
                    foreach (var modelError in ms.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                credentialResponse.ErrorMessages = modelErrors;
            }
            return credentialResponse;
        }
        public async Task<CredentialResponse> ProcessJson(HttpRequest request, ModelStateDictionary modelState, string userId, string fileName, string json, AuthorizationModel authorization)
        {
            var credentialResponse = new CredentialResponse();
            SchemaResult schemaResult;
            if (json.Contains("https://www.w3.org/2018/credentials/v1"))
            {
                schemaResult = await _schemaService.ValidateSchemaAsync<VerifiableCredential>(request, json);
                if (schemaResult.IsValid)
                    return await ProcessVerifiableCredential(userId, fileName, json, authorization);
            }
            else if (json.Contains("CLRSet"))
            {
                schemaResult = await _schemaService.ValidateSchemaAsync<ClrSetDType>(request, json);
                if (schemaResult.IsValid)
                    await SaveClrSetPackageAsync(modelState, json, authorization);
            }
            else
            {
                schemaResult = await _schemaService.ValidateSchemaAsync<ClrDType>(request, json);
                if (schemaResult.IsValid)
                    await SaveClrPackageAsync(modelState, json, authorization);
            }

            if (!schemaResult.IsValid)
            {
                credentialResponse.ErrorMessages = schemaResult.ErrorMessages;
            }

            if (!modelState.IsValid)
            {
                var modelErrors = new List<string>();
                foreach (var ms in modelState.Values)
                {
                    foreach (var modelError in ms.Errors)
                    {
                        modelErrors.Add(modelError.ErrorMessage);
                    }
                }
                credentialResponse.ErrorMessages = modelErrors;
            }

            return credentialResponse;
        }
        public async Task SaveClrPackageModelAsync(CredentialPackageModel credentialPackage)
        {
            try
            {
                _context.CredentialPackages.Add(credentialPackage);
                credentialPackage.AssertionsCount = credentialPackage.Clr.AssertionsCount;

                await _context.SaveChangesAsync();

                await EnhanceArtifactsAsync(credentialPackage);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, ex.Message);
                throw;
            }
        }
        public async Task SaveClrPackageAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {
            try
            {
                var pkgAssertionCount = 0;
                var pkgType = PackageTypeEnum.Clr;
                var clrDType = null as ClrDType;
                //Turn EF Tracking on for untracked authorization
                if (authorization != null)
                {
                    _context.Attach(authorization);
                }
                try
                {
                    clrDType = JsonConvert.DeserializeObject<ClrDType>(content);
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(string.Empty, ex.Message);
                    _logger.LogException(ex, ex.Message);

                    return;
                }

                //For now, every uploaded Package (no Auth/Source) will be saved anew. A method to uniquely identify such a package by it's contents will need to be finalized
                // Example might be Package content type - ClrSet, VC, CLR, Backpack followed by the contained entity's unique identifier
                // Every refresh of uploaded packages will save the raw package in blob storage, but the database will contain only the most recent version
                var credentialPackage = null as CredentialPackageModel;
                credentialPackage = new CredentialPackageModel
                {
                    TypeId = pkgType,
                    AuthorizationForeignKey = authorization?.Id,
                    UserId = _httpContextAccessor.HttpContext.User.JwtUserId(),
                    CreatedAt = DateTime.UtcNow
                };
                _context.CredentialPackages.Add(credentialPackage);
                await _context.SaveChangesAsync(); //added this for now because clr.credentialpackageid fk was barking

                // Save each CLR
                var clr = null as ClrModel;
                clr = ConvertClr(clrDType, null, authorization, credentialPackage);
                pkgAssertionCount += clr.AssertionsCount;
                clr.ParentCredentialPackageId = credentialPackage.Id;
                if (_context.Entry(clr).State == EntityState.Detached)
                {
                    credentialPackage.Clr = clr;
                }

                credentialPackage.AssertionsCount = pkgAssertionCount;

                await _context.SaveChangesAsync();

                await EnhanceArtifactsAsync(credentialPackage);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, ex.Message);

                throw;
            }
        }
        public async Task SaveClrSetPackageAsync(ModelStateDictionary modelState, string content, AuthorizationModel authorization)
        {
            try
            {
                var pkgAssertionCount = 0;
                var pkgType = authorization.Source.SourceTypeId switch
                {
                    SourceTypeEnum.OpenBadge => PackageTypeEnum.OpenBadge,
                    SourceTypeEnum.Clr => PackageTypeEnum.ClrSet,
                    _ => throw new NotImplementedException()
                };

                if (pkgType != PackageTypeEnum.ClrSet)
                {
                    throw new ArgumentOutOfRangeException("CredentialPackage type not ClrSet in SaveClrSetPackageAsync.");
                }

                // Validate the response data
                await _schemaService.ValidateSchemaAsync<ClrSetDType>(_httpContextAccessor.HttpContext.Request, content);

                if (!modelState.IsValid) return;

                //Turn EF Tracking on for untracked authorization
                if (authorization != null)
                {
                    _context.Attach(authorization);
                }
                var clrsetDType = JsonConvert.DeserializeObject<ClrSetDType>(content); //clrSet.Id will always be null at least from imsglobal as there is no explicit set definition                

                //For now, every uploaded Package (no Auth/Source) will be saved anew. A method to uniquely identify such a package by it's contents will need to be finalized
                // Example might be Package content type - ClrSet, VC, CLR, Backpack followed by the contained entity's unique identifier
                // Every refresh of uploaded packages will save the raw package in blob storage, but the database will contain only the most recent version
                var credentialPackage = null as CredentialPackageModel; 
                                   
                credentialPackage = new CredentialPackageModel
                {
                    TypeId = pkgType,
                    AuthorizationForeignKey = authorization.Id,
                    UserId = _httpContextAccessor.HttpContext.User.JwtUserId(),
                    CreatedAt = DateTime.UtcNow,
                    ClrSet = new ClrSetModel
                    {
                        ParentCredentialPackage = credentialPackage,
                        Identifier = clrsetDType.Context,
                        Json = content,
                        ClrsCount = clrsetDType.Clrs.Count,
                        Clrs = new List<ClrModel>()
                    }
                };
                _context.CredentialPackages.Add(credentialPackage);
                await _context.SaveChangesAsync(); //added this for now because clr.credentialpackageid fk was barking


                // Save each CLR
                var clr = null as ClrModel;
                foreach (var clrDType in clrsetDType.Clrs)
                {
                    clr = ConvertClr(clrDType, null, authorization, credentialPackage);
                    pkgAssertionCount += clr.AssertionsCount;
                    // Add @context to each clr in case it is downloaded
                    clrDType.Context = clrsetDType.Context;
                    clr.ParentClrSetId = credentialPackage.ClrSet.Id;
                    if (_context.Entry(clr).State == EntityState.Detached)
                    {
                        credentialPackage.ClrSet.Clrs.Add(clr);
                    }
                }

                foreach (var signedClrDType in clrsetDType.SignedClrs)
                {
                    var clrDType = signedClrDType.DeserializePayload<ClrDType>();
                    clr = ConvertClr(clrDType, signedClrDType, authorization, credentialPackage);
                    pkgAssertionCount += clr.AssertionsCount;
                    clr.ParentClrSet = credentialPackage.ClrSet;
                    clr.ParentClrSetId = credentialPackage.ClrSet.Id;

                    // Add @context to each clr in case it is downloaded
                    clrDType.Context = clrsetDType.Context;
                    if (_context.Entry(clr).State == EntityState.Detached)
                    {
                        credentialPackage.ClrSet.Clrs.Add(clr);
                    }
                }

                credentialPackage.AssertionsCount = pkgAssertionCount;

                await _context.SaveChangesAsync();

                await EnhanceArtifactsAsync(credentialPackage);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, ex.Message);

                throw;
            }
        }
        public async Task<CredentialResponse> ProcessVerifiableCredential(string userId, string fileName, string json, AuthorizationModel authorization)
        {
            var pkgAssertionCount = 0;
            var credentialResponse = new CredentialResponse();
            try
            {
                VerifiableCredential vc;
                try
                {
                    vc = System.Text.Json.JsonSerializer.Deserialize<VerifiableCredential>(json);
                    if (vc.CredentialSubjects == null)
                    {
                        vc.CredentialSubjects = new List<ICredentialSubject>();
                    }
                }
                catch (Exception e)
                {
                    credentialResponse.ErrorMessages.Add(e.Message);
                    return credentialResponse;
                }

                if (await _context.VerifiableCredentials.AnyAsync(v => v.Identifier == vc.Id && !v.IsDeleted))
                {
                    credentialResponse.ErrorMessages.Add("That credential has already been loaded.");
                    return credentialResponse;
                }

                var credentialPackage = new CredentialPackageModel
                {
                    TypeId = PackageTypeEnum.VerifiableCredential,
                    AuthorizationForeignKey = authorization == null ? null : authorization.Id,
                    Name = fileName,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    UserId = userId,

                    VerifiableCredential = new VerifiableCredentialModel
                    {
                        CredentialsCount = vc.CredentialSubjects.Count,
                        Identifier = vc.Id,
                        IssuedOn = vc.IssuanceDate,
                        Issuer = vc.Issuer,
                        Json = json,
                        Clrs = new List<ClrModel>(),
                        ClrSets = new List<ClrSetModel>()
                    }
                };

                foreach (var cred in vc.CredentialSubjects)
                {
                    if (cred is ClrSetSubject clrSet)
                    {
                        var clrSetJson = System.Text.Json.JsonSerializer.Serialize(clrSet, new JsonSerializerOptions { IgnoreNullValues = true });
                        var clrSetModel = new ClrSetModel
                        {
                            Json = clrSetJson,
                            Identifier = clrSet.Id,
                            ClrsCount = (clrSet.Clrs?.Count ?? 0) + (clrSet.SignedClrs?.Count ?? 0),
                            Clrs = new List<ClrModel>()
                        };

                        foreach (var clr in clrSet.Clrs)
                        {
                            var clrEntity = ConvertClr(clr, null, authorization, credentialPackage);
                            pkgAssertionCount += clrEntity.AssertionsCount;
                            clrSetModel.Clrs.Add(clrEntity);
                        }

                        foreach (var signedClr in clrSet.SignedClrs)
                        {
                            var clr = signedClr.DeserializePayload<ClrDType>();
                            var clrEntity = ConvertClr(clr, signedClr, authorization, credentialPackage);
                            pkgAssertionCount += clrEntity.AssertionsCount;
                            clrSetModel.Clrs.Add(clrEntity);
                        }
                        credentialPackage.VerifiableCredential.ClrSets.Add(clrSetModel);
                    }

                    if (cred is ClrSubject clrSubject)
                    {
                        var clrEntity = ConvertClr(clrSubject, null, authorization, credentialPackage);
                        pkgAssertionCount += clrEntity.AssertionsCount;
                        credentialPackage.VerifiableCredential.Clrs.Add(clrEntity);
                    }
                }

                credentialPackage.AssertionsCount = pkgAssertionCount;

                await _context.CredentialPackages.AddAsync(credentialPackage);
                await _context.SaveChangesAsync();

                await EnhanceArtifactsAsync(credentialPackage);

                credentialResponse.Id = credentialPackage.Id;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "ETLService.ProcessVerifiableCredential", null);
                credentialResponse.ErrorMessages.Add(ex.Message);
            }
            return credentialResponse;
        }
        public void AddClrAchievements(ref ClrModel clr, ClrDType clrDType)
        {
            var order = 0;
            if (clrDType.Achievements != null)
            {
                foreach (var achievementDType in clrDType.Achievements)
                {
                    order++;
                    var achievementModel = ConvertAchievement(achievementDType);
                    var clrAchievement = ClrAchievement.Combine(clr.ClrId, achievementModel, order);
                    clr.ClrAchievements.Add(clrAchievement);
                }
            }
        }
        //public void RefreshClr (ClrModel clrModel, AssertionModel assertion, AssertionDType assertionDType)
        //{
        //    assertion.Delete();
        //    _context.Entry(assertion).State = EntityState.Modified;

        //    assertion = AssertionModel.FromDType(assertionDType)

        //}
        public void AddClrAssertions(ref ClrModel clr, ClrDType clrDType)
        {
            var order = 0;
            if (clrDType.SignedAssertions != null)
            {
                foreach (var asrtDType in clrDType.SignedAssertions)
                {
                    order++;
                    var decoded = asrtDType.DeserializePayload<AssertionDType>();
                    var signedAsrt = AssertionModel.FromDTypeShallow(decoded, asrtDType);
                    AddAssertionChildren(ref signedAsrt, decoded);
                    var clrAssertion = ClrAssertion.Combine(clr.ClrId, signedAsrt, order);
                    clr.ClrAssertions.Add(clrAssertion);
                }
            }
            if (clrDType.Assertions != null)
            {
                foreach (var asrtDType in clrDType.Assertions)
                {
                    order++;
                    var clrAsrt = AssertionModel.FromDTypeShallow(asrtDType);
                    AddAssertionChildren(ref clrAsrt, asrtDType);
                    var clrAssertion = ClrAssertion.Combine(clr.ClrId, clrAsrt, order);
                    clr.ClrAssertions.Add(clrAssertion);
                }
            }
            clr.AssertionsCount = clr.ClrAssertions.Count;
        }
        public void AddAssertionChildren(ref AssertionModel assertionModel, AssertionDType assertionDType)
        {
            try
            { 
                var order = 0;
                if (assertionDType.Recipient != null)
                {
                    assertionModel.Recipient = Data.Models.ClrEntities.IdentityModel.FromDType(assertionDType.Recipient);
                }
                if (assertionDType.Source != null)
                {
                    order = 0;
                    assertionModel.Source = ProfileModel.FromDType(assertionDType.Source);
                    if (assertionDType.Source.SignedEndorsements != null)
                    {
                        var endorsementModels = ConvertSignedEndorsements(assertionDType.SignedEndorsements);

                        foreach (var endorsementModel in endorsementModels)
                        {
                            order++;
                            var profileEndorsement = ProfileEndorsement.Combine(assertionModel.Source, endorsementModel, order);
                            assertionModel.Source.ProfileEndorsements.Add(profileEndorsement);
                        }
                    }
                    if (assertionDType.Source.Endorsements != null)
                    {
                        var endorsementModels = ConvertEndorsements(assertionDType.Endorsements);
                        foreach (var endorsementModel in endorsementModels)
                        {
                            order++;
                            var profileEndorsement = ProfileEndorsement.Combine(assertionModel.Source, endorsementModel, order);
                            assertionModel.Source.ProfileEndorsements.Add(profileEndorsement);
                        }
                    }
                }
                if (assertionDType.Verification != null)
                {
                    assertionModel.Verification = VerificationModel.FromDType(assertionDType.Verification);
                }
                AddAssertionEvidence(ref assertionModel, assertionDType);

                assertionModel.Achievement = ConvertAchievement(assertionDType.Achievement);

                order = 0;
                if (assertionDType.SignedEndorsements != null)
                {
                    var endorsementModels = ConvertSignedEndorsements(assertionDType.SignedEndorsements);
                    foreach (var endorsementModel in endorsementModels)
                    {
                        order++;
                        var assertionEndorsement = AssertionEndorsement.Combine(assertionModel, endorsementModel, order);
                        assertionModel.AssertionEndorsements.Add(assertionEndorsement);
                    }
                }
                if (assertionDType.Endorsements != null)
                {
                    var endorsementModels = ConvertEndorsements(assertionDType.Endorsements);
                    foreach (var endorsementModel in endorsementModels)
                    {
                        order++;
                        var assertionEndorsement = AssertionEndorsement.Combine(assertionModel, endorsementModel, order);
                        assertionModel.AssertionEndorsements.Add(assertionEndorsement);
                    }
                }
                if (assertionDType.Results != null)
                {
                    order = 0;
                    foreach (var resultDType in assertionDType.Results)
                    {
                        order++;
                        var resultModel = ResultModel.FromDType(resultDType);
                        resultModel.Order = order;
                        assertionModel.Results.Add(resultModel);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "ETLService.ProcessVerifiableCredential", null);
            }
        }

        public void AddAssertionEvidence(ref AssertionModel assertionModel, AssertionDType assertionDType)
        {
            if (assertionDType.Evidence != null)
            {
                var order = 0;
                foreach (var evidenceDType in assertionDType.Evidence)
                {
                    order++;
                    var evidenceModel = EvidenceModel.FromDType(evidenceDType);
                    if (evidenceDType.Artifacts != null)
                    {
                        var eOrder = 0;
                        foreach (var artifactDType in evidenceDType.Artifacts)
                        {
                            eOrder++;
                            var artifactModel = ArtifactModel.FromArtifactDType(artifactDType);
                            var evidenceArtifact = EvidenceArtifact.Combine(evidenceModel, artifactModel, eOrder);
                            evidenceModel.EvidenceArtifacts.Add(evidenceArtifact);
                        }
                    }
                    var assertionEvidence = AssertionEvidence.Combine(assertionModel, evidenceModel, order);
                    assertionModel.AssertionEvidences.Add(assertionEvidence);
                }
            }
        }

        public AchievementModel ConvertAchievement(AchievementDType achievementDType)
        {
            
            var achievementModel = AchievementModel.FromDType(achievementDType);
            achievementModel.Issuer = ProfileModel.FromDType(achievementDType.Issuer);
            if (achievementDType.Requirement != null)
            {
                achievementModel.Requirement = CriteriaModel.FromDType(achievementDType.Requirement);
            }
            var order = 0;
            if (achievementDType.Alignments != null)
            {
                var alignmentModels = ConvertAlignments(achievementDType.Alignments);
                foreach (var alignmentModel in alignmentModels)
                {
                    order++;

                    var achievementAlignment = AchievementAlignment.Combine(achievementModel, alignmentModel, order);
                    achievementModel.AchievementAlignments.Add(achievementAlignment);
                }
            }
            if (achievementDType.Associations != null)
            {
                order = 0;
                foreach (var associationDType in achievementDType.Associations)
                {
                    order++;
                    var associationModel = AssociationModel.FromDType(associationDType);

                    var achievementAssociation = AchievementAssociation.Combine(achievementModel, associationModel, order);
                    achievementModel.AchievementAssociations.Add(achievementAssociation);
                }
            }
            if (achievementDType.ResultDescriptions != null)
            {
                order = 0;
                foreach (var resultDescriptionDType in achievementDType.ResultDescriptions)
                {
                    order++;
                    var resultDescriptionModel = ResultDescriptionModel.FromDType(resultDescriptionDType);
                    resultDescriptionModel.Order = order;
                    if (resultDescriptionDType.RubricCriterionLevels != null)
                    {
                        var order2 = 0;
                        foreach (var rcl in resultDescriptionDType.RubricCriterionLevels)
                        {
                            order2++;
                            var rclModel = RubricCriterionLevelModel.FromDType(rcl);
                            rclModel.Order = order2;
                            var order3 = 0;
                            if (rcl.Alignments != null)
                            {
                                var alignmentModels = ConvertAlignments(rcl.Alignments);
                                foreach (var alignmentModel in alignmentModels)
                                {
                                    order3++;

                                    var rclAlignment = RubricCriterionLevelAlignment.Combine(rclModel, alignmentModel, order3);
                                    rclModel.RubricCriterionLevelAlignments.Add(rclAlignment);
                                }
                            }
                            resultDescriptionModel.RubricCriterionLevels.Add(rclModel);
                        }
                    }
                    achievementModel.ResultDescriptions.Add(resultDescriptionModel);
                }
            }
            if (achievementDType.SignedEndorsements != null)
            {
                var endorsementModels = ConvertSignedEndorsements(achievementDType.SignedEndorsements);
                order = 0;
                foreach (var endorsementModel in endorsementModels)
                {
                    order++;
                    var achievementEndorsement = AchievementEndorsement.Combine(achievementModel, endorsementModel, order);
                    achievementModel.AchievementEndorsements.Add(achievementEndorsement);
                }
            }
            if (achievementDType.Endorsements != null)
            {
                var endorsementModels = ConvertEndorsements(achievementDType.Endorsements);
                foreach (var endorsementModel in endorsementModels)
                {
                    order++;
                    var achievementEndorsement = AchievementEndorsement.Combine(achievementModel, endorsementModel, order);
                    achievementModel.AchievementEndorsements.Add(achievementEndorsement);
                }
            }
            return achievementModel;
        }

        public ClrModel ConvertClr(ClrDType clr, string signedClr = null, AuthorizationModel authorization = null, CredentialPackageModel package = null)
        {
            var clrEntity = ClrModel.FromDType(clr, System.Text.Json.JsonSerializer.Serialize(clr, new JsonSerializerOptions { IgnoreNullValues = true }), signedClr);

            clrEntity.IssuedOn = clr.IssuedOn;

            AddClrAchievements(ref clrEntity, clr);

            AddClrAssertions(ref clrEntity, clr);

            var order = 0;
            if (clr.SignedEndorsements != null)
            {
                var endorsementModels = ConvertSignedEndorsements(clr.SignedEndorsements);
                order = 0;
                foreach (var endorsementModel in endorsementModels)
                {
                    order++;
                    var clrEndorsement = ClrEndorsement.Combine(clrEntity.ClrId, clrEntity, endorsementModel, order);
                    clrEntity.ClrEndorsements.Add(clrEndorsement);
                }
            }
            if (clr.Endorsements != null)
            {
                var endorsementModels = ConvertEndorsements(clr.Endorsements);
                foreach (var endorsementModel in endorsementModels)
                {
                    order++;
                    var clrEndorsement = ClrEndorsement.Combine(clrEntity.ClrId, clrEntity, endorsementModel, order);
                    clrEntity.ClrEndorsements.Add(clrEndorsement);
                }
            }
            clrEntity.Learner = ProfileModel.FromDType(clr.Learner);
            clrEntity.Publisher = ProfileModel.FromDType(clr.Publisher);
            clrEntity.Verification = VerificationModel.FromDType(clr.Verification);
            clrEntity.LearnerName = clr.Learner.Name;
            clrEntity.PublisherName = clr.Publisher.Name;
            clrEntity.RefreshedAt = DateTime.Now;
            if (package !=null)
            {
                clrEntity.CredentialPackage = package;
            }
            if (authorization != null)
            {
                clrEntity.Authorization = authorization;
                clrEntity.AuthorizationForeignKey = authorization.Id;
            }

            clrEntity.AssertionsCount = (clr.Assertions?.Count ?? 0)
                                    + (clr.SignedAssertions?.Count ?? 0);

            BuildAssociatedAssertionTree(ref clrEntity);

            return clrEntity;
        }
        public List<EndorsementModel> ConvertEndorsements(List<EndorsementDType> endorsementDTypes)
        {
            var endorsements = new List<EndorsementModel>();
            foreach (var endorsementDType in endorsementDTypes)
            {
                var endorsementModel = EndorsementModel.FromDType(endorsementDType);
                endorsementModel.Issuer = ProfileModel.FromDType(endorsementDType.Issuer, true);
                endorsementModel.Verification = VerificationModel.FromDType(endorsementDType.Verification);
                endorsementModel.EndorsementClaim = EndorsementClaimModel.FromDType(endorsementDType.Claim);
                //NB EndorsementProfiles cannot have child endorsements like Profiles do                
                endorsements.Add(endorsementModel);
            }
            return endorsements;
        }
        public List<EndorsementModel> ConvertSignedEndorsements(List<string> signedEndorsements)
        {
            var endorsements = new List<EndorsementModel>();
            foreach (var endorsement in signedEndorsements)
            {
                var endorsementDType = endorsement.DeserializePayload<EndorsementDType>();
                var endorsementModel = EndorsementModel.FromDType(endorsementDType, endorsement);
                endorsementModel.Issuer = ProfileModel.FromDType(endorsementDType.Issuer, true);
                endorsementModel.Verification = VerificationModel.FromDType(endorsementDType.Verification);
                endorsementModel.EndorsementClaim = EndorsementClaimModel.FromDType(endorsementDType.Claim);
                //NB EndorsementProfiles cannot have child endorsements like Profiles do                
                endorsements.Add(endorsementModel);
            }
            return endorsements;
        }

        public List<AlignmentModel> ConvertAlignments(List<AlignmentDType> alignmentDTypes)
        {
            var alignments = new List<AlignmentModel>();
            var order = 0;
            foreach (var alignmentDType in alignmentDTypes)
            {
                order++;
                var alignmentModel = AlignmentModel.FromDType(alignmentDType);
                alignments.Add(alignmentModel);
            }
            return alignments;
        }

        public async Task DeletePackageAsync(int id)
        {
            var item = await _context.CredentialPackages
                    .SingleAsync(x => x.Id == id);

            item.Delete();
            IEnumerable<IBaseEntity> kids = new List<IBaseEntity>();
            IEnumerable<ClrModel> pkgClrs = new List<ClrModel>();

            switch (item.TypeId)
            {
                case PackageTypeEnum.Collection:
                case PackageTypeEnum.Clr:
                    kids = await _context.Clrs
                        .Where(x => x.ParentCredentialPackageId == id)
                        .ToListAsync();
                    break;
                case PackageTypeEnum.ClrSet:
                    kids = await _context.ClrSets
                        .Where(x => x.ParentCredentialPackageId == id)
                        .ToListAsync();
                    break;
                case PackageTypeEnum.VerifiableCredential:
                    kids = await _context.VerifiableCredentials
                        .Include(vc => vc.Clrs)
                        .Include(vc => vc.ClrSets)
                        .ThenInclude(cls => cls.Clrs)
                        .Where(x => x.ParentCredentialPackageId == id)
                        .ToListAsync();
                    break;
                case PackageTypeEnum.OpenBadgeConnect:
                case PackageTypeEnum.OpenBadge:
                    kids = await _context.BadgrBackpacks
                        .Include(b => b.BadgrAssertions)
                        .Where(x => x.ParentCredentialPackageId == id)
                        .ToListAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"DeletePackageAsync invalid packagetype: {item.TypeId}.");
            }

            foreach (var kid in kids)
            {
                kid.Delete();
                if (item.TypeId == PackageTypeEnum.Clr || item.TypeId == PackageTypeEnum.Collection) {
                    var clr = kid as ClrModel;
                    await RemoveLinksAsync(clr);
                }
                if (item.TypeId == PackageTypeEnum.OpenBadgeConnect)
                {
                    foreach (var bas in ((BadgrBackpackModel)kid).BadgrAssertions)
                    {
                        bas.Delete();
                    }
                }
                else if (item.TypeId == PackageTypeEnum.VerifiableCredential)
                {
                    var vc = kid as VerifiableCredentialModel;

                    foreach(var clr in vc.Clrs)
                    {
                        clr.Delete();
                        await RemoveLinksAsync(clr);
                    }

                    foreach (var cs in vc.ClrSets)
                    {
                        cs.Delete();
                        foreach (var clr in cs.Clrs)
                        {
                            clr.Delete();
                            await RemoveLinksAsync(clr);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            async Task RemoveLinksAsync(ClrModel clr)
            {
                if (await _context.Links.AnyAsync(l => l.ClrForeignKey == clr.ClrId))
                {
                    var link = await _context
                        .Links
                        .Include(l => l.Shares)
                        .FirstOrDefaultAsync(l => l.ClrForeignKey == clr.ClrId);
                    link.IsDeleted = true;
                    link.ModifiedAt = DateTime.UtcNow;
                    link.Shares.ForEach(s => s.StatusId = StatusEnum.Deleted);
                }
            }
        }

        /// <summary>
        /// Embed a badge in a new CLR
        /// </summary>
        public async Task<int> CreateClrFromBadgeAsync(int id, ApplicationUser user)
        {
            var badge = await _context.BadgrAssertions.AsNoTracking()
                .Include(a => a.BadgrBackpack)
                .ThenInclude(bp => bp.ParentCredentialPackage)
                .ThenInclude(cp => cp.Authorization)
                .ThenInclude(a => a.Source)
                .Where(a => a.BadgrAssertionId == id)
                .FirstOrDefaultAsync();

            var newClrDType = new ClrDType
            {
                Context = ClrConstants.JsonLd.Context,
                Type = ClrConstants.Type.Clr,
                Assertions = new List<AssertionDType>(),
                Id = $"urn:uuid:{Guid.NewGuid():D}",
                IssuedOn = DateTime.Now.ToLocalTime(),
                SignedAssertions = new List<string>()
            };
            var emailAddress = (string)null;
            var email = (BadgrUserEmailDType)null;
            ProfileDType learnerProfile;
            if (String.IsNullOrEmpty(badge.RecipientJson))
            {
                var tele = user.PhoneNumber;
                emailAddress = user.Email;
                learnerProfile = new ProfileDType()
                {
                    Type = "Profile",
                    Id = user.Id,
                    Email = emailAddress,
                    Name = user.UserName,
                    Telephone = tele,
                    SourcedId = user.Id,
                    Url = user.ProfileImageUrl
                };
            }
            else
            {
                var badgrUserResponse = JsonConvert.DeserializeObject<BadgrUserResponse>(badge.RecipientJson);
                var badgrUser = badgrUserResponse.BadgrBadgrUsers.FirstOrDefault();
                if (badgrUser == null)
                {
                    badgrUser = new BadgrUserDType();
                }
                if (badgrUser.Emails.Count > 0)
                {
                    email = badgrUser.Emails.Where(e => e.Primary).FirstOrDefault();
                    if (email == null)
                    {
                        email = badgrUser.Emails.Where(e => e.Primary).FirstOrDefault();
                    }
                    if (email != null)
                    {
                        emailAddress = email.Email;
                    }
                }

                var tele = badgrUser.Telephone.FirstOrDefault();
                learnerProfile = new ProfileDType()
                {
                    Type = "Profile",
                    Id = badgrUser.Id,
                    Email = emailAddress,
                    FamilyName = badgrUser.LastName,
                    GivenName = badgrUser.FirstName,
                    Name = badgrUser.FirstName + " " + badgrUser.LastName,
                    Telephone = tele,
                    SourcedId = badgrUser.Id,
                    Url = badgrUser.Url.FirstOrDefault()
                };
                await _context.Profiles.AddAsync(ProfileModel.FromDType(learnerProfile));
            }

            newClrDType.Learner = learnerProfile;
            var badgrIssuer = JsonConvert.DeserializeObject<BadgrIssuerDType>(badge.IssuerJson);
            if (badgrIssuer == null)
            {
                badgrIssuer = new BadgrIssuerDType();
            }
            var issuerProfile = new ProfileDType()
            {
                Type = "Profile",
                Id = badgrIssuer.Id,
                Email = badgrIssuer.Email,
                Name = badgrIssuer.Name,
                SourcedId = badgrIssuer.Id,
                Url = badgrIssuer.Url,
                Description = badgrIssuer.Description
            };
            var publisherId = Guid.Parse("421D5B30-F74A-479E-AC43-6DAF21429011");
            HostSettings hostSettings;
            try
            {
                hostSettings = _hostSettings?.Value;
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, ex.Message);
                hostSettings = new HostSettings { ClientName = "Default Client", DnsName = "https://localhost" };
            }
            var publisherProfile = new ProfileDType
            {
                Type = "Profile",
                Id = $"urn:uuid:{publisherId:D}",
                SourcedId = $"urn:uuid:{publisherId:D}",
                Name = hostSettings.ClientName,
                Url = $"https://{hostSettings.DnsName}"
            };

            newClrDType.Publisher = publisherProfile;

            newClrDType.Name = badgrIssuer.Name + " Issued Badges";

            var addlProperties = new Dictionary<string, object>();

            var badgeclassLD = SchemaSerializer.DeserializeObject<dynamic>(badge.BadgeClassJson);

            var badgeClass = JsonConvert.DeserializeObject<BadgrBadgeClassDType>(badgeclassLD.ToString());

            var img = "";
            if (badgeClass.Image.GetType() == typeof(JObject))
            {
                img = badgeClass.Image["id"];
            }
            else
            {
                img = badgeClass.Image.ToString();
            }

            var achievement = new AchievementDType()
            {
                Id = badgeClass.Id,
                AchievementType = "Badge",
                Description = badgeClass.Description,
                Image = img,
                Issuer = issuerProfile,
                Name = badgeClass.Name,
                Alignments = badgeClass.Alignments,
                Tags = badgeClass.Tags
            };

            var assertion = new AssertionDType()
            {
                Achievement = achievement,
                Id = badge.OpenBadgeId,
                Type = "Assertion",
                IssuedOn = badge.IssuedOn,
                Recipient = badge.Recipient,
                Narrative = badge.Narrative,
                AdditionalProperties = badge.AdditionalProperties,
                Revoked = badge.Revoked,
                RevocationReason = badge.RevocationReason
            };

            newClrDType.Assertions.Add(assertion);

            var credentialPackage = new CredentialPackageModel()
            {
                UserId = user.Id,
                TypeId = PackageTypeEnum.Clr,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            await _context.CredentialPackages.AddAsync(credentialPackage);
            await _context.SaveChangesAsync();

            // Save each CLR
            var clr = null as ClrModel;
            clr = ConvertClr(newClrDType, null, null, credentialPackage);
            clr.ParentCredentialPackageId = credentialPackage.Id;
            if (_context.Entry(clr).State == EntityState.Detached)
            {
                credentialPackage.Clr = clr;
            }

            credentialPackage.AssertionsCount = clr.AssertionsCount;

            await _context.SaveChangesAsync();

            await EnhanceArtifactsAsync(credentialPackage);

            return credentialPackage.Id;
        }

        private void BuildAssociatedAssertionTree(ref ClrModel clr)
        {
            foreach (var assertion in clr.ClrAssertions.Select(ca => ca.Assertion))            
            {
                if (assertion.Achievement?.AchievementAssociations == null) continue;

                foreach (var association in assertion.Achievement.AchievementAssociations.Select(aa => aa.Association))
                {
                    if (association.AssociationType ==
                        AssociationDType.AssociationTypeEnum.IsChildOfEnum
                        || association.AssociationType ==
                        AssociationDType.AssociationTypeEnum.IsPartOfEnum)
                    {
                        if (association.TargetId == assertion.Achievement.Id) continue;

                        var parentAssertion = clr.ClrAssertions.Select(ca => ca.Assertion)
                            .SingleOrDefault(a => a.Achievement.Id == association.TargetId);

                        if (parentAssertion != null)
                        {
                            assertion.ParentAssertion = parentAssertion;
                            //if (!parentAssertion.ChildAssertions.Contains(assertion))
                            //{
                            //    parentAssertion.ChildAssertions.Add(assertion);
                            //}
                        }
                    }

                    if (association.AssociationType ==
                        AssociationDType.AssociationTypeEnum.IsParentOfEnum)
                    {
                        foreach (var childAssertion in clr.ClrAssertions.Select(ca => ca.Assertion).Where(x => x.Achievement.Id == association.TargetId))
                        {

                            if (!assertion.ChildAssertions.Contains(childAssertion))
                            {
                                childAssertion.ParentAssertion = assertion;
                                //assertion.ChildAssertions.Add(childAssertion);
                            }
                        }
                    }
                }
            }
            foreach (var asrt in clr.ClrAssertions.Select(ca => ca.Assertion))
            {
                if (asrt.Achievement != null && asrt.Achievement.Issuer != null)
                {
                    if (asrt.Achievement.Issuer.Id == clr.Learner.Id)
                    {
                        asrt.IsSelfPublished = true;
                    }
                }
            }
        }
        private async Task EnhanceArtifactsAsync(CredentialPackageModel pkg)
        {
            var clrIds = pkg.ContainedClrs.Select(c => c.ClrId).ToList();

            if (!clrIds.Any())
                return;

            var clrs = await _context.Clrs.Where(clr => clrIds.Contains(clr.ClrId)).ToDictionaryAsync(clr => clr.ClrId);

            var artifacts = await _context.Artifacts
                .Include(a => a.EvidenceArtifact)
                .ThenInclude(ea => ea.Evidence)
                .ThenInclude(e => e.AssertionEvidence)
                .ThenInclude(ae => ae.Assertion)
                .ThenInclude(a => a.ClrAssertion)
                .Where(a => a.ClrId.HasValue && clrIds.Contains(a.ClrId.Value))
                .ToListAsync();

            foreach (var art in artifacts)
            {
                var clr = clrs[art.ClrId.Value];
                var eaf = EnhancedArtifactFields.FromArtifact(clr, art);
                art.ClrId = eaf.ClrId;
                art.AssertionId = eaf.AssertionId;
                art.ClrIssuedOn = eaf.ClrIssuedOn;
                art.ClrName = eaf.ClrName;
                art.EvidenceName = eaf.EvidenceName;
            }
            await _context.SaveChangesAsync();

        }

        private async Task<IEnumerable<ClrModel>> GetPackageClrsAsync(int id)
        {
            var clrs = await _context.Clrs
                .AsNoTracking()
                .Where(clr => clr.CredentialPackageId == id)
                .OrderByDescending(x => x.IssuedOn)
                .ToListAsync();

            return clrs;
        }
    }
}
