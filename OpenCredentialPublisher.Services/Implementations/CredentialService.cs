using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Abstracts;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.ViewModels.nG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using QRCoder;
using OpenCredentialPublisher.ClrLibrary;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;

        private readonly ETLService _etlService;

        public CredentialService(WalletDbContext context, SchemaService schemaService, ETLService etlService)
        {
            _context = context;
            _schemaService = schemaService;
            _etlService = etlService;
        }

        public async Task<List<LinkModel>> GetAllLinksAsync(string userId)
        {
            var result = await _context.Links.AsNoTracking()
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }

        public async Task<bool> CredentialPackageHasPdfAsync(int id)
        {
            return await _context.CredentialPackageArtifactView.AnyAsync(cp => cp.CredentialPackageId == id && cp.IsPdf && !cp.IsDeleted && !cp.Revoked);
        }

        public async Task<CredentialPackageArtifactView> CredentialPackagePdfArtifactAsync(int id)
        {
            return await _context.CredentialPackageArtifactView.AsNoTracking().FirstOrDefaultAsync(cp => cp.CredentialPackageId == id && cp.IsPdf && !cp.IsDeleted && !cp.Revoked);
        }

        public async Task<ArtifactModel> GetClrFirstPdfArtifactAsync(int id)
        {
            return await _context.Artifacts
                .Include(a => a.EvidenceArtifact)
                .ThenInclude(ea => ea.Evidence)
                .ThenInclude(e => e.AssertionEvidence)
                .ThenInclude(ae => ae.Assertion)
                .Where(a => a.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.ClrId == id
                    && a.IsPdf)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<ArtifactModel> GetClrTranscriptArtifactAsync(int id)
        {
            return await _context.Artifacts
                .Include(a => a.EvidenceArtifact)
                .ThenInclude(ea => ea.Evidence)
                .ThenInclude(e => e.AssertionEvidence)
                .ThenInclude(ae => ae.Assertion)
                .Where(a => a.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.ClrId == id
                    && a.IsPdf && a.NameContainsTranscript)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();
        }
        public async Task<PdfShareViewModel> GetNewestPdfTranscriptAsync(string userId)
        {
            var artifact = await _context.Artifacts
                .Include(a => a.EvidenceArtifact)
                .ThenInclude(ea => ea.Evidence)
                .ThenInclude(e => e.AssertionEvidence)
                .ThenInclude(ae => ae.Assertion)
                .ThenInclude(a => a.ClrAssertion)
                .ThenInclude(ca => ca.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .Where(a => a.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.Clr.CredentialPackage.UserId == userId
                    && a.IsPdf && a.NameContainsTranscript)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();

            if (artifact != null)
            {
                return PdfShareViewModel.FromArtifact(artifact);
            }
            return null;
        }
        public async Task<PackageVM> GetPackageVMAsync(int id, string userId)
        {
            var cp = await _context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.ContainedClrs)
                .Where(cp => cp.Id == id)
                .OrderBy(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var clrIds = cp.ContainedClrs
                .Select(c => c.ClrId)
                .ToList();

            var latestTranscript = await _context.Artifacts.AsNoTracking()
                .Where(a => a.IsPdf && a.NameContainsTranscript && clrIds.Contains(a.ClrId.Value))
                .OrderByDescending(a => a.ClrIssuedOn)
                .FirstOrDefaultAsync();

            var pkgVM = new PackageVM
                {
                    Id = cp.Id,
                    TypeId = cp.TypeId,
                    AssertionCount = cp.AssertionsCount,
                    CreatedAt = cp.CreatedAt,
                    ModifiedAt = cp.ModifiedAt,
                    ShowDownloadPdfButton = latestTranscript != null,
                    ShowDownloadVCJsonButton = cp.TypeId == PackageTypeEnum.VerifiableCredential,
                    ClrIds = clrIds,
                    IsOwner = userId == cp.UserId,
                    NewestPdfTranscript = PdfShareViewModel.FromArtifact(latestTranscript)
            };

            return pkgVM;
        }
        public async Task<List<PackageVM>> GetPackageVMListAsync(string userId)
        {
            var packages = await _context.CredentialPackages.AsNoTracking()
                .Where(cp => cp.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var pkgVMs = new List<PackageVM>();

            foreach (var cp in packages)
            {
                var pkgVM = new PackageVM
                {
                    Id = cp.Id,
                    TypeId = cp.TypeId,
                    AssertionCount = cp.AssertionsCount,
                    CreatedAt = cp.CreatedAt,
                    ModifiedAt = cp.ModifiedAt,
                    ShowDownloadPdfButton = true,
                    ShowDownloadVCJsonButton = true,
                    IsOwner = userId == cp.UserId
                };
                pkgVMs.Add(pkgVM);
            }

            return pkgVMs;
        }
        public async Task<int> GetVerifiableCredentialCountAsync(string userId)
        {
            return await _context.CredentialPackages.AsNoTracking()
                .Where(cp => cp.UserId == userId && cp.TypeId == PackageTypeEnum.VerifiableCredential)                
                .CountAsync();
        }
        public async Task<int> GetAssertionsCountAsync(string userId)
        {
            return await _context.CredentialPackages.AsNoTracking()
                .Where(cp => cp.UserId == userId)
                .Select(cp => cp.AssertionsCount)
                .SumAsync();
        }
        public async Task<IEnumerable<ClrModel>> GetPackageClrsAsync(int id)
        {
            var clrs = await _context.Clrs
                .AsNoTracking()
                .Where(clr => clr.CredentialPackageId == id)
                .OrderByDescending(x => x.IssuedOn)
                .ToListAsync();

            return clrs;
        }
        public async Task<AssertionModel> GetClrAssertionDetailAsync(int clrId, string id)
        {
            var assertion = await _context.Assertions.AsNoTracking()
                .Include(a => a.ClrAssertion)
                .Include(a => a.Results)
                .Include(a => a.Achievement)
                .ThenInclude(aa => aa.ResultDescriptions)
                .Include(a => a.Achievement)
                .ThenInclude(aa => aa.Issuer)
                .Where(a => a.ClrAssertion.ClrId == clrId && a.Id == id)
                .FirstOrDefaultAsync();

            return assertion;
        }
        public async Task<AssertionModel> GetClrAssertionAsync(int id)
        {
            var assertion = await _context.Assertions
                .AsNoTracking()
                .Where(assertion => assertion.AssertionId == id)
                .FirstOrDefaultAsync();

            return assertion;
        }
        public async Task<IEnumerable<AssertionModel>> GetClrAssertionsAsync(int id)
        {
            var assertions = await _context.Assertions.AsNoTracking()
                .Include(a => a.Achievement)
                .Where(a => a.ClrAssertion.ClrId == id)
                .OrderBy(a => a.AssertionId)
                .ToListAsync();

            return assertions;
        }

        public async Task<ClrModel> GetSingleClrAsync(int id)
        {
            var clr = await _context.Clrs.AsNoTracking()
                .Include(c => c.Learner)
                .Include(c => c.Publisher)
                .Include(c => c.SmartResume)
                .Include(c => c.ClrAchievements)
                .ThenInclude(c => c.Achievement)
                .Where(clr => clr.ClrId == id)
                .FirstOrDefaultAsync();

            return clr;
        }
        
        public async Task<List<int>> GetPackageClrIdsAsync(int id)
        {
            var clrs = await _context.Clrs.AsNoTracking()      
                .Where(clr => clr.CredentialPackageId == id)
                .OrderByDescending(x => x.IssuedOn)
                .Select(clr => clr.ClrId)
                .ToListAsync();

            return clrs;
        }

        public async Task<List<ClrModel>> GetAllClrsShallowAsync(string userId)
        {
            return await _context.Clrs.AsNoTracking()
                .Include(c => c.Learner)
                .Include(c => c.Publisher)
                .Include(clr => clr.CredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => c.CredentialPackage.UserId == userId).ToListAsync();
        }
        public async Task<SendWalletVM> GetSendWalletVMAsync(ModelStateDictionary modelState, string userId, int id)
        {
            var packages = await _context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.ContainedClrs)
                .Include(cp => cp.VerifiableCredential)
                .Where(cp => cp.UserId == userId && !cp.Revoked && cp.TypeId != PackageTypeEnum.OpenBadge && cp.TypeId != PackageTypeEnum.OpenBadgeConnect)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var vm = new SendWalletVM();

            var wallet = await _context.WalletRelationships.AsNoTracking()
                .Include(w => w.CredentialRequests)
                .Where(w => w.UserId == userId && w.Id == id)
                .FirstOrDefaultAsync();

            if (wallet == null)
            {
                modelState.AddModelError("", "No wallet found for that Id.");
                return vm;
            }
            else
            {
                var countDictionary = wallet.CredentialRequests.Where(cr => cr.CredentialRequestStep == CredentialRequestStepEnum.OfferAccepted).GroupBy(cr => cr.CredentialPackageId).ToDictionary(cr => cr.Key, cr => cr.Count());
                
                foreach (var cp in packages)
                {
                    var credVM = new WalletCredentialVM(cp);
                    credVM.TimesSent = countDictionary.ContainsKey(credVM.Id) ? countDictionary[credVM.Id] : 0;
                    vm.Credentials.Add(credVM);
                }

                vm.Credentials = vm.Credentials.OrderByDescending(c => Convert.ToDateTime(c.DateAdded)).ToList();
            }
            vm.Connection = new ConnectionViewModel { Name = wallet.WalletName, Id = id, RelationshipDid = wallet.RelationshipDid, DateCreated = wallet.CreatedAt.ToString("g") };

            return vm;
        }

        public string CreateQRCode(string url)
        {
            using var generator = new QRCodeGenerator();
            var codeData = generator.CreateQrCode($"{url}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(codeData);
            var qrCodeBytes = qrCode.GetGraphic(4);
            return Convert.ToBase64String(qrCodeBytes);
        }


        public async Task<ClrModel> GetClrForDeletionAsync(int id)
        {
            return await _context.Clrs.AsNoTracking().IgnoreQueryFilters()
                .Include(c => c.SmartResume)
                .Include(c => c.Artifacts)
                    .ThenInclude(a => a.EvidenceArtifact)
                .Include(a => a.ClrAssertions)
                    .ThenInclude(ca => ca.Assertion)
                        .ThenInclude(a => a.AssertionEvidences)
                        .ThenInclude(e => e.Evidence)
                .Include(a => a.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .ThenInclude(a => a.Results)
                .Include(a => a.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .ThenInclude(a => a.Achievement)
                .ThenInclude(aa => aa.ResultDescriptions)
                .Include(a => a.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .ThenInclude(a => a.Achievement)
                .ThenInclude(aa => aa.Issuer)
                .Include(x => x.Authorization)
                .Include(x => x.Verification)
                //All Endorsements
                .Include(c => c.Learner)
                .ThenInclude(l => l.ProfileEndorsements)
                .ThenInclude(pe => pe.Endorsement)
                .Include(c => c.Publisher)
                .ThenInclude(p => p.ProfileEndorsements)
                .ThenInclude(pe => pe.Endorsement)
                .Include(a => a.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .ThenInclude(a => a.AssertionEndorsements)
                .ThenInclude(a => a.Endorsement)
                .Include(a => a.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .ThenInclude(a => a.Achievement)
                .ThenInclude(a => a.AchievementEndorsements)
                .ThenInclude(ae => ae.Endorsement)
                .Include(a => a.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .ThenInclude(a => a.Achievement)
                .ThenInclude(aa => aa.Issuer)
                .ThenInclude(p => p.ProfileEndorsements)
                .ThenInclude(pe => pe.Endorsement)

                .SingleAsync(x => x.ClrId == id);
        }


        public async Task<AssertionModel> GetAssertionForVerificationAsync(int clrId, string id)
        {
            return await _context.Assertions.AsNoTracking()
                .Include(a => a.Achievement)
                .Include(x => x.Verification)
                .Where(a => a.Id == id && a.ClrAssertion.ClrId == clrId)
                .FirstOrDefaultAsync();
        }
        public async Task<EndorsementModel> GetEndorsementForVerificationAsync(int clrId, string id, VerifyVM data)
        {
            var endorsement = null as EndorsementModel;

            if (data.Ancestors.EndsWith("clr.endorsement"))
            {
                return await _context.Endorsements.AsNoTracking()
                    .Include(x => x.Verification)
                    .Include(x => x.Issuer)
                    .Where(e => e.Id == id && e.ClrEndorsement.ClrId == clrId)
                    .FirstOrDefaultAsync();
            }
            if (data.Ancestors.EndsWith("clr.assertion.endorsement"))
            {
                return await _context.Endorsements.AsNoTracking()
                    .Include(x => x.Verification)
                    .Include(x => x.Issuer)
                    .Where(e => e.Id == id && e.AssertionEndorsement.Assertion.ClrAssertion.ClrId == clrId)
                    .FirstOrDefaultAsync();
            }
            if (data.Ancestors.EndsWith("clr.assertion.achievement.endorsement"))
            {
                return await _context.Endorsements.AsNoTracking()
                    .Include(x => x.Verification)
                    .Include(x => x.Issuer)
                    .Where(e => e.Id == id && e.AchievementEndorsement.Achievement.Assertion.ClrAssertion.ClrId == clrId)
                    .FirstOrDefaultAsync();
            }
            if (data.Ancestors.EndsWith(".profile"))
            {
                if (data.Ancestors.EndsWith("clr.endorsement.profile"))
                {
                    var clr = await _context.Clrs.AsNoTracking()
                    .Include(x => x.Learner)
                    .ThenInclude(x => x.ProfileEndorsements)
                    .ThenInclude(pe => pe.Endorsement)
                    .ThenInclude(x => x.Issuer)
                    .Where(c => c.ClrId == clrId)
                    .FirstOrDefaultAsync();

                    if (clr != null)
                    {
                        endorsement = clr.Learner.ProfileEndorsements.Where(pe => pe.Endorsement.Id == id)
                            .Select(pe => pe.Endorsement)
                            .FirstOrDefault();
                    }

                }

                if (data.Ancestors.EndsWith("clr.endorsement.profile") && endorsement == null)
                {
                    var clr = await _context.Clrs.AsNoTracking()
                        .Include(x => x.Publisher)
                        .ThenInclude(x => x.ProfileEndorsements)
                        .ThenInclude(pe => pe.Endorsement)
                        .ThenInclude(x => x.Issuer)
                        .Where(c => c.ClrId == clrId)
                        .FirstOrDefaultAsync();

                    if (clr != null)
                    {
                        endorsement = clr.Publisher.ProfileEndorsements.Where(pe => pe.Endorsement.Id == id)
                            .Select(pe => pe.Endorsement)
                            .FirstOrDefault();
                    }

                }
                return endorsement;
            }

            if (data.Ancestors.EndsWith("clr.achievement.endorsement"))
            {
                return await _context.Endorsements.AsNoTracking()
                    .Include(x => x.Issuer)
                    .Where(e => e.AchievementEndorsement.Achievement.ClrAchievement.ClrId == clrId && e.Id == id)
                    .FirstOrDefaultAsync();
            }
            throw new ArgumentOutOfRangeException($"GetEndorsementForVerificationAsync unexpected Ancestors path ${data.Ancestors}");
        }

        public async Task<ClrModel> GetClrAsync(int id)
        {
            return await _context.Clrs
                    .AsNoTracking()
                    .Include(x => x.Verification)
                    .Include(x => x.Publisher)
                    .Include(x => x.CredentialPackage)
                    .Include(x => x.Authorization)
                    .ThenInclude(x => x.Source)
                    .SingleAsync(x => x.ClrId == id);
        }
        //End V2 *************************************************************************************************
        [Obsolete]
        public IEnumerable<ClrAssertion> GetNotPersistedClrAssertions(ClrModel clr, ClrDType clrDType)
        {
            var order = 0;
            var clrAssertions = new List<ClrAssertion>();
            if (clrDType.SignedAssertions != null)
            {
                foreach (var asrt in clrDType.SignedAssertions)
                {
                    order++;
                    var decoded = asrt.DeserializePayload<AssertionDType>();
                    var signedAsrt = AssertionModel.FromDTypeShallow(decoded, asrt);
                    var clrAssertion = ClrAssertion.Combine(clr, signedAsrt, order);
                    clrAssertions.Add(clrAssertion);
                }
            }
            if (clrDType.Assertions != null)
            {
                foreach (var asrt in clrDType.Assertions)
                {
                    order++;
                    var clrAsrt = AssertionModel.FromDTypeShallow(asrt);
                    var clrAssertion = ClrAssertion.Combine(clr, clrAsrt, order);
                    clrAssertions.Add(clrAssertion);
                }
            }
            return clrAssertions;
        }
        public async Task<List<CredentialPackageModel>> GetAllAsync(string userId)
        {
            var packages = await GetAllDeep(userId)
                .ToListAsync();

            return packages;
        }
        public IQueryable<CredentialPackageModel> GetAllDeep(string userId)
        {
            var packages = _context.CredentialPackages
                .Include(cp => cp.Authorization)
                .ThenInclude(auth => auth.Source)
                .AsNoTracking()
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(clra => clra.Source)
                .Include(cp => cp.ClrSet)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.Clr)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.BadgrBackpack)
                .ThenInclude(bp => bp.BadgrAssertions)
                .Where(package => package.UserId == userId)
                .OrderBy(x => x.CreatedAt);

            return packages;
        }
        // DO NOT wire this up to any controllers !!!
        public async Task<IEnumerable<int>> GetPackageUniverseIdsAsync()
        {
            var pkgIds = await _context.CredentialPackages.AsNoTracking()
                .Select(p => p.Id)
                .ToListAsync();

            return pkgIds;
        }
        public IQueryable<CredentialPackageModel> GetAllShallow(string userId)
        {
            var packages = _context.CredentialPackages
                .AsNoTracking()
                .Where(package => package.UserId == userId)
                .OrderBy(x => x.CreatedAt);

            return packages;
        }
        public async Task<CredentialPackageModel> GetShallowAsync(string userId, int id)
        {
            var package = await GetAllShallow(userId)
                .Where(package => package.Id == id)
                .FirstOrDefaultAsync();

            return package ?? new CredentialPackageModel();
        }
        public async Task<int> GetPackageAssertionCountAsync(int id)
        {
            var count = await _context.Clrs.AsNoTracking()
                .Where(clr => clr.CredentialPackageId == id)
                .Select(clr => clr.AssertionsCount)
                .SumAsync();

            return count;
        }
        public async Task<List<int>> GetClrPackageClrIdAsync(int id)
        {
            var clr = await _context.Clrs
                .AsNoTracking()
                .Where(clr => clr.ParentCredentialPackageId == id)
                .Select(clr => clr.ClrId)
                .FirstOrDefaultAsync();

            return new List<int> { clr };
        }
        public async Task<VerifiableCredentialVM> GetPackageVerifiableCredentialVMAsync(int id)
        {
            var vc = await _context.VerifiableCredentials.AsNoTracking()
                .Include(vc => vc.ClrSets)
                .Include(clrSets => clrSets.Clrs)
                .Where(vc => vc.ParentCredentialPackageId == id)
                .OrderByDescending(x => x.IssuedOn)
                .FirstOrDefaultAsync();

            var vm = VerifiableCredentialVM.FromVC(vc);

            return vm;
        }
        public async Task<IEnumerable<ClrModel>> GetPackageClrsWithClrAssertionsAsync(int id)
        {
            var clrs = await _context.Clrs
                .Include(clr => clr.CredentialPackage)
                .Include(clr => clr.ClrAssertions)
                .ThenInclude(ca => ca.Assertion)
                .AsNoTracking()
                .Where(clr => clr.CredentialPackageId == id)
                .OrderByDescending(x => x.IssuedOn)
                .ToListAsync();

            return clrs;
        }
        //Utility use only Don't use this in normal app code
        public async Task<IEnumerable<AssertionModel>> GetAssertionsWithClrAsync()
        {
            var assertions = await _context.Assertions
                .IgnoreQueryFilters()
                .Include(clr => clr.ClrAssertion)
                .ThenInclude(ca => ca.Clr)
                .ToListAsync();

            return assertions;
        }
        //Utility use only Don't use this in normal app code
        public async Task<IEnumerable<AssertionModel>> GetAssertionsAsync()
        {
            var assertions = await _context.Assertions
                .IgnoreQueryFilters()
                .ToListAsync();

            return assertions;
        }
        public IQueryable<CredentialPackageModel> GetDeep(string userId, int id)
        {
            var packages = GetAllDeep(userId)
                .Where(package => package.Id == id);

            return packages;
        }
        public async Task<CredentialPackageModel> GetBackpackPackageAsync(string userId, int id)
        {
            var packages = await _context.CredentialPackages
                .AsNoTracking()
                .Include(cp => cp.BadgrBackpack)
                .ThenInclude(bp => bp.BadgrAssertions)
                .Where(package => package.UserId == userId && package.Id == id)
                .FirstOrDefaultAsync();

            return packages;
        }
        public async Task<List<ClrModel>> GetAllClrsAsync(string userId)
        {
            return await _context.Clrs.AsNoTracking()
                .Include(clr => clr.ParentVerifiableCredential)
                .ThenInclude(vc => vc.ParentCredentialPackage)
                .Include(clr => clr.ParentCredentialPackage)
                .Include(clr => clr.ParentClrSet)
                .ThenInclude(cs => cs.ParentCredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => c.CredentialPackage.UserId == userId).ToListAsync();
        }
        public async Task<List<ClrModel>> GetAllClrsAsync(string userId, int[] ids)
        {
            return await _context.Clrs.AsNoTracking()
                .Include(clr => clr.ParentCredentialPackage)
                .Include(clr => clr.ParentClrSet)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => ids.Contains(c.ClrId) && ((c.CredentialPackage.UserId == userId)))
                .ToListAsync();
        }

        public IQueryable<ClrModel> GetAllClrs(string userId)
        {
            return _context.Clrs.AsNoTracking()
               .Include(clr => clr.CredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => c.CredentialPackage.UserId == userId);
        }

        public async Task<CredentialPackageModel> GetWithSourcesAsync(string userId, int id)
        {
            var credentialPackage = await _context.CredentialPackages
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(clra => clra.Source)
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.ClrSet)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(cp => cp.Clr)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.Id == id);
            return credentialPackage;
        }

        public async Task<CredentialPackageModel> GetAsync(string userId, int id)
        {
            var credentialPackage = await _context.CredentialPackages
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .Include(cp => cp.ClrSet)
                .ThenInclude(sets => sets.Clrs)
                .Include(cp => cp.Clr)
                .AsNoTracking()
                .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.Id == id);
            return credentialPackage;
        }

        public async Task<bool> PackageExistsAsync(string userId, int id)
        {
            return await _context.CredentialPackages.AnyAsync(cp => cp.UserId == userId && cp.Id == id);
        }


        public async Task<CredentialPackageModel> GetAsync(int id)
        {
            var credentialPackage = await _context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(clrSets => clrSets.Clrs)
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.Clrs)
                .Include(cp => cp.Clr)
                .FirstOrDefaultAsync(cp => cp.Id == id);
            return credentialPackage;
        }
        public async Task<CredentialPackageModel> UpdateAsync(CredentialPackageModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<CertificateModel> AddCertificateAsync(CertificateModel input)
        {
            _context.Certificates.Add(input);
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<CertificateModel> UpdateCertificateAsync(CertificateModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<CertificateModel> GetCertificateAsync(string id)
        {
            return await _context.Certificates.FindAsync(id);
        }
        public async Task<ClrModel> GetClrAsync(string userId, int id)
        {
            var clr = await _context.Clrs.AsNoTracking()
                .Include(c => c.CredentialPackage)
                .FirstOrDefaultAsync(c => c.ClrId == id && c.CredentialPackage.UserId == userId);

            return clr;
        }
        
        public async Task DeleteClrAsync(int id)
        {
            var item =  await _context.Clrs
                    .Include(x => x.Authorization)
                    .ThenInclude(x => x.Source)
                    .SingleAsync(x => x.ClrId == id);

            item.Authorization.Delete();
            item.Delete();
            //_context.Clrs.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task<ClrModel> UpdateClrAsync(ClrModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        
        public async Task CreateClrFromSelectedAsync(string userId, string name, int[] ids) 
        {
            var clrs = await GetAllClrsAsync(userId, ids);

            if (!clrs.Any())
            {
                throw new Exception("There weren't any CLRs matching your selections.");
            }

            var newClr = new ClrDType
            {
                Context = ClrConstants.JsonLd.Context,
                Type = ClrConstants.Type.Clr,
                Assertions = new List<AssertionDType>(),
                Id = $"urn:uuid:{Guid.NewGuid():D}",
                IssuedOn = DateTime.Now.ToLocalTime(),
                Name = name,
                SignedAssertions = new List<string>()
            };

            foreach (var clrModel in clrs)
            {
                    var clr = JsonSerializer.Deserialize<ClrDType>(clrModel.Json);

                    if (!string.IsNullOrEmpty(clrModel.SignedClr))
                    {
                        clr = clrModel.SignedClr.DeserializePayload<ClrDType>();
                    }

                    // Assume all the CLRs are for the same person

                    newClr.Learner = clr.Learner;
                    newClr.Publisher = clr.Learner; //this is correct :-) the learner is selfpublishing a collection...

                    foreach (var assertion in clr.Assertions ?? new List<AssertionDType>())
                    {
                        newClr.Assertions.Add(assertion);
                    }

                    foreach (var signedAssertion in clr.SignedAssertions ?? new List<string>())
                    {
                        newClr.SignedAssertions.Add(signedAssertion);
                    }
            }
            //var mainClr = new ClrModel
            //{
            //    AssertionsCount = newClr.Assertions.Count + newClr.SignedAssertions.Count,
            //    Id = newClr.Id,
            //    IssuedOn = newClr.IssuedOn,
            //    Json = JsonSerializer.Serialize(newClr),
            //    LearnerName = newClr.Learner.Name,
            //    Name = newClr.Name,
            //    PublisherName = newClr.Publisher.Name,
            //    RefreshedAt = newClr.IssuedOn,
            //    Publisher = ProfileModel.FromDType(newClr.Publisher),
            //    Learner = ProfileModel.FromDType(newClr.Learner),
            //};

            var credentialPackage = new CredentialPackageModel()
            {
                UserId = userId,
                TypeId = PackageTypeEnum.Collection,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
            var mainClr = _etlService.ConvertClr(newClr, null, null, credentialPackage);
            credentialPackage.Clr = mainClr;
            await _etlService.SaveClrPackageModelAsync(credentialPackage);

        }
        
    }

    public class CredentialResponse: GenericModel
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
    }

}
