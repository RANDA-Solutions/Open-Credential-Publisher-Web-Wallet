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

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{ 
    public class DisplayModel : ClrDisplayPageModel
    {
        public bool RequiresAccessKey { get; set; }
        public bool ShowDownloadVCJsonButton { get; set; }
        public bool ShowData { get; set; }
        public string Id { get; set; }

        [BindProperty]    
        public string AccessKey { get; set; }

        public DisplayModel(
            WalletDbContext context,
            IHttpClientFactory factory,
            IConfiguration configuration,
            ClrService clrService)
            :base(context, factory, configuration, clrService)
        {
        }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null) return NotFound();

            Id = id;

            var link = await Context.Links
                .Include(l => l.Clr)
                .ThenInclude(l => l.CredentialPackage)
                .Include(l => l.Shares)
                .SingleOrDefaultAsync(l => l.Id == id);

            if (link?.Clr == null) return RedirectToPage("NotAvailable");

            if (link.RequiresAccessKey)
            {
                if (User?.UserId() == link.UserId)
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

            await Context.SaveChangesAsync();

            Clr = JsonSerializer.Deserialize<ClrViewModel>(link.Clr.Json);
            Clr.ClrId = link.Clr.Id;
            Clr.BuildAssertionsTree();
            Clr.Context = "https://purl.imsglobal.org/spec/clr/v1p0/context/imsclr_v1p0.jsonld";

            ShowDownloadVCJsonButton = ShowData && link.Clr.CredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;

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

            var link = await Context.Links
                .Include(l => l.Clr)
                .ThenInclude(l => l.CredentialPackage)
                .ThenInclude(l => l.VerifiableCredential)
                .SingleOrDefaultAsync(l => l.Id == id);

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

            var link = await Context.Links
                .Include(l => l.Clr)
                .Include(l => l.Shares)
                .SingleOrDefaultAsync(l => l.Id == id);

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
            await Context.SaveChangesAsync();

            Clr = JsonSerializer.Deserialize<ClrViewModel>(link.Clr.Json);
            Clr.ClrId = link.Clr.Id;
            Clr.BuildAssertionsTree();
            Clr.Context = "https://purl.imsglobal.org/spec/clr/v1p0/context/imsclr_v1p0.jsonld";

            return Page();
        }
    }
}