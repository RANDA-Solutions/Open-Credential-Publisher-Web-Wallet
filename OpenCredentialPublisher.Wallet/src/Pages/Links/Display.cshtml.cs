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
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Wallet.Extensions;
using OpenCredentialPublisher.Services.Drawing;
using System;
using OpenCredentialPublisher.Data.Options;
using Microsoft.Extensions.Options;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{ 
    public class DisplayModel : ClrDisplayPageModel
    {
        private readonly LinkService _linkService;

        public bool RequiresAccessKey { get; set; }
        public bool ShowDownloadVCJsonButton { get; set; }
        public bool ShowDownloadPdfButton { get; set; }
        public PdfShareViewModel TranscriptPdf { get; set; }
        public bool ShowData { get; set; }
        public string Id { get; set; }
        public SiteSettingsOptions SiteSettings { get; set; }

        [BindProperty]    
        public string AccessKey { get; set; }

        public DisplayModel(
            CredentialService credentialService,
            CredentialPackageService credentialPackageService,
            ConnectService connectService,
            IHttpClientFactory factory,
            IConfiguration configuration,
            RevocationService revocationService,
            ClrService clrService,
            BadgrService badgrService,
            LinkService linkService,
            IOptions<SiteSettingsOptions> siteSettings
            )
            :base(credentialService, connectService, factory, configuration, revocationService, clrService, badgrService, credentialPackageService)
        {
            _linkService = linkService;
            SiteSettings = siteSettings?.Value;
        }

        public async Task<IActionResult> OnGet(string id, string key = null)
        {
            if (id == null) return NotFound();

            Id = id;

            var link = await _linkService.GetAsync(id);

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
                    AccessKey = key;
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

            if (ShowData)
            {
                if (User?.UserId() != link.UserId)
                {
                    link.DisplayCount += 1;
                }

                await _linkService.UpdateAsync(link);

                link = await _linkService.GetDeepAsync(link.Id);

                Clr = ClrViewModel.FromClrModel(link.Clr);
                if (Clr.Pdfs.HasTranscriptPdf())
                {
                    ShowDownloadPdfButton = true;
                    TranscriptPdf = Clr.Pdfs.GetTranscriptPdf();
                }

                ShowDownloadVCJsonButton = ShowData && Clr.AncestorCredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostPdf(string id, int clrId, string assertionId, string evidenceName, int artifactId, string artifactName)
        {
            var link = await _linkService.GetAsync(id);
            ShareModel share;
            if (link.UserId == User.UserId())
            {
                share = link.Shares.FirstOrDefault(s => s.StatusId == StatusEnum.Active);
                if (share == null)
                {
                    share = new ShareModel
                    {
                        LinkId = link.Id,
                        ShareTypeId = ShareTypeEnum.Pdf,
                        AccessKey = Crypto.CreateRandomString(16),
                        UseCount = 0,
                        CreatedOn = DateTimeOffset.UtcNow,
                        StatusId = StatusEnum.Active
                    };

                    link.RequiresAccessKey = true;
                    link.ModifiedAt = DateTimeOffset.UtcNow;
                    await _linkService.AddShareAsync(share);
                    await _linkService.UpdateAsync(link);
                }
            }
            else
            {
                share = link.Shares.FirstOrDefault(s => s.AccessKey == AccessKey && s.StatusId == StatusEnum.Active);
            }

            if (share != null)
            {
                var model = (LinkViewModel.FromLinkModel(link));

                var assertionVM = model.ClrVM.AllAssertions.FirstOrDefault(a => a.Assertion.Id == assertionId);
                var evidence = assertionVM.Assertion.Evidence.FirstOrDefault(e => e.Name == evidenceName);
                var artifact = evidence.Artifacts.FirstOrDefault(a => a.ArtifactKey == artifactId);
                var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(artifact.Url);
                bytes = PdfUtility.AppendQRCodePage(bytes, this.GetLinkUrl(link.Id), share.AccessKey);
                return new FileContentResult(bytes, mimeType) { FileDownloadName = $"{artifactName}.pdf" };
            }
            return NotFound();
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

            if (link.UserId == User.UserId() || link.Shares.Any(s => s.AccessKey == AccessKey && s.StatusId == StatusEnum.Active))
            {
                if (link.Clr?.CredentialPackage?.VerifiableCredential != null)
                {
                    var package = link.Clr?.CredentialPackage;
                    return new FileContentResult(UTF8Encoding.UTF8.GetBytes(package.VerifiableCredential.Json), "application/json") { FileDownloadName = $"VerifiableCredential-{package.VerifiableCredential.Identifier}.json" };
                }
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
            link = await _linkService.GetDeepAsync(link.Id);

            Clr = ClrViewModel.FromClrModel(link.Clr);
            ShowDownloadVCJsonButton = ShowData && Clr.AncestorCredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;


            return Page();
        }
    }
}