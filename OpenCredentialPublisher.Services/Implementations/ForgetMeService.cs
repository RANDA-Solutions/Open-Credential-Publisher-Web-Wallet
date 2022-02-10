using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    public class ForgetMeService
    {
        private readonly WalletDbContext _walletContext;
        private readonly CredentialPackageService _credentialPackageService;
        private readonly CredentialService _credentialService;
        private readonly ILogger<ForgetMeService> _logger;
        public ForgetMeService(WalletDbContext walletContext, CredentialPackageService credentialPackageService, CredentialService credentialService, ILogger<ForgetMeService> logger)
        {
            _credentialPackageService = credentialPackageService;
            _credentialService = credentialService;
            _walletContext = walletContext;
            _logger = logger;
        }


        public async Task ForgetUser(string userId)
        {
            try
            {
                var revocations = _walletContext.Revocations.IgnoreQueryFilters().Where(r => r.UserId == userId);
                _walletContext.RemoveRange(revocations);

                var connections = _walletContext.ConnectionRequests.IgnoreQueryFilters().Where(cr => cr.UserId == userId);
                _walletContext.RemoveRange(connections);

                var loginProofRequests = _walletContext.LoginProofRequests.IgnoreQueryFilters().Where(pr => pr.UserId == userId);
                _walletContext.RemoveRange(loginProofRequests);

                var proofRequests = _walletContext.ProofRequests.IgnoreQueryFilters()
                    .Include(pr => pr.Messages)
                    .Where(pr => pr.UserId == userId);
                _walletContext.RemoveRange(proofRequests);

                var emailVerifications = _walletContext.EmailVerifications.IgnoreQueryFilters()
                    .Include(ev => ev.Message)
                    .Where(ev => ev.UserId == userId);
                _walletContext.RemoveRange(emailVerifications);

                var smartResumes = _walletContext.SmartResumes.IgnoreQueryFilters().Where(sr => sr.UserId == userId);
                _walletContext.RemoveRange(smartResumes);

                var links = _walletContext.Links
                    .Include(l => l.Shares)
                        .ThenInclude(s => s.Messages)
                    .Where(l => l.UserId == userId);

                _walletContext.RemoveRange(links);

                var recipients = _walletContext.Recipients.IgnoreQueryFilters().Where(r => r.UserId == userId);
                _walletContext.RemoveRange(recipients);

                var credentialRequests = _walletContext.CredentialRequests.IgnoreQueryFilters().Where(cr => cr.UserId == userId);
                _walletContext.RemoveRange(credentialRequests);

                var wallets = _walletContext.WalletRelationships.IgnoreQueryFilters().Where(wr => wr.UserId == userId);
                _walletContext.RemoveRange(wallets);

                await _walletContext.SaveChangesAsync();
                _walletContext.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem deleting part of your profile.");
            }

            try
            {

                var credentialPackages = await _credentialService.GetAllShallow(userId).ToListAsync();
                for (var p = 0; p < credentialPackages.Count; p++)
                {
                    var package = credentialPackages[p];
                    _credentialPackageService.GetCredentialPackageModel(ref package, false, true);
                    if (package.TypeId == Data.Models.Enums.PackageTypeEnum.OpenBadgeConnect || package.TypeId == Data.Models.Enums.PackageTypeEnum.OpenBadge)
                    {
                        var backpack = await _credentialService.GetBackpackPackageAsync(userId, package.Id);
                        package.BadgrBackpack = backpack.BadgrBackpack;
                    }
                    else if (package.TypeId == Data.Models.Enums.PackageTypeEnum.Clr)
                    {
                        package.Clr = await _credentialService.GetClrForDeletionAsync(package.Clr.ClrId);

                    }
                    else if (package.TypeId == Data.Models.Enums.PackageTypeEnum.ClrSet)
                    {
                        for (var i = 0; i < package.ClrSet.Clrs.Count; i++)
                        {
                            package.ClrSet.Clrs[i] = await _credentialService.GetClrForDeletionAsync(package.ClrSet.Clrs[i].ClrId);
                        }
                    }
                    else if (package.TypeId == Data.Models.Enums.PackageTypeEnum.VerifiableCredential)
                    {
                        if (package.VerifiableCredential?.Clrs?.Any() == true)
                        {
                            for (var i = 0; i < package.VerifiableCredential.Clrs.Count; i++)
                            {
                                package.VerifiableCredential.Clrs[i] = await _credentialService.GetClrForDeletionAsync(package.VerifiableCredential.Clrs[i].ClrId);
                            }
                        }
                        else if (package.VerifiableCredential?.ClrSets?.Any() == true)
                        {
                            foreach (var clrSet in package.VerifiableCredential.ClrSets)
                            {
                                for (var i = 0; i < clrSet.Clrs.Count; i++)
                                {
                                    clrSet.Clrs[i] = await _credentialService.GetClrForDeletionAsync(clrSet.Clrs[i].ClrId);
                                }
                            }
                        }
                    }
                    _walletContext.Remove(package);
                    await _walletContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem deleting your package.");
            }

        }
    }
}
