using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.ClrWallet.Utilities;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using SysJson = System.Text.Json;
//2021-06-17 EF Tracking OK
namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class CredentialsController : SecureApiController<CredentialsController>
    {
        private readonly CredentialService _credentialService;
        private readonly RevocationService _revocationService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkService _linkService;
        private readonly CredentialPackageService _credentialPackageService;
        private readonly ETLService _etlService;
        private readonly BadgrService _badgrService;

        private List<CredentialPackageViewModel> CredentialPackageVMs { get; set; } = new List<CredentialPackageViewModel>();
        public CredentialsController(UserManager<ApplicationUser> userManager, ILogger<CredentialsController> logger, CredentialService credentialService
            , RevocationService revocationService, IOptions<SiteSettingsOptions> siteSettings, ETLService etlService
            , IHttpContextAccessor httpContextAccessor, LinkService linkService, CredentialPackageService credentialPackageService
            , BadgrService badgrService) : base(userManager, logger)
        {
            _credentialService = credentialService;
            _revocationService = revocationService;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
            _httpContextAccessor = httpContextAccessor;
            _linkService = linkService;
            _etlService = etlService;
            _credentialPackageService = credentialPackageService;
            _badgrService = badgrService;
        }

        /// <summary>
        /// Gets all UserPreferences for the current user
        /// GET api/userprefs
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet("PackageList")]
        [ProducesResponseType(200, Type = typeof(PackageListVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPackageList()
        {
            try
            {
                var packageVMs = await _credentialService.GetPackageVMListAsync(_userId);

                var links = await _credentialService.GetAllLinksAsync(_userId);

                var vm = new PackageListVM { UserId = _userId, EnableCollections = _siteSettings.EnableCollections, EnableSource = _siteSettings.EnableSource };

                vm.Packages.AddRange(packageVMs);

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetPackageList", null);
                throw;
            }
        }

        /// <summary>
        /// Gets all CLRs for the specified package
        /// GET api/PackageClrs/{id}
        /// </summary>
        /// <returns>Array of CLRVM's</returns>
        [HttpGet("PackageClrs/{id}")]
        [ProducesResponseType(200, Type = typeof(PackageVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPackageClrs(int id)
        {
            try
            {
                var clrVMs = new List<ClrVM>();
                var clrs = await _credentialService.GetPackageClrsAsync(id);
                foreach (var clr in clrs)
                {
                    var clrVM = ClrVM.FromModel(clr);
                    clrVM.EnableSmartResume = _siteSettings.EnableSmartResume;
                    clrVMs.Add(clrVM);
                }

                return ApiOk(clrVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetPackageClrs", null);
                throw;
            }
        }

        /// <summary>
        /// Gets a single CredentialPackage
        /// GET api/credentials/Package/id
        /// </summary>
        /// <returns>Single PackageVM</returns>
        [HttpGet("Package/{id}")]
        [ProducesResponseType(200, Type = typeof(PackageVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPackage(int id)
        {
            try
            {
                var packageVM = await _credentialService.GetPackageVMAsync(id, _userId);
                return ApiOk(packageVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetPackage", null);
                throw;
            }
        }
        /// <summary>
        /// Converts Open Badge to a CLR
        /// Post api/OpenBadge/ClrEmbed/id
        /// </summary>
        /// <returns>id of new package</returns>
        [HttpPost("OpenBadge/ClrEmbed/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> ClrEmbed(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(_userId);
                var newId = await _etlService.CreateClrFromBadgeAsync(id, user);

                return ApiOk(newId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.ClrEmbed", null);
                throw;
            }
        }
        //V1 **************************************************************************************************

        /// <summary>
        /// SoftDelete's a single CredentialPackage
        /// Post api/credentials/Package/Delete/id
        /// </summary>
        /// <returns>Single PackageVM</returns>
        [HttpPost("Package/Delete/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> DeletePackage(int id)
        {
            try
            {
                await _etlService.DeletePackageAsync(id);

                return ApiOk(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.DeletePackage", null);
                throw;
            }
        }
        /// <summary>
        /// Gets all assertions for a CLR
        /// GET api/credentials/ClrAssertions/{id}
        /// </summary>
        /// <returns>Array of ClrAssertions</returns>
        [HttpGet("Assertions/{id}")]
        [ProducesResponseType(200, Type = typeof(PackageVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAssertion(int id)
        {
            try
            {
                var assertion = await _credentialService.GetClrAssertionAsync(id); 

                return ApiOk(ClrAssertionVM.FromClrAssertion(assertion));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetAssertion", null);
                throw;
            }
        }
        /// <summary>
        /// Gets all assertions for a CLR
        /// GET api/credentials/ClrAssertions/{id}
        /// </summary>
        /// <returns>Array of ClrAssertions</returns>
        [HttpGet("ClrAssertions/{id}")]
        [ProducesResponseType(200, Type = typeof(PackageVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrAssertions(int id)
        {
            try
            {
                var clrAssertionVMs = new List<AssertionHeaderVM>();
                var assertions = await _credentialService.GetClrAssertionsAsync(id);
                clrAssertionVMs.AddRange(assertions.Select(ca => new AssertionHeaderVM { Id = ca.Id, DisplayName = ca.Achievement?.Name, IssuedOn = ca.IssuedOn == DateTime.MinValue || ca.IssuedOn == null ? null : ca.IssuedOn }));

                return ApiOk(clrAssertionVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetClrAssertions", null);
                throw;
            }
        }
        /// <summary>
        /// Gets assertion detail for a CLR/Assertion
        /// GET api/credentials/Clr{clrId}/AssertionDetail/{id}
        /// </summary>
        /// <returns>single ClrAssertion</returns>
        [HttpGet("Clr/{clrId}/AssertionDetail")]
        [ProducesResponseType(200, Type = typeof(PackageVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrAssertionDetail(int clrId, string assertionId)
        {
            try
            {
                assertionId = HttpUtility.UrlDecode(assertionId);
                var assertion = await _credentialService.GetClrAssertionDetailAsync(clrId, assertionId);
                var clrAssertionVM = ClrAssertionVM.FromClrAssertion(assertion);

                return ApiOk(clrAssertionVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetClrAssertionDetail", null);
                throw;
            }
        }
        /// <summary>
        /// Gets all ClrVM's for a user
        /// GET api/credentials/Clrs
        /// </summary>
        /// <returns>Array of ClrVMs</returns>
        [HttpGet("Clrs")]
        [ProducesResponseType(200, Type = typeof(ClrCollectionVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrs()
        {
            try
            {
                var clrCollection = new ClrCollectionVM();
                var clrs = await _credentialService.GetAllClrsShallowAsync(_userId);


                foreach (var clr in clrs)
                {
                    clrCollection.Clrs.Add(ClrVM.FromModel(clr));
                }

                return ApiOk(clrCollection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetClrs", null);
                throw;
            }
        }
    /// <summary>
    /// Creates a CLR Collection
    /// POST api/credentials/ClrCollection
    /// </summary>
    /// <returns>the cLRcOLLECTION</returns>
    [HttpPost("Upload"), DisableRequestSizeLimit]
    [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
    public async Task<IActionResult> Upload()
    {
            try
            {
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var clrJson = await FileHelpers.ProcessFormFile("ClrUpload", file, ModelState);
                    ModelState.Clear();
                    if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

                    var result = await _etlService.ProcessJson(_httpContextAccessor.HttpContext.Request, ModelState, _userId, fileName, clrJson, null);

                    if (result.HasError)
                    {
                        foreach (var err in result.ErrorMessages)
                        {
                            ModelState.AddModelError("ClrUpload", err);
                        }
                    }

                    if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

                    return ApiOk(null);
                }
                else
                {
                    ModelState.AddModelError("ClrUpload", "Please select a file to upload.");
                    return ApiModelInvalid(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, ex.Message);
                return StatusCode(500, $"Internal server error: {ex}");
            }
            //if (Request.Form.Files.Count == 0)
            //{
            //    return BadRequest();
            //}

            //if (ClrUpload == null)
            //{
            //    ModelState.AddModelError("ClrUpload", "Please select a file to upload.");
            //    return ApiModelInvalid(ModelState);
            //}

            //var clrJson = await FileHelpers.ProcessFormFile("ClrUpload", ClrUpload, ModelState);

            //if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

            //await _credentialService.ProcessJson(_httpContextAccessor.HttpContext.Request, _userId, clrJson, null);

            //if (!ModelState.IsValid) return ApiModelInvalid(ModelState);

            //return ApiOk(null);
        }

    /// <summary>
    /// Creates a CLR Collection
    /// POST api/credentials/ClrCollection
    /// </summary>
    /// <returns>the cLRcOLLECTION</returns>
    [HttpPost("ClrCollection")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> OnPost(ClrCollectionVM input)
        {
            if (input.Clrs.All(x => x.IsSelected == false))
            {
                ModelState.AddModelError(nameof(input), "Please select at least one CLR.");
            }

            if (!ModelState.IsValid)
            {
                return ApiModelInvalid(ModelState);
            }

            await _credentialService.CreateClrFromSelectedAsync(User.JwtUserId(), input.Name, input.Clrs.Where(x => x.IsSelected).Select(x => x.Id).ToArray());

            return ApiOk(null);
        }
        /// <summary>
        /// Gets the VerifiableCredential for the specified package
        /// GET api/PackageVerifiableCredential/{id}
        /// </summary>
        /// <returns>VerifiableCredentialModel</returns>
        [HttpGet("PackageVerifiableCredential/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPackageVerifiableCredential(int id)
        {
            try
            {
                var vm = await _credentialService.GetPackageVerifiableCredentialVMAsync(id);

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetPackageVerifiableCredential", null);
                throw;
            }
        }
        /// <summary>
        /// Gets a single CLR view model
        /// GET api/credentials/clrs/{id}
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet("Clrs/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClr(int id)
        {
            try
            {
                var clr = await _credentialService.GetSingleClrAsync(id);
                
                return ApiOk(ClrVM.FromModel(clr));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetPackageClrs", null);
                throw;
            }
        }
        /// <summary>
        /// Gets all UserPreferences for the current user
        /// GET api/userprefs
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet("BackpackPackage/{id}")]
        [ProducesResponseType(200, Type = typeof(PackageVM))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetBackpackPackage(int id)
        {
            try
            {
                var badgeName = string.Empty;
                var badgeDescription = string.Empty;
                var issuerName = string.Empty;
                var pkg = await _credentialService.GetBackpackPackageAsync(_userId, id);

                var vm = new BackpackPackageVM { Id = id, IsBadgr = pkg.BadgrBackpack.IsBadgr};
                foreach (var ba in pkg.BadgrBackpack.BadgrAssertions)
                {
                    if (ba.IsBadgr)
                    {
                        dynamic badgeClass = JsonConvert.DeserializeObject<ExpandoObject>(ba.BadgeClassJson, new ExpandoObjectConverter());
                        badgeName = ((IDictionary<string, object>)badgeClass)["name"].ToString();
                        badgeDescription = ((IDictionary<string, object>)badgeClass)["description"].ToString();
                        issuerName = "";
                        if (!string.IsNullOrWhiteSpace(ba.IssuerJson))
                        {
                            dynamic issuer = JsonConvert.DeserializeObject<ExpandoObject>(ba.IssuerJson, new ExpandoObjectConverter());
                            issuerName = ((IDictionary<string, object>)issuer)["name"].ToString();
                        }
                        else
                        {
                            issuerName = "Issuer information not available";
                        }
                    }
                    else
                    {
                        if (ba.IsValidJson)
                        {
                            badgeName = ba.Id;
                        }
                        else
                        {
                            badgeName = "Invalid Badge";
                        }
                        issuerName = "Issuer information not available";

                    }
                    var obVM = new OpenBadgeVM
                    {
                         IdIsUrl = Uri.IsWellFormedUriString(ba.Id, UriKind.Absolute),
                         Id = ba.BadgrAssertionId,
                         BadgeName = badgeName,
                         IssuerName = issuerName,
                         BadgeDescription = badgeDescription,
                         BadgrAssertionId = ba.Id,
                         BadgeImage = ba.Image,
                         IsSelected = false
                    };
                    vm.Badges.Add(obVM);
                }           

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CredentialsController.GetPackageList", null);
                throw;
            }
        }
        private ClrDType GetRawClr(ClrModel clr)
        {
            var rawClr = new ClrDType();

            if (clr == null)
            {
                return rawClr;
            }

            if (!string.IsNullOrEmpty(clr.SignedClr))
            {
                rawClr = clr.SignedClr.DeserializePayload<ClrDType>();
            }
            else
            {
                rawClr = SysJson.JsonSerializer.Deserialize<ClrDType>(clr.Json);
            }
            return rawClr;
        }
        private int CountAssertions(CredentialPackageViewModel cp)
        {

            //TODO nG CountAssertions
            var cnt = 0;
            //switch (cp.TypeId)
            //{
            //    case PackageTypeEnum.Clr:

            //        break;
            //    default:
            //        break;
            //}

            //if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.Clr)
            //{
            //    var clrVM = ClrViewModel.FromClrModel(pkg.Clr);
            //    pkgVM.ClrVM = clrVM;
            //    pkgVM.AssertionsCount = clrVM.AllAssertions.Count;
            //    pkgVM.Pdfs.AddRange(clrVM.Pdfs);
            //}
            //else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.ClrSet)
            //{
            //    var clrSetVM = ClrSetViewModel.FromClrSetModel(pkg.ClrSet);
            //    pkgVM.ClrSetVM = clrSetVM;
            //    pkgVM.AssertionsCount = clrSetVM?.AssertionsCount ?? 0;
            //    pkgVM.Pdfs.AddRange(clrSetVM?.Pdfs);
            //}
            //else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential)
            //{
            //    var vcVM = VerifiableCredentialViewModel.FromVerifiableCredentialModel(pkg.VerifiableCredential);
            //    pkgVM.VerifiableCredentialVM = vcVM;
            //    pkgVM.AssertionsCount = vcVM.AssertionsCount;
            //    pkgVM.Pdfs.AddRange(vcVM.Pdfs);
            //}
            //else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.OpenBadge)
            //{
            //    var vcVM = ClrViewModel.FromBackpack(pkg.BadgrBackpack);
            //    pkgVM.ClrVM = vcVM;
            //    pkgVM.AssertionsCount = vcVM.AllAssertions.Count;
            //    pkgVM.Pdfs.AddRange(vcVM.Pdfs);
            //}
            //else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.OpenBadgeConnect)
            //{
            //    var vcVM = ClrViewModel.FromBackpack(pkg.BadgrBackpack);
            //    pkgVM.ClrVM = vcVM;
            //    pkgVM.AssertionsCount = vcVM.AllAssertions.Count;
            //    pkgVM.Pdfs.AddRange(vcVM.Pdfs);
            //}

            return cnt;
        }
    }
}
