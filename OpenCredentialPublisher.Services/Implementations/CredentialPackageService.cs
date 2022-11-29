using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class CredentialPackageService
    {
        private readonly RevocationDocumentService _revocationDocumentService;
        private readonly CredentialService _credentialService;
        private readonly WalletDbContext _context;
        private readonly ILogger<CredentialPackageService> _logger;
        public CredentialPackageService(RevocationDocumentService revocationDocumentService, CredentialService credentialService, WalletDbContext context, ILogger<CredentialPackageService> logger)
        {
            _revocationDocumentService = revocationDocumentService;
            _credentialService = credentialService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> UpdateCredentialPackageNameAsync(string userId, int credentialPackageId, string name)
        {
            try
            {
                var credentialPackage = await _context.CredentialPackages.FirstOrDefaultAsync(cp => cp.Id == credentialPackageId);
                if (credentialPackage == null)
                {
                    _logger.LogError("A credential package with the supplied id could not be found.", userId, credentialPackageId, name);
                }
                else if (credentialPackage.UserId != userId)
                {
                    _logger.LogError("That credential package belongs to another user.", userId, credentialPackageId, name);
                }
                else
                {
                    credentialPackage.Name = name;
                    credentialPackage.ModifiedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId, credentialPackageId, name);
            }
            return false;
        }

        public async Task<CredentialPackageModel> GetCredentialPackageAsync(int id, string userId)
        {
            return await _context.CredentialPackages.AsNoTracking().FirstOrDefaultAsync(cp => cp.Id == id && cp.UserId == userId);
        }

        public CredentialPackageViewModel GetCredentialPackageViewModel(CredentialPackageModel model)
        {
            GetCredentialPackageModel(ref model);
            return CredentialPackageViewModel.FromCredentialPackageModel(model);
        }

        public void GetCredentialPackageModel(ref CredentialPackageModel model, bool includeSource = true, bool includeDeleted = false)
        {
            var packageId = model.Id;
            var query = _context.CredentialPackages.AsNoTracking();
            if (includeDeleted)
                query = query.IgnoreQueryFilters();

            if (model.TypeId == PackageTypeEnum.Clr)
            {
                model = query
                    .Include(cp => cp.Clr).FirstOrDefault(cp => cp.Id == packageId);
            }
            else if (model.TypeId == PackageTypeEnum.ClrSet)
            {
                model = query
                    .Include(cp => cp.ClrSet)
                    .ThenInclude(clrs => clrs.Clrs)
                    .FirstOrDefault(cp => cp.Id == packageId);
            }
            else if (model.TypeId == PackageTypeEnum.VerifiableCredential)
            {
                model = query
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(clrSets => clrSets.Clrs)
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.Clrs)
                .FirstOrDefault(cp => cp.Id == packageId);
            }
            else if (model.TypeId == PackageTypeEnum.OpenBadge || model.TypeId == PackageTypeEnum.OpenBadgeConnect)
            {
                var badgeQuery = query
                    .Include(cp => cp.BadgrBackpack)
                    .ThenInclude(bdgr => bdgr.BadgrAssertions)
                    .Include(cp => cp.Authorization);

                if (includeSource)
                {
                    model = badgeQuery.ThenInclude(au => au.Source).FirstOrDefault(cp => cp.Id == packageId);
                }
                else
                {
                    model = badgeQuery.FirstOrDefault(cp => cp.Id == packageId);
                }
            }
        }

        public async Task<(bool revoked, string revocationReason)> CheckRevocationAsync(CredentialPackageModel package)
        {
            var verifiableCredential = JsonSerializer.Deserialize<VerifiableCredential>(package.VerifiableCredential.Json);
            if (verifiableCredential.CredentialStatus != null)
            {
                var document = await _revocationDocumentService.GetRevocationDocumentAsync(verifiableCredential.CredentialStatus.Id);
                if (document?.Revocations != null && document.Revocations.Any())
                {
                    var revocation = document.Revocations.FirstOrDefault(r => r.Id == verifiableCredential.Id);
                    if (revocation != null)
                    {
                        package.Revoked = true;
                        package.RevocationReason = document.Statuses[revocation.Status];
                        await _credentialService.UpdateAsync(package);
                        return (true, package.RevocationReason);
                    }
                }
            }
            return (false, null);
        }
    }
}
