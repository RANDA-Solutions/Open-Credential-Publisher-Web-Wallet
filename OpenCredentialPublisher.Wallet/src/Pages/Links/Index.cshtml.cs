using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using System.Text.Json;
using OpenCredentialPublisher.ClrWallet.Utilities;
using OpenCredentialPublisher.Credentials.Clrs.Utilities;
using OpenCredentialPublisher.Services.Drawing;
using Microsoft.AspNetCore.Mvc;
using OpenCredentialPublisher.Wallet.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class IndexModel : PageModel
    {
        private readonly WalletDbContext _context;

        public IndexModel(WalletDbContext context)
        {
            _context = context;
        }

        public List<LinkModel> Links { get; set; }

        public async Task OnGet()
        {
            Links = await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Include(l => l.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .Where(l => l.UserId == User.UserId())
                .ToListAsync();

            foreach (var link in Links)
            {
                link.ClrViewModel = JsonSerializer.Deserialize<ClrViewModel>(link.Clr.Json);
                link.ClrViewModel.ClrId = link.Clr.Id;
                link.ClrViewModel.BuildAssertionsTree();
            }
        }

        public async Task<IActionResult> OnPostPdf(string id, int clrId, string assertionId, string evidenceName, int artifactId, string artifactName)
        {
            var link = await _context.Links
                .Include(l => l.Clr)
                .FirstOrDefaultAsync(l => l.UserId == User.UserId() && l.Id == id);
            if (link == null)
                return NotFound();

            var model = JsonSerializer.Deserialize<ClrViewModel>(link.Clr.Json);
            model.BuildAssertionsTree();

            var assertion = model.AllAssertions.FirstOrDefault(a => a.Id == assertionId);
            var evidence = assertion.Evidence.FirstOrDefault(e => e.Name == evidenceName);
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

            await _context.Shares.AddAsync(shareModel);
            await _context.SaveChangesAsync();

            var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(artifact.Url);
            bytes = PdfUtility.AppendQRCodePage(bytes, this.GetLinkUrl(link.Id), shareModel.AccessKey, PdfUtility.SourceApplicationName);
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