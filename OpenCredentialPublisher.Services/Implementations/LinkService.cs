using OpenCredentialPublisher.ClrLibrary.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenCredentialPublisher.Data.Abstracts;
using System.Linq;
using OpenCredentialPublisher.Data.ViewModels.nG;
using Microsoft.AspNetCore.Http;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Services.Implementations
{
    public class LinkService
    {
        private readonly WalletDbContext _context;
        private readonly SchemaService _schemaService;

        public LinkService(WalletDbContext context, SchemaService schemaService)
        {
            _context = context;
            _schemaService = schemaService;
        }

        public async Task<LinkListVM> GetLinkVMListAsync(string userId, HttpRequest request)
        {
            var packages = await _context.CredentialPackages.AsNoTracking()
                .Include(cp => cp.ContainedClrs)
                    .ThenInclude(cl => cl.Links)
                .Include(cp => cp.ContainedClrs)
                    .ThenInclude(cl => cl.Artifacts)
                .Where(cp => cp.UserId == userId
                    && cp.IsDeleted == false
                    && cp.ContainedClrs.Any(cl => cl.Links.Any(l => !l.IsDeleted)))
                .ToListAsync();

            var credentials = new List<CredentialLinkVM>();
            foreach (var package in packages)
            {
                foreach (var clr in package.ContainedClrs)
                {
                    var credential = new CredentialLinkVM
                    {
                        CredentialPackageId = package.Id,
                        PackageCreatedAt = package.CreatedAt,
                        ClrId = clr.ClrId,
                        ClrName = clr.Name,
                        ClrIssuedOn = clr.IssuedOn,
                        ClrPublisherName = clr.PublisherName
                    };
                    var artifacts = clr.Artifacts
                        .Where(ar => ar.IsPdf);

                    credential.Pdfs =
                            artifacts
                                .OrderByDescending(a => a.CreatedAt)
                                .Select(a => PdfShareViewModel.FromArtifact(a))
                                .ToList();

                    credential.Links =
                        clr.Links.Select(l =>
                            new ShortLinkVM
                            {
                                Id = l.Id,
                                DisplayCount = l.DisplayCount,
                                Nickname = l.Nickname,
                                Url = GetLinkUrl(request, l.Id)
                            }
                        ).ToList();

                    credentials.Add(credential);
                }
            }
            return new LinkListVM { Credentials = credentials };
        }
        public async Task<LinkVM> GetLinkVMAsync(string userId, string id, HttpRequest request)
        {
            var link = await _context.Links.AsNoTracking()
                .Include(l => l.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(cl => cl.Artifacts)
                .FirstOrDefaultAsync(l => (l.UserId == userId || userId == null) && l.Id == id);

            var pdfs = 
                 link.Clr.Artifacts
                    .Where(a => a.IsPdf)
                    .OrderByDescending(a => a.CreatedAt)
                    .Select(a => PdfShareViewModel.FromArtifact(a))
                    .ToList();

            var lvm = new LinkVM
            {
                Id = link.Id,
                ClrId = link.Clr.ClrId,
                ClrIssuedOn = link.Clr.IssuedOn,
                ClrPublisherName = link.Clr.PublisherName,
                Pdfs = pdfs,
                DisplayCount = link.DisplayCount,
                Nickname = link.Nickname,
                PackageCreatedAt = link.Clr.CredentialPackage.CreatedAt,
                Url = GetLinkUrl(request, link.Id)
            };

            return lvm;
        }

        public async Task<List<ClrLinkVM>> GetAllClrsLinkVMsAsync(string userId)
        {
            var clrLinkVMs = new List<ClrLinkVM>();
            var clrs = await _context.Clrs.AsNoTracking()
                .Include(clr => clr.CredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => c.CredentialPackage.UserId == userId)
                .ToListAsync();


            foreach (var clr in clrs)
            {
                clrLinkVMs.Add(new ClrLinkVM
                {
                    ClrId = clr.ClrId,
                    AddedOn = clr.CredentialPackage.CreatedAt.ToLocalTime(),
                    CreatedAt = clr.IssuedOn,
                    Name = clr.Name,
                    Nickname = clr.Name,
                    SourceId = clr.Authorization?.Source?.Id,
                    SourceName = clr.Authorization?.Source?.Name,
                    PublisherName = clr.PublisherName
                });
            }
            return clrLinkVMs;
        }
        public static string GetLinkUrl(HttpRequest request, string id)
        {
            //var Request = model.Request;
            if (Uri.TryCreate($"{request.Scheme}://{request.Host}{request.PathBase}/s/{id}", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }

        public static string GetLinkUrl(Uri baseUri, string id)
        {
            //var Request = model.Request;
            if (Uri.TryCreate($"{baseUri.Scheme}://{baseUri.Authority}/s/{id}", UriKind.Absolute, out var url))
            {
                return url.AbsoluteUri;
            }

            return string.Empty;
        }

        public async Task<LinkModel> GetAsync(string userId, string id)
        {
            return await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => (x.UserId == userId || userId == null) && x.Id == id);
        }

        public async Task<LinkModel> GetAsync(string id)
        {
            return await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<PdfShareViewModel>> GetClrPdfsAsync(int id)
        {
            var arts =  await _context.Artifacts
                .Include(a => a.EvidenceArtifact)
                .ThenInclude(ea => ea.Evidence)
                .ThenInclude(e => e.AssertionEvidence)
                .ThenInclude(ae => ae.Assertion)
                .Where(a => a.EvidenceArtifact.Evidence.AssertionEvidence.Assertion.ClrAssertion.ClrId == id
                    && a.IsPdf)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return arts
                .Select(a => PdfShareViewModel.FromArtifact(a)).ToList();
        }
        public async Task<ClrModel> GetSingleClrAsync(int id)
        {
            var clr = await _context.Clrs.AsNoTracking()
                .Include(c => c.CredentialPackage)
                .Include(c => c.Verification)
                .Include(c => c.Learner)
                .Include(c => c.Publisher)
                .Include(c => c.ClrAchievements)
                .ThenInclude(c => c.Achievement)
                .Where(clr => clr.ClrId == id)
                .FirstOrDefaultAsync();

            return clr;
        }
        public async Task<LinkModel> GetLinkClrVCAsync(string id)
        {
            var result = await _context.Links.AsNoTracking()
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Include(l => l.Clr)
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();

            var vc = await _context.VerifiableCredentials.AsNoTracking()
                .Where(e => e.ParentCredentialPackageId == result.Clr.CredentialPackageId)
                .FirstOrDefaultAsync();

            result.Clr.ParentVerifiableCredential = vc;
            //TODO EF Core config issue, wont populate this via include???
            var cp = await _context.CredentialPackages.FirstOrDefaultAsync(p => p.Id == result.Clr.CredentialPackageId);
            result.Clr.CredentialPackage = cp;
            return result;
        }
        //End V2 *************************************************************************************************
        public async Task<LinkModel> AddAsync(LinkModel input)
        {
            await _context.Links.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task DeleteAsync(string id)
        {
            var item = await _context.Links
                    .Include(l => l.Shares)
                    .ThenInclude(s => s.Messages)
                    .SingleAsync(x => x.Id == id);

            item.Delete();
            //_context.Links.Remove(item);

            await _context.SaveChangesAsync();
        }
        public async Task<LinkModel> UpdateAsync(LinkModel input)
        {
            _context.Entry(input).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return input;
        }
        public async Task<List<LinkModel>> GetAllAsync(string userId)
        {
            var result = await _context.Links.AsNoTracking()
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }
        public async Task<List<LinkModel>> GetAllDeepAsync(string userId)
        {
            var result = await _context.Links.AsNoTracking()
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ParentClrSet)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Where(l => l.UserId == userId)
                .ToListAsync();

            return result;
        }

        public IQueryable<LinkModel> GetAllDeep(string userId)
        {
            var result = _context.Links.AsNoTracking()
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.CredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ParentClrSet)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Where(l => l.UserId == userId);

            return result;
        }

        public async Task<LinkModel> GetAsync(string userId, int clrId)
        {
            return await _context.Links
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .AsNoTracking()
                .OrderByDescending(l => l.CreatedAt)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ClrForeignKey == clrId);
        }
        public async Task<LinkModel> GetDeepAsync(string id)
        {
            var result = await _context.Links.AsNoTracking()
                .Include(l => l.Shares)
                .Include(l => l.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Include(l => l.Clr)
                .ThenInclude(l => l.ParentVerifiableCredential)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ParentClrSet)
                .ThenInclude(l => l.ParentVerifiableCredential)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Include(l => l.Clr)
                .ThenInclude(c => c.ParentClrSet)
                .ThenInclude(c => c.ParentCredentialPackage)
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();

            //TODO EF Core config issue, wont populate this via include???
            var cp = await _context.CredentialPackages.FirstOrDefaultAsync(p => p.Id == result.Clr.CredentialPackageId);
            result.Clr.CredentialPackage = cp;
            return result;
        }

        public async Task<ShareModel> AddShareAsync(ShareModel input)
        {
            await _context.Shares.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }
    }
}
