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
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Wallet.Extensions;
using System.Linq;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class DisplayModel : CredentialPackageDisplayPageModel
    {
        public bool ShowDownloadVCJsonButton { get; set; }
        public bool ShowDownloadPdfButton { get; set; }
        public PdfShareViewModel TranscriptPdf { get; set; }

        public LinkService _linkService;

        public DisplayModel(
            CredentialService credentialService,
            CredentialPackageService credentialPackageService,
            IHttpClientFactory factory,
            IConfiguration configuration,
            ConnectService connectService,
            RevocationService revocationService,
            BadgrService badgrService,
            ClrService clrService,
            LinkService linkService)
        : base(factory, clrService, connectService, credentialService, credentialPackageService, badgrService, revocationService)
        {
            _linkService = linkService;
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

        public async Task<IActionResult> OnPostPdf(int clrId, string assertionId, string evidenceName, int artifactId, string artifactName)
        {
            var link = await _linkService.GetAsync(User.UserId(), clrId);
            if (link == null)
            {
                var clr = await _credentialService.GetClrAsync(User.UserId(), clrId);
                if (clr == null)
                {
                    return NotFound();
                }
                link = new LinkModel { ClrForeignKey = clr.Id, UserId = User.UserId(), Nickname = $"{clr.Name} - {clr.PublisherName}", CreatedAt = DateTimeOffset.UtcNow };

                await _linkService.AddAsync(link);
            }

            var model = (LinkViewModel.FromLinkModel(link));

            var assertionVM = model.ClrVM.AllAssertions.FirstOrDefault(a => a.Assertion.Id == assertionId);
            var evidence = assertionVM.Assertion.Evidence.FirstOrDefault(e => e.Name == evidenceName);
            var artifact = evidence.Artifacts.FirstOrDefault(a => a.ArtifactKey == artifactId);

            var shareModel = new ShareModel
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
            await _linkService.AddShareAsync(shareModel);
            await _linkService.UpdateAsync(link);

            var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(artifact.Url);
            bytes = PdfUtility.AppendQRCodePage(bytes, this.GetLinkUrl(link.Id), shareModel.AccessKey);
            return new FileContentResult(bytes, mimeType) { FileDownloadName = $"{artifactName}.pdf" };
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
            ShowDownloadVCJsonButton = credentialPackage.TypeId == PackageTypeEnum.VerifiableCredential;
            if (CredentialPackageViewModel.Pdfs.HasTranscriptPdf())
            {
                ShowDownloadPdfButton = true;
                TranscriptPdf = CredentialPackageViewModel.Pdfs.GetTranscriptPdf();
            }
            if (credentialPackage.TypeId == PackageTypeEnum.Clr)
            {
                await _revocationService.MarkClrViewModelRevocationsAsync(User.UserId(), CredentialPackageViewModel.ClrVM);
            }
            return Page();

        }
    }
}