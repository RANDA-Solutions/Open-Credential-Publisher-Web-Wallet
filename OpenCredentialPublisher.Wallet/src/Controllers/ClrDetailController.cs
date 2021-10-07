using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    [ApiController]
    public class ClrDetailController : ControllerBase
    {
        //TODO Protect with CCG in addition to SameSite ?
        private readonly ClrDetailService _clrDetailService;
        private readonly RevocationService _revocationService;
        private readonly SiteSettingsOptions _siteSettings;
        private readonly ILogger<ClrDetailController> _logger;
        private List<CredentialPackageViewModel> CredentialPackageVMs { get; set; } = new List<CredentialPackageViewModel>();
        public ClrDetailController(ILogger<ClrDetailController> logger, ClrDetailService clrDetailService
            , RevocationService revocationService, IOptions<SiteSettingsOptions> siteSettings)
        {
            _clrDetailService = clrDetailService;
            _revocationService = revocationService;
            _siteSettings = siteSettings?.Value ?? throw new NullReferenceException("Site settings were not set.");
            _logger = logger;
        }


        /// <summary>
        /// Gets learner information for the specified CLR
        /// GET api/ClrDetail/Learner/{id}
        /// </summary>
        /// <returns>Learner view model (ClrProfileVM)</returns>
        [Route("api/ClrDetail/Learner/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetLearner(int id)
        {
            try
            {
                var learnerVM = await _clrDetailService.GetClrLearnerVMAsync(id);

                return ApiOk(learnerVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetLearner", null);
                throw;
            }
        }

        /// <summary>
        /// Gets publisher information for the specified CLR
        /// GET api/ClrDetail/Publisher/{id}
        /// </summary>
        /// <returns>Publisher view model (ClrProfileVM)</returns>
        [Route("api/ClrDetail/Publisher/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetPublisher(int id)
        {
            try
            {
                var learnerVM = await _clrDetailService.GetClrPublisherVMAsync(id);

                return ApiOk(learnerVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetPublisher", null);
                throw;
            }
        }

        /// <summary>
        /// Gets Clr verificationDType
        /// GET api/ClrDetail/Verification/{clrId}
        /// </summary>
        /// <returns>VerificationDType</returns>
        [Route("api/ClrDetail/Verification/{clrId}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrVerification(int clrId)
        {
            try
            {
                var verification = await _clrDetailService.GetClrVerificationAsync(clrId);

                return ApiOk(verification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetClrVerification", null);
                throw;
            }
        }

        /// <summary>
        /// Gets top level Assertions for the specified CLR
        /// GET api/ClrDetail/ParentAssertions/{id}
        /// </summary>
        /// <returns>List of strings (id's)</returns>
        [Route("api/ClrDetail/ParentAssertions/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetParentAssertions(int id, bool isShare)
        {
            try
            {
                var assertionIds = await _clrDetailService.GetClrParentAssertionIdsAsync(id, isShare);

                return ApiOk(assertionIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetParentAssertions", null);
                throw;
            }
        }

        /// <summary>
        /// Gets children Assertions for the specified CLR/Assertion
        /// GET api/ClrDetail/{clrId}/ChildAssertions/{id}
        /// </summary>
        /// <returns>List of strings (id's)</returns>
        [Route("api/ClrDetail/ChildAssertions/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetChildAssertions(int id, int assertionId, bool isShare)
        {
            try
            {
                var assertionIds = await _clrDetailService.GetClrChildAssertionIdsAsync(id, assertionId, isShare);

                return ApiOk(assertionIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetChildAssertions", null);
                throw;
            }
        }

        /// <summary>
        /// Gets top level Assertion for the specified CLR & assertion id
        /// GET api/ClrDetail/{clrId}/ClrAssertion/{assertionId}
        /// </summary>
        /// <returns>List of strings (id's)</returns>
        [Route("api/ClrDetail/Achievement/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrAchievement(int id, string achievementId)
        {
            try
            {
                achievementId = HttpUtility.UrlDecode(achievementId);
                var ach = await _clrDetailService.GetClrAchievementVMAsync(id, achievementId);

                return ApiOk(ach);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetClrAssertion", null);
                throw;
            }
        }
        //End V2 *************************************************************************************************

        /// <summary>
        /// Gets publisher information for the specified CLR
        /// GET api/ClrDetail/Publisher/{id}
        /// </summary>
        /// <returns>Publisher view model (ClrProfileVM)</returns>
        [Route("api/ClrDetail/Association/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAssociation(int id, string targetId)
        {
            try
            {
                targetId = HttpUtility.UrlDecode(targetId);
                var assertions = await _clrDetailService.GetClrParentAssertionsAsync(id);
                var aa = assertions.FirstOrDefault(a => a.Achievement.Id == targetId);
                var vm = new AssociationVM();
                if (aa != null)
                {
                    if (Uri.TryCreate(targetId, UriKind.Absolute, out var uri))
                    {
                        if (uri.Scheme == "http" || uri.Scheme == "https")
                        {
                            vm.Uri = uri.AbsoluteUri;
                        }
                        else
                        {
                            vm.Text = targetId;
                        }
                    }
                    else
                    {
                        vm.Text = targetId;
                    }
                }
                else
                {
                    vm.Text = aa.Achievement.Name;
                }

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetAssociation", null);
                throw;
            }
        }
        /// <summary>
        /// Gets top level Assertion for the specified CLR & assertion id
        /// GET api/ClrDetail/{clrId}/ClrAssertion/{assertionId}
        /// </summary>
        /// <returns>List of strings (id's)</returns>
        [Route("api/ClrDetail/ClrAssertion/{id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetClrAssertion(int id, string assertionId)
        {
            try
            {
                _logger.LogInformation("ClrDetailController.GetClrAssertion called");
                assertionId = HttpUtility.UrlDecode(assertionId);
                if (assertionId == "urn:uuid:8772ac79-6079-499c-b3e8-b55852fce47b")
                {
                    _logger.LogInformation("ClrDetailController.GetClrAssertion called");
                }
                var assertion = await _clrDetailService.GetAssertionWithAchievementVMAsync(id, assertionId);

                return ApiOk(assertion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrDetailController.GetClrAssertion", null);
                throw;
            }
        }
        private OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        private OkObjectResult ApiModelInvalid(ModelStateDictionary modelState)
        {
            return Ok(new ApiBadRequestResponse(modelState));
        }
    }
}
