using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class ClrsController : ApiController<ClrsController>
    {
        private readonly ClrsService _clrsService;

        public ClrsController(ILogger<ClrsController> logger, ClrsService clrsService) : base(logger)
        {
            _clrsService = clrsService;
        }

        [HttpGet("Achievements/Associations/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAchievementAssociations(int clrId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);

                var associationVMs = await _clrsService.GetAchievementAssociationVMListAsync(clrId, id);

                return ApiOk(associationVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAchievementAssociations", null);
                throw;
            }
        }

        [HttpGet("Assertions/Achievements/Alignments/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAssertionAchievementAlignments(int clrId, int assertionId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);

                var alignmentVMs = await _clrsService.GetAssertionAchievementAlignmentVMListAsync(clrId, assertionId, id);

                return ApiOk(alignmentVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAchievementAlignments", null);
                throw;
            }
        }

        [HttpGet("Achievements/Alignments/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAchievementAlignments(int clrId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);

                var alignmentVMs = await _clrsService.GetAchievementAlignmentVMListAsync(clrId, id);

                return ApiOk(alignmentVMs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAchievementAlignments", null);
                throw;
            }
        }
        [HttpGet("Assertions/Results/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAssertionResults(int clrId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);

                var assertionResultVM = await _clrsService.GetResultVMListAsync(clrId, id);

                return ApiOk(assertionResultVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAssertionResults", null);
                throw;
            }
        }
        [HttpGet("Assertions/Evidence/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAssertionEvidence(int clrId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);
                var assertionResultVM = await _clrsService.GetEvidenceVMListAsync(clrId, id);

                return ApiOk(assertionResultVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAssertionEvidence", null);
                throw;
            }
        }
        [HttpGet("Assertions/Endorsements/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAssertionEndorsements(int clrId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);

                var endorsementVMList = await _clrsService.GetAssertionEndorsementVMListAsync(clrId, id);

                return ApiOk(endorsementVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAssertionEndorsements", null);
                throw;
            }
        }

        [HttpGet("Achievements/Endorsements/{clrId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetAchievementEndorsements(int clrId, string id)
        {
            try
            {
                id = HttpUtility.UrlDecode(id);

                var endorsementVMList = await _clrsService.GetAchievementEndorsementVMListAsync(clrId, id);

                return ApiOk(endorsementVMList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ClrsController.GetAssertionEndorsements", null);
                throw;
            }
        }
    }
}
