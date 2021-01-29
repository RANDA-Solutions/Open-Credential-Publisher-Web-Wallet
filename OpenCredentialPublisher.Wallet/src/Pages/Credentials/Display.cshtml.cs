using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class DisplayModel : CredentialPackageDisplayPageModel
    {
        public bool ShowDownloadVCJsonButton { get; set; }


        public DisplayModel(
            WalletDbContext context,
            IHttpClientFactory factory,
            IConfiguration configuration,
            ClrService clrService,
            ConnectService connectService)
        : base(context, factory, configuration, clrService, connectService)
        {
        }

        public async Task<IActionResult> OnPostDownloadVCJson(int? id)
        {
            if (id == null) return NotFound();

            var package = await Context.CredentialPackages
                .Include(l => l.VerifiableCredential)
                .FirstOrDefaultAsync(l => l.UserId == User.UserId() && l.Id == id);

            if (package?.VerifiableCredential != null)
            {
                return new FileContentResult(UTF8Encoding.UTF8.GetBytes(package.VerifiableCredential.Json), "application/json") { FileDownloadName = $"VerifiableCredential-{package.VerifiableCredential.Identifier}.json" };
            }
            return NotFound();
        }

        public async Task OnGet(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing Credential id.");
                return;
            }

            var package = await Context.CredentialPackages
                .FirstOrDefaultAsync(clrModel => clrModel.Id == id);

            if (package == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find credential {id}.");
                return;
            }

            if (package.TypeId == PackageTypeEnum.Clr)
            {
                var credentialPackage = await Context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.Clr).FirstOrDefaultAsync(cp => cp.Id == id);

                CredentialPackage = credentialPackage;
                CredentialPackage.BuildView();
            }
            else if (package.TypeId == PackageTypeEnum.ClrSet)
            {
                throw new NotImplementedException("Not fully implemented.");
            }
            else if (package.TypeId == PackageTypeEnum.VerifiableCredential)
            {
                var credentialPackage = await Context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(clrSets => clrSets.Clrs)
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(vc => vc.Clrs)
                .FirstOrDefaultAsync(cp => cp.Id == id);

                CredentialPackage = credentialPackage;
                CredentialPackage.BuildView();

                ShowDownloadVCJsonButton = true;
            }
            else
            {
                throw new NotImplementedException("Not fully implemented.");
            }
            
        }
    }
}