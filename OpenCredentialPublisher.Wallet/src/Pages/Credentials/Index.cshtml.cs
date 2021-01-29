using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.ClrWallet.Utilities;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Credentials.Clrs.Utilities;
using OpenCredentialPublisher.Wallet.Extensions;
using OpenCredentialPublisher.Services.Drawing;
using OpenCredentialPublisher.Data.Dtos;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class IndexModel : PageModel
    {
        private readonly WalletDbContext _context;
        private readonly ClrService _clrService;
        private readonly SchemaService _schemaService;
        private readonly CredentialService _credentialService;

        public IndexModel(
            WalletDbContext context,
            SchemaService schemaService,
            ClrService clrService,
            CredentialService credentialService)
        {
            _context = context;
            _clrService = clrService;
            _schemaService = schemaService;
            _credentialService = credentialService;
        }

        public ProfileModel Profile { get; set; }
        public DashboardModel Dashboard { get; set; }

        public List<CredentialPackageModel> CredentialPackages { get; set; } = new List<CredentialPackageModel>();

        public List<CredentialPackageModel> Clrs => CredentialPackages.Where(cp => cp.TypeId == PackageTypeEnum.Clr).ToList();
        public List<CredentialPackageModel> ClrSets => CredentialPackages.Where(cp => cp.TypeId == PackageTypeEnum.ClrSet).ToList();

        public List<LinkModel> Links { get; set; }
        public List<CredentialPackageModel> VerifiableCredentials => CredentialPackages.Where(cp => cp.TypeId == PackageTypeEnum.VerifiableCredential).ToList();

        
        [BindProperty]
        [Display(Name = "CLR")]
        public IFormFile ClrUpload { get; set; }

        public async Task OnGet()
        {
            await LoadPackages();
        }

        /// <summary>
        /// Get fresh copies of all the CLRs from the resource server.
        /// </summary>
        /// <param name="id">The authorization id.</param>
        [UsedImplicitly]
        public async Task<IActionResult> OnPostRefreshAsync(string id)
        {
            await LoadPackages();

            await _clrService.RefreshClrsAsync(this, id);
            
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            if (!id.HasValue) return Page();

            var clr = await _context.Clrs.FindAsync(id);
            if (clr == null) return Page();

            var fileDownloadName = clr.Name ?? clr.PublisherName;

            return File(Encoding.UTF8.GetBytes(clr.Json), ClrConstants.MediaTypes.JsonMediaType, $"{fileDownloadName}.json");
        }

        /// <summary>
        /// Upload a CLR JSON file or a CLR HTML file with embedded JSON.
        /// </summary>
        [UsedImplicitly]
        public async Task<IActionResult> OnPostUploadAsync()
        {
            await LoadPackages();

            if (ClrUpload == null)
            {
                ModelState.AddModelError(nameof(ClrUpload), "Please select a file to upload.");
                return Page();
            }

            var clrJson = await FileHelpers.ProcessFormFile(this, ClrUpload, ModelState);

            if (!ModelState.IsValid) return Page();

            await _credentialService.ProcessJson(this, clrJson);

            if (!ModelState.IsValid) return Page();

            await LoadPackages();

            return Page();
        }

        
        public async Task<IActionResult> OnGetPdf(int clrId, string assertionId, string evidenceName, int artifactId)
        {
            var clr = await _context.Clrs
                .Include(c => c.CredentialPackage)
                .FirstOrDefaultAsync(c => c.Id == clrId && c.CredentialPackage.UserId == User.UserId());
            if (clr == null)
            {
                ModelState.AddModelError("", "Something went wrong with that operation.");
            }

            var model = System.Text.Json.JsonSerializer.Deserialize<ClrViewModel>(clr.Json);
            model.BuildAssertionsTree();

            var assertion = model.AllAssertions.FirstOrDefault(a => a.Id == assertionId);
            var evidence = assertion.Evidence.FirstOrDefault(e => e.Name == evidenceName);
            var artifact = evidence.Artifacts.FirstOrDefault(a => a.ArtifactKey == artifactId);

            return new JsonResult(new PdfGetResponseModel { DataUrl = artifact.Url });
        }


        private async Task LoadPackages()
        {
            var cpIds = CredentialPackages.Select(cp => cp.Id).ToArray();
            var credentialPackages = _context.CredentialPackages
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(p => p.VerifiableCredential)
                .ThenInclude(vc => vc.ClrSets)
                .ThenInclude(sets => sets.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(auth => auth.Source)
                .Include(p => p.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(package => package.UserId == User.UserId() && cpIds.All(cp => package.Id != cp));

            Links = await _context.Links
                .Where(l => l.UserId == User.UserId())
                .ToListAsync();

            var assertions = 0;
            
            foreach (var cp in credentialPackages)
            {
                cp.BuildView();
                assertions += cp.AssertionsCount;
                CredentialPackages.Add(cp);
            }

            CredentialPackages = CredentialPackages.OrderByDescending(cp => cp.CreatedAt).ToList();
            var package = CredentialPackages.FirstOrDefault(cp => cp.HasPdfs && cp.Pdfs.Any(pdf => pdf.ArtifactName.Contains("transcript", StringComparison.OrdinalIgnoreCase)));
            PdfShareViewModel pdfShareViewModel = null;
            if (package != null)
            {
                pdfShareViewModel = package.Pdfs.FirstOrDefault(pdf => pdf.ArtifactName.Contains("transcript", StringComparison.OrdinalIgnoreCase));

            }

            Profile = new ProfileModel
            {
                HasProfileImage = false,
                Credentials = CredentialPackages.Count(c => c.VerifiableCredential != null),
                Achievements = assertions,
                ActiveLinks = Links.Count()
            };

            Dashboard = new DashboardModel
            {
                ShowShareableLinksSection = Links.Any(),
                LatestShareableLink = Links.OrderByDescending(l => l.CreatedAt).FirstOrDefault(),
                NewestPdfTranscript = pdfShareViewModel
            };
        }

        [UsedImplicitly]
        public async Task<IActionResult> ProfileImageAsync()
        {
            await Task.Delay(0);

            return File(new byte[0], "img/png", "image.png");
        }
    }
}