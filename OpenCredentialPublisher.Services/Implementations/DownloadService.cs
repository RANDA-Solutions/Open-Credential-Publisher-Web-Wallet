using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class DownloadService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;
        private readonly LinkService _linkService;
        private readonly CredentialService _credService;

        public DownloadService(WalletDbContext context, SchemaService schemaService, LinkService linkService, CredentialService credService)
        {
            _context = context;
            _schemaService = schemaService;
            _linkService = linkService;
            _credService = credService;
        }

        public async Task<IActionResult> GetPdfAsync(HttpRequest request, PdfRequest dReq, string userId)
        {
            if (!string.IsNullOrEmpty(dReq.LinkId))
            {

            }
            var shareModel = new ShareModel();

            var artifact = await _context.Artifacts.AsNoTracking()
                .Where(a => a.ArtifactId == dReq.ArtifactId.Value)
                .FirstOrDefaultAsync();

            return await GetFileContentResultAsync(request, artifact, dReq, userId, shareModel);
        }
        public async Task<IActionResult> GetClrPdfAsync(HttpRequest request, PdfRequest dReq, string userId)
        {
            var shareModel = new ShareModel();

            var clr = await _credService.GetClrAsync(dReq.ClrId.Value);
            if (clr == null)
                return new NotFoundResult();

            var artifact = await _context.Artifacts.AsNoTracking()
                .Where(a => a.ArtifactId == dReq.ArtifactId.Value && a.EvidenceName == dReq.EvidenceName && a.AssertionId == dReq.AssertionId)
                .FirstOrDefaultAsync();

            if (dReq.CreateLink)
            {
                var link = new LinkModel { ClrForeignKey = clr.ClrId, UserId = userId, Nickname = $"{clr.Name} - {clr.PublisherName}", CreatedAt = DateTimeOffset.UtcNow };

                await _linkService.AddAsync(link);
                shareModel = new ShareModel
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
            }

            return await GetFileContentResultAsync(request, artifact, dReq, userId, shareModel);
        }
        public async Task<IActionResult> GetLinkPdfAsync(HttpRequest request, PdfRequest dReq, string userId)
        {

            var link = await _linkService.GetAsync(userId, dReq.LinkId);

            var linkVM = await _linkService.GetLinkVMAsync(userId, dReq.LinkId, request);

            if (linkVM == null)
                return new NotFoundResult();

            var shareModel = new ShareModel
            {
                LinkId = link.Id,
                ShareTypeId = ShareTypeEnum.Pdf,
                AccessKey = Crypto.CreateRandomString(16),
                UseCount = 0,
                CreatedOn = DateTimeOffset.UtcNow,
                StatusId = StatusEnum.Active
            };

            var artifact = await _context.Artifacts.AsNoTracking()
                .Where(a => a.ArtifactId == dReq.ArtifactId.Value && a.EvidenceName == dReq.EvidenceName && a.AssertionId == dReq.AssertionId)
                .FirstOrDefaultAsync();

            link.ModifiedAt = DateTimeOffset.UtcNow;
            await _linkService.AddShareAsync(shareModel);
            await _linkService.UpdateAsync(link);

            return await GetFileContentResultAsync(request, artifact, dReq, userId, shareModel);
        }
        private string GetLinkUrl(HttpRequest request, string id)
        {
            //var Request = model.Request;
            if (Uri.TryCreate($"{request.Scheme}://{request.Host}{request.PathBase}/Public/Links/Display/{id}", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }
        private async Task<IActionResult> GetFileContentResultAsync(HttpRequest request, ArtifactModel artifact, PdfRequest dReq, string userId, ShareModel shareModel)
        {
            if (artifact.Url.StartsWith("data:"))
            {
                var (mimeType, bytes) = DataUrlUtility.ParseDataUrl(artifact.Url);
                if (dReq.LinkId != null)
                {
                    bytes = PdfUtility.AppendQRCodePage(bytes, this.GetLinkUrl(request, dReq.LinkId), shareModel.AccessKey);
                }
                return new FileContentResult(bytes, mimeType) { FileDownloadName = $"{dReq.ArtifactName}.pdf" };
            }
            else
            {
                using (var client = new HttpClient())
                {
                    using (var result = await client.GetAsync(artifact.Url))
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            var ubytes = await result.Content.ReadAsByteArrayAsync();
                            var umimeType = "application/pdf";
                            return new FileContentResult(ubytes, umimeType) { FileDownloadName = $"{dReq.ArtifactName}.pdf" };
                        }
                        else
                        {
                            throw new ApplicationException("Error retrieveing Pdf.");
                        }
                    }
                }
            }
        }
    }
}
