using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Data.Utils;
using OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified;
using OpenCredentialPublisher.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class ClrsService
    {
        private readonly WalletDbContext _context;

        public ClrsService(WalletDbContext context)
        {
            _context = context;
        }

        public async Task<List<EndorsementVM>> GetAchievementEndorsementVMListAsync(int clrId, string id)
        {

            var endorsements = await _context.Endorsements.AsNoTracking()
                .Include(e => e.EndorsementClaim)
                .Include(e =>e.Issuer)
                .Include(e => e.Verification)
                .Where(e => e.AchievementEndorsement.Achievement.Id == id && e.AchievementEndorsement.Achievement.ClrAchievement.ClrId == clrId)
                .Select(e => EndorsementVM.FromModel(e))
                .ToListAsync();

            return endorsements;
        }
        public async Task<List<AlignmentVM>> GetAssertionAchievementAlignmentVMListAsync(int clrId, int assertionId, string id)
        {
            var alignments = await _context.Alignments.AsNoTracking()
                .Where(e => e.AchievementAlignment.Achievement.Id == id
                    && e.AchievementAlignment.Achievement.Assertion.AssertionId == assertionId
                    && e.AchievementAlignment.Achievement.Assertion.ClrAssertion.ClrId == clrId)
                .Select(e => AlignmentVM.FromModel(e))
                .ToListAsync();

            return alignments;
        }
        public async Task<List<AlignmentVM>> GetAchievementAlignmentVMListAsync(int clrId, string id)
        {
            var alignments = await _context.Alignments.AsNoTracking()
                .Where(e => e.AchievementAlignment.Achievement.Id == id && e.AchievementAlignment.Achievement.ClrAchievement.ClrId == clrId)
                .Select(e => AlignmentVM.FromModel(e))
                .ToListAsync();

            return alignments;
        }
        public async Task<List<AssociationVM>> GetAchievementAssociationVMListAsync(int clrId, string id)
        {
            var associations = await _context.Associations.AsNoTracking()
                //.Include(e => e.AchievementAssociation.Achievement.ClrAchievement.Clr.ClrAssertions).ThenInclude(ass => ass.Assertion.Achievement)
                .Where(e => e.AchievementAssociation.Achievement.Id == id && e.AchievementAssociation.Achievement.ClrAchievement.ClrId == clrId)
                .Select(e => AssociationVM.FromModel(e))
                .ToListAsync();

            return associations;
        }
        public async Task<List<EndorsementVM>> GetAssertionEndorsementVMListAsync(int clrId, string id)
        {

            var endorsements = await _context.Endorsements.AsNoTracking()
                .Include(e => e.EndorsementClaim)
                .Include(e => e.Issuer)
                .Include(e => e.Verification)
                .Where(e => e.AssertionEndorsement.Assertion.Id == id && e.AssertionEndorsement.Assertion.ClrAssertion.ClrId == clrId)
                .Select(e => EndorsementVM.FromModel(e))
                .ToListAsync();

            return endorsements;
        }

        public async Task<List<EvidenceVM>> GetEvidenceVMListAsync(int clrId, string id)
        {

            var evidence = await _context.Evidences.AsNoTracking()
                .Include(e => e.EvidenceArtifacts)
                .ThenInclude(ea => ea.Artifact)
                .Where(e => e.AssertionEvidence.Assertion.Id == id && e.AssertionEvidence.Assertion.ClrAssertion.ClrId == clrId)
                .Select(e=> EvidenceVM.FromModel(e))
                .ToListAsync();

            return evidence;
        }

        public async Task<AssertionResultsVM> GetResultVMListAsync(int clrId, string id)
        {

            var results = await _context.Results.AsNoTracking()
                .Where(r => r.Assertion.Id == id && r.Assertion.ClrAssertion.ClrId == clrId)
                .Select(r => ResultVM.FromModel(r))
                .ToListAsync();

            var resultDescriptions = await _context.ResultDescriptions.AsNoTracking()
                .Include(rd => rd.ResultDescriptionAlignments)
                .ThenInclude(rda => rda.Alignment)
                .Include(rd => rd.RubricCriterionLevels)
                .Where(r => r.Achievement.Assertion.Id == id && r.Achievement.Assertion.ClrAssertion.ClrId == clrId)
                .Select(r => ResultDescriptionVM.FromModel(r))
                .ToListAsync();

            return new AssertionResultsVM(results, resultDescriptions);
        }
    }
}
