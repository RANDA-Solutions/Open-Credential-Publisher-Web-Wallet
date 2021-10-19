using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Utils;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class ClrDetailService
    {
        private readonly WalletDbContext _context;
        private readonly AuthorizationsService _authorizationsService;
        private readonly CredentialService _credentialService;
        private readonly IHttpClientFactory _factory;
        private readonly SchemaService _schemaService;
        private readonly LogHttpClientService _logHttpClientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<AugmentedAssertionDType> AllAssertions = new List<AugmentedAssertionDType>();
        private List<AssociatedAssertion> AssociatedAssertions { get; set; } = new List<AssociatedAssertion>();
        private List<AssociatedAssertion> ParentAssertions { get; set; }
        private bool IsSelfPublished  { get; set; }
        private List<PdfShareModel> Pdfs { get; set; } = new List<PdfShareModel>();

        // ClrService - it is presumed revocation will be reflected by current source, no need to check prior revocationList
        public ClrDetailService(WalletDbContext context, IHttpClientFactory factory, IConfiguration configuration, AuthorizationsService authorizationsService, SchemaService schemaService
            , IHttpContextAccessor httpContextAccessor, CredentialService credentialService, LogHttpClientService logHttpClientService)
        {
            _context = context;
            _authorizationsService = authorizationsService;
            _factory = factory;
            _schemaService = schemaService;
            _logHttpClientService = logHttpClientService;
            _credentialService = credentialService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<VerificationVM> GetClrVerificationAsync(int clrId)
        {
            var clr = await GetClrAsync(clrId);     

            return VerificationVM.FromModel(clr.Verification);
        }

        public async Task<ClrProfileVM> GetClrLearnerVMAsync(int id)
        {
            var clr = await GetClrAsync(id);

            return ClrProfileVM.FromClrProfile(clr.Learner);
        }
        public async Task<ClrProfileVM> GetClrPublisherVMAsync(int id)
        {
            var clr = await GetClrAsync(id); ;

            return ClrProfileVM.FromClrProfile(clr.Publisher);
        }

        public async Task<List<AssertionHeaderVM>> GetClrParentAssertionIdsAsync(int id, bool isShare)
        {
            var tranArtifact = await _context.Artifacts.AsNoTracking()
                .Where(a => a.ClrId == id && a.IsPdf && a.NameContainsTranscript)
                .FirstOrDefaultAsync();

            if (tranArtifact != null)
            {
                return await _context.ClrAssertions.AsNoTracking()
                    .Include(ca => ca.Assertion)
                    .Where(ca => ca.ClrId == id)
                    .OrderByDescending(ca => ca.Assertion.Id == tranArtifact.AssertionId)
                    .ThenByDescending(a => a.Assertion.IssuedOn)
                    .Select(ca => new AssertionHeaderVM { Id = ca.Assertion.Id, DisplayName = ca.Assertion.DisplayName })
                    .ToListAsync();
            }

            return await _context.ClrAssertions.AsNoTracking()
                    .Include(ca => ca.Assertion)
                    .Where(ca => ca.ClrId == id)
                    .Select(ca => new AssertionHeaderVM { Id = ca.Assertion.Id, DisplayName = ca.Assertion.DisplayName })
                    .ToListAsync();
        }
        public async Task<List<AssertionHeaderVM>> GetClrChildAssertionIdsAsync(int clrId, int id, bool isShare)
        {
            return await _context.Assertions.AsNoTracking()
                .Where(a => a.ParentAssertionId == id)
                .Select(a => new AssertionHeaderVM { Id = a.Id, DisplayName = a.DisplayName })
                .ToListAsync();
        }

        public async Task<AssertionWithAchievementVM> GetAssertionWithAchievementVMAsync(int clrId, string assertionId)
        {
            //var id = Uri.UnescapeDataString(assertionId);
            var asrt = await _context.Assertions.AsNoTracking()
                .Include(a => a.ClrAssertion)
                .Include(a => a.Verification)
                .Include(a => a.Achievement)
                .ThenInclude(aa => aa.Issuer)
                .Include(a => a.Achievement)
                .ThenInclude(aa => aa.ResultDescriptions)
                .ThenInclude(rd => rd.ResultDescriptionAlignments)
                .Include(a => a.Achievement)
                .ThenInclude(aa => aa.ResultDescriptions)
                .ThenInclude(rd => rd.RubricCriterionLevels)
                .Where(a => a.ClrAssertion.ClrId == clrId && a.Id == assertionId)
                .FirstOrDefaultAsync();

            return AssertionWithAchievementVM.FromAssertion(asrt);
        }

        public async Task<AchievementVM> GetClrAchievementVMAsync(int clrId, string achievementId)
        {
            var achievement =  await _context.Achievements.AsNoTracking()
                .Include(c => c.ResultDescriptions)
                .ThenInclude(rd => rd.ResultDescriptionAlignments)
                .Include(aa => aa.ResultDescriptions)
                .ThenInclude(rd => rd.RubricCriterionLevels)
                .Include(c => c.Issuer)
                .FirstOrDefaultAsync(x => x.ClrAchievement.ClrId == clrId && x.Id == achievementId);            

            return AchievementVM.FromModel(achievement);
        }
        //End V2 *************************************************************************************************

        public async Task<List<AssertionModel>> GetClrParentAssertionsAsync(int id)
        {
            return await _context.Assertions.AsNoTracking()
                .Include(a => a.ClrAssertion)
                .Where(a => a.ClrAssertion.ClrId == id)
                .ToListAsync();
        }
        private async Task<ClrModel> GetClrAsync(int id)
        {
            return await _context.Clrs.AsNoTracking()
                .Include(c => c.Learner)
                .Include(c => c.Publisher)
                .Include(c => c.Verification)
                .FirstOrDefaultAsync(x => x.ClrId == id);
        }
    }
}
