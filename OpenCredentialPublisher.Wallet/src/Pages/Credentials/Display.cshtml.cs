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
using OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class DisplayModel : CredentialPackageDisplayPageModel
    {
        public bool ShowDownloadVCJsonButton { get; set; }

        public DisplayModel(
            CredentialService credentialService,
            CredentialPackageService credentialPackageService,
            IHttpClientFactory factory,
            IConfiguration configuration,
            ConnectService connectService,
            RevocationService revocationService,
            BadgrService badgrService,
            ClrService clrService)
        : base(factory, configuration, clrService, connectService, credentialService, credentialPackageService, badgrService, revocationService)
        {
        }

        public async Task<IActionResult> OnPostDownloadVCJson(int? id)
        {
            if (id == null) return NotFound();

            var package = await _credentialService.GetWithSourcesAsync(User.UserId(), id.Value);

            if (package?.VerifiableCredential != null)
            {
                return new FileContentResult(UTF8Encoding.UTF8.GetBytes(package.VerifiableCredential.Json), "application/json") { FileDownloadName = $"VerifiableCredential-{package.VerifiableCredential.Identifier}.json" };
            }
            return NotFound();
        }

        public async Task<IActionResult> OnGet(int? id, string action = null)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing Credential id.");
                return Page();
            }

            if (action == "b2c")
            {
                // id = await BadgrService.CreateClrFromBadgeAsync(id.Value, User.UserId());
                id = await _badgrService.ConvertClrFromBadgeAsync(id.Value, User.UserId());
            }

            var credentialPackage = await _credentialPackageService.GetCredentialPackageAsync(id.Value, User.UserId());
            if (credentialPackage == null)
            {
                ModelState.AddModelError(string.Empty, "Missing Credential.");
                return Page();
            }
            CredentialPackageViewModel = await _credentialPackageService.GetCredentialPackageViewModelAsync(credentialPackage);

            if (credentialPackage.TypeId == PackageTypeEnum.Clr)
            {
                await _revocationService.MarkClrViewModelRevocationsAsync(User.UserId(), CredentialPackageViewModel.ClrVM);
            }
            return Page();

        }
    }
}