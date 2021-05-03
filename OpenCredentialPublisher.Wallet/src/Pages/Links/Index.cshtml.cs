using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.ClrWallet.Utilities;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using OpenCredentialPublisher.Wallet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class IndexModel : PageModel
    {
        private readonly LinkService _linkService;
        public IndexModel(LinkService linkService)
        {
            _linkService = linkService;
        }

        public List<LinkViewModel> LinkVMs { get; set; }

        public async Task OnGet()
        {
            LinkVMs = new List<LinkViewModel>();
            var links = _linkService.GetAllDeep(User.UserId()).OrderByDescending(l => l.CreatedAt);

            foreach (var link in links)
            {
               LinkVMs.Add(LinkViewModel.FromLinkModel(link));
            }
        }

        public async Task<IActionResult> OnPostPdf(string id, int clrId, string assertionId, string evidenceName, int artifactId, string artifactName)
        {
            var link = await _linkService.GetAsync(User.UserId(), id);
            if (link == null)
                return NotFound();

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

        //public string GetLinkUrl(string id)
        //{
        //    if (Uri.TryCreate($"{Request.Scheme}://{Request.Host}{Request.PathBase}/Links/Display/{id}", UriKind.Absolute, out var url))
        //    {
        //        return url.AbsoluteUri;
        //    }

        //    return string.Empty;
        //}
    }
}