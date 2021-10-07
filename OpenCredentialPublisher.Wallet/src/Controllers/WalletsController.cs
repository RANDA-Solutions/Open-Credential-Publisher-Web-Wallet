using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Commands;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class WalletsController : SecureApiController<WalletsController>
    {
        private readonly IMediator _mediator;
        private readonly WalletRelationshipService _walletService;
        private readonly CredentialService _credentialService;
        private List<CredentialPackageViewModel> CredentialPackageVMs { get; set; } = new List<CredentialPackageViewModel>();
        public WalletsController(UserManager<ApplicationUser> userManager, ILogger<WalletsController> logger, WalletRelationshipService walletService
            , IMediator mediator, CredentialService credentialService  ) : base(userManager, logger)
        {
            _mediator = mediator;
            _walletService = walletService;
            _credentialService = credentialService;
        }

        /// <summary>
        /// Gets all UserPreferences for the current user
        /// GET api/userprefs
        /// </summary>
        /// <returns>Array of UserPreferences (Name/Value)</returns>
        [HttpGet("WalletList")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetWalletList()
        {
            try
            {
                var wallets = await _walletService.GetWalletRelationships(_userId)
                    .Select(w => WalletVM.FromWalletRelationship(w))
                    .ToListAsync();

                return ApiOk(wallets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WalletsController.GetWalletList", null);
                throw;
            }
        }
        [HttpPost("Connect")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> OnPost()
        {
            await _mediator.Publish(new GenerateInvitationCommand(_userId));

            return ApiOk(null);
        }
        /// <summary>
        /// Delete relationship
        /// POST api/wallets/Delete/{id}
        /// </summary>
        /// <returns>ok </returns>
        [HttpPost("Delete/{id}")]  /* success returns 200 - Ok */
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ModelStateDictionary modelState = new ModelStateDictionary();
                await _walletService.DeleteRelationshipAsync(id);
                return ApiOk(null);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WalletsController.Delete", null);
                throw;
            }
        }
        /// <summary>
        /// Gets a walletrelationship
        /// GET api/wallets/Relationship/{id}
        /// </summary>
        /// <returns>RelationshipVM</returns>
        [HttpGet("Relationship/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetRelationship(int id)
        {
            try
            {
                var wallet = await _walletService.GetWalletRelationshipByIdAsync(id);

                return ApiOk(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WalletsController.GetWalletList", null);
                throw;
            }
        }
        /// <summary>
        /// Gets an Invitation
        /// GET api/wallets/Inivitation/{id}
        /// </summary>
        /// <returns>InivitationVM</returns>
        [HttpGet("Connection/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetConnection(int id)
        {
            try
            {
                var relationship = await _walletService.GetWalletRelationships(_userId).FirstOrDefaultAsync(w => w.Id == id);
                if (relationship == null)
                {
                    ModelState.AddModelError("", "Cannot find the connection requested.");
                    return ApiModelInvalid(ModelState);
                }
                var invite = new ConnectionViewModel {  Name = relationship.WalletName, Id = id, RelationshipDid = relationship.RelationshipDid, DateCreated = relationship.CreatedOn.ToString("g") };

                return ApiOk(invite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(WalletsController.GetConnection), null);
                throw;
            }
        }

        /// <summary>
        /// Gets an Invitation
        /// GET api/wallets/Inivitation/{id}
        /// </summary>
        /// <returns>InivitationVM</returns>
        [HttpGet("Invitation/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetInvitation(int id)
        {
            try
            {
                var relationship = await _walletService.GetWalletRelationships(_userId).FirstOrDefaultAsync(w => w.Id == id);
                if (relationship == null)
                {
                    ModelState.AddModelError("", "Cannot find the invitation requested.");
                    return ApiModelInvalid(ModelState);
                }
                var url = new UriBuilder(relationship.InviteUrl);
                var queryString = HttpUtility.ParseQueryString(url.Query);
                var payload = queryString["c_i"];
                var invite = new InvitationVM { Nickname = relationship.WalletName, Id = id, Payload = payload, QRCodeString = _credentialService.CreateQRCode(relationship.InviteUrl), HideQRCode = relationship.IsConnected };

                return ApiOk(invite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(WalletsController.GetInvitation), null);
                throw;
            }
        }

        [HttpPost("SaveConnection/{id}")]
        public async Task<IActionResult> SaveConnection(int id, ConnectionViewModel input)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _walletService.UpdateRelationshipNameAsync(_userId, id, input.Name);
                    return ApiOk(null);
                }

                // If we got this far, something failed, redisplay form
                return ApiModelInvalid(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPost("{id}/Send/{pkgId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> OnPost(int id, int pkgId)
        {
            try
            {
                if (await _credentialService.PackageExistsAsync(_userId, pkgId)
                    && await _walletService.ExistsAsync(_userId, id))
                {
                    await _mediator.Publish(new StartCredentialOfferCommand(_userId, id, pkgId));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
            return ApiOk(null);
        }
        /// <summary>
        /// Gets ViewModel for sending a wallet/credential
        /// GET api/wallets/WalletSendVM/{id}
        /// </summary>
        /// <returns>WalletSendVM</returns>
        [HttpGet("WalletSendVM/{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]  /* success returns 200 - Ok */
        public async Task<IActionResult> GetWalletSendVM(int id)
        {
            try
            {
                var vm = await  _credentialService.GetSendWalletVMAsync(ModelState, _userId, id);
                if (!ModelState.IsValid)
                {
                    return ApiModelInvalid(ModelState);
                }

                return ApiOk(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WalletsController.GetWalletSendVM", null);
                throw;
            }
        }
    }
}
