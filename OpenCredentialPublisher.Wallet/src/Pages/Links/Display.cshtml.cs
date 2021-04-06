using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Services.Implementations;
using System.ComponentModel.DataAnnotations;
using OpenCredentialPublisher.Services.Extensions;
using System.Linq;
using System.Text;
using OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{ 
    public class DisplayModel : ClrDisplayPageModel
    {
        private readonly LinkService _linkService;

        public bool RequiresAccessKey { get; set; }
        public bool ShowDownloadVCJsonButton { get; set; }
        public bool ShowData { get; set; }
        public string Id { get; set; }

        [BindProperty]    
        public string AccessKey { get; set; }

        public DisplayModel(
            CredentialService credentialService,
            IHttpClientFactory factory,
            IConfiguration configuration,
            RevocationService revocationService,
            ClrService clrService,
            BadgrService badgrService,
            LinkService linkService
            )
            :base(credentialService, factory, configuration, revocationService, clrService, badgrService)
        {
            _linkService = linkService;
        }

        public async Task<IActionResult> OnGet(string id, string key = null)
        {
            if (id == null) return NotFound();

            Id = id;

            var link = await _linkService.GetDeepAsync(id);

            if (link?.Clr == null) return RedirectToPage("NotAvailable");

            if (link.RequiresAccessKey)
            {
                if (User?.UserId() == link.UserId)
                {
                    ShowData = true;
                }
                else if (!string.IsNullOrEmpty(key) && link.Shares.Any(s => s.AccessKey == key && s.StatusId == StatusEnum.Active))
                {
                    ShowData = true;
                }
                else
                {
                    RequiresAccessKey = true;
                }
            }
            else
            {
                ShowData = true;
            }

            if (User?.UserId() != link.UserId)
            {
                link.DisplayCount += 1;
            }

            await _linkService.UpdateAsync(link);

            Clr = ClrViewModel.FromClrModel(link.Clr);

            ShowDownloadVCJsonButton = ShowData && Clr.AncestorCredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;

            return Page();
        }

        //public async Task<IActionResult> OnPostDownloadPdf(string id)
        //{
        //    if (id == null) return NotFound();

        //    Id = id;

        //    var link = await Context.Links
        //        .Include(l => l.Clr)
        //        .Include(l => l.Shares)
        //        .SingleOrDefaultAsync(l => l.Id == id);

        //    if (link?.Clr == null) return NotFound();
        //}
        public async Task<IActionResult> OnPostDownloadVCJson(string id)
        {
            if (id == null) return NotFound();

            var link = await _linkService.GetDeepAsync(id);

            if (link.Clr?.CredentialPackage?.VerifiableCredential != null)
            {
                var package = link.Clr?.CredentialPackage;
                return new FileContentResult(UTF8Encoding.UTF8.GetBytes(package.VerifiableCredential.Json), "application/json") { FileDownloadName = $"VerifiableCredential-{package.VerifiableCredential.Identifier}.json" };
            }
            return NotFound();
        }
        public async Task<IActionResult> OnPostAccess(string id)
        {
            if (id == null) return NotFound();

            Id = id;

            var link = await _linkService.GetAsync(id);

            if (link?.Clr == null) return NotFound();

            RequiresAccessKey = link.RequiresAccessKey;

            AccessKey = AccessKey?.Trim();

            if (link.Shares.Any(s => s.AccessKey == AccessKey && s.StatusId == StatusEnum.Active))
            {
                ShowData = true;
            }
            else
            {
                ModelState.AddModelError("", "The access key is not valid");
                return Page();
            }

            link.DisplayCount += 1;
            await _linkService.UpdateAsync(link);

            Clr = ClrViewModel.FromClrModel(link.Clr);

            return Page();
        }
    }
}