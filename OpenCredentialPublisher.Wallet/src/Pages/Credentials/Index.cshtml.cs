using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.ClrLibrary;
using OpenCredentialPublisher.ClrWallet.Utilities;
using OpenCredentialPublisher.Data.Dtos;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class IndexModel : PageModel
    {
        private readonly RevocationService _revocationService;
        private readonly AuthorizationsService _authorizationsService;
        private readonly ClrService _clrService;
        private readonly BadgrService _badgrService;
        private readonly SchemaService _schemaService;
        private readonly CredentialService _credentialService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SiteSettingsOptions _siteSettingsOptions;

        public IndexModel(
            AuthorizationsService authorizationsService, 
            SchemaService schemaService,
            ClrService clrService,
            BadgrService badgrService,
            RevocationService revocationService,
            CredentialService credentialService,
            UserManager<ApplicationUser> userManager,
            IOptions<SiteSettingsOptions> siteSettingsOptions)
        {
            _authorizationsService = authorizationsService;
            _revocationService = revocationService;
            _clrService = clrService;
            _badgrService = badgrService;
            _schemaService = schemaService;
            _credentialService = credentialService;
            _userManager = userManager;
            _siteSettingsOptions = siteSettingsOptions?.Value ?? default(SiteSettingsOptions);
        }

        public bool EnableSource => _siteSettingsOptions.EnableSource;
        public bool EnableCollections => _siteSettingsOptions.EnableCollections;

        public ProfileModel Profile { get; set; }
        public DashboardModel Dashboard { get; set; }

        public List<CredentialPackageViewModel> CredentialPackageVMs { get; set; } = new List<CredentialPackageViewModel>();

        public List<CredentialPackageViewModel> Clrs => CredentialPackageVMs.Where(cp => cp.CredentialPackage.TypeId == PackageTypeEnum.Clr).ToList();
        public List<CredentialPackageViewModel> ClrSets => CredentialPackageVMs.Where(cp => cp.CredentialPackage.TypeId == PackageTypeEnum.ClrSet).ToList();

        public List<LinkModel> Links { get; set; }
        public List<CredentialPackageViewModel> VerifiableCredentials => CredentialPackageVMs.Where(cp => cp.CredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential).ToList();

        
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
            if ((await _authorizationsService.GetSourceTypeAsync(id)).Equals(SourceTypeEnum.OpenBadge))
            {
                await _badgrService.RefreshBackpackAsync(this, id);
            }
            else
            {
                await _clrService.RefreshClrsAsync(this, id);
            }
            
            return Page();
        }

        [UsedImplicitly]
        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            if (!id.HasValue) return Page();

            var clr = await _credentialService.GetClrAsync(id.Value);

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

            await _credentialService.ProcessJson(this, clrJson, null);

            if (!ModelState.IsValid) return Page();

            await LoadPackages();

            return Page();
        }

        
        public async Task<IActionResult> OnGetPdf(int clrId, string assertionId, string evidenceName, int artifactId)
        {
            var clr = await _credentialService.GetClrAsync(User.UserId(), clrId);

            if (clr == null)
            {
                ModelState.AddModelError("", "Something went wrong with that operation.");
            }

            var clrVM = ClrViewModel.FromClrModel(clr);

            var assertionVM = clrVM.AllAssertions.FirstOrDefault(a => a.Assertion.Id == assertionId);
            var evidence = assertionVM.Assertion.Evidence.FirstOrDefault(e => e.Name == evidenceName);
            var artifact = evidence.Artifacts.FirstOrDefault(a => a.ArtifactKey == artifactId);

            return new JsonResult(new PdfGetResponseModel { DataUrl = artifact.Url });
        }


        private async Task LoadPackages()
        {
            var userId = User.UserId();
            var cpIds = CredentialPackageVMs.Select(cp => cp.CredentialPackage.Id).ToArray();
            var credentialPackagesQuery = _credentialService.GetAllDeep(userId);
            credentialPackagesQuery = credentialPackagesQuery.Where(package => cpIds.All(cp => package.Id != cp));

            Links = await _credentialService.GetAllLinksAsync(userId);

            var assertions = 0;
            
            foreach (var cp in credentialPackagesQuery)
            {
                var cpVM = CredentialPackageViewModel.FromCredentialPackageModel(cp);
                await _revocationService.MarkPackageViewModelRevocationsAsync(userId, cpVM);
                assertions += cpVM.AssertionsCount;
                CredentialPackageVMs.Add(cpVM);
            }

            CredentialPackageVMs = CredentialPackageVMs.OrderByDescending(cp => cp.CredentialPackage.CreatedAt).ToList();
            var package = CredentialPackageVMs.FirstOrDefault(cp => cp.HasPdfs && cp.Pdfs.Any(pdf => pdf.ArtifactName.Contains("transcript", StringComparison.OrdinalIgnoreCase)));
            PdfShareViewModel pdfShareViewModel = null;
            if (package != null)
            {
                pdfShareViewModel = package.Pdfs.FirstOrDefault(pdf => pdf.ArtifactName.Contains("transcript", StringComparison.OrdinalIgnoreCase));

            }

            var user = await _userManager.GetUserAsync(User);
            Profile = new ProfileModel
            {
                DisplayName = user.DisplayName,
                HasProfileImage = !String.IsNullOrEmpty(user.ProfileImageUrl),
                ProfileImageUrl = user.ProfileImageUrl,
                Credentials = CredentialPackageVMs.Count(c => c.CredentialPackage.VerifiableCredential != null),
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