using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Dtos.Account_Manage;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.nG;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Wallet.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Controllers.Account
{
    [Route("api/account/[controller]")]
    [Authorize]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {

        private readonly IEmailSender _emailSender;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ILogger<ChangePasswordController> _logger;
        protected String _userId => User.UserId();
        public ChangePasswordController(IEmailSender emailSender, UserManager<ApplicationUser> userManager, ILogger<ChangePasswordController> logger) 
        {
            _emailSender = emailSender;
            _logger = logger;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost("")]
        public async Task<IActionResult> ChangePassword([FromBody]PasswordInput input)
        {
            var user = await _userManager.FindByIdAsync(User.UserId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{User.UserId()}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return Ok(new ApiBadRequestResponse("Could not update password"));
            }

            //await _signInManager.RefreshSignInAsync(user);

            return ApiOk("Password updated.");
        }

        public OkObjectResult ApiOk(object model, string message = null, string redirectUrl = null)
        {
            return Ok(new ApiOkResponse(model, message, redirectUrl));
        }
        
    }
}
