using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Services.Extensions;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class DetailsModel : PageModel
    {
        private readonly ClrService _clrService;
        private readonly AuthorizationsService _authorizationsService;
        private readonly BadgrService _badgrService;

        public DetailsModel(
            IHttpClientFactory factory,
            IConfiguration configuration,
            ClrService clrService,
            BadgrService badgrService,
            AuthorizationsService authorizationsService)
        {
            _clrService = clrService;
            _badgrService = badgrService;
            _authorizationsService = authorizationsService;
        }

        public AuthorizationModel Authorization { get; set; }

        public async Task<IActionResult> OnGet(string id, bool? refresh)
        {
            if (refresh.HasValue && refresh.Value)
            {
                return await OnPost(id);
            }

            await OnPageLoad(id);

            return Page();
        }

        /// <summary>
        /// Refresh the list of CLRs
        /// </summary>
        /// <param name="id">The authorization id</param>
        public async Task<IActionResult> OnPost(string id)
        {
            await OnPageLoad(id);

            if (!ModelState.IsValid) return Page();

            if ((await _authorizationsService.GetSourceTypeAsync(id)).Equals(SourceTypeEnum.OpenBadge))
            {
                await _badgrService.RefreshBackpackAsync(this, id);
            }
            else
            {
                await _clrService.RefreshClrsAsync(this, id);
            }

            // Return redirect to this page so user can display CLRs
            // and return back to this page without being prompted to resubmit

            if (ModelState.IsValid)
            {
                return RedirectToPage("./Details", new {Authorization.Id});
            }

            return Page();
        }

        private async Task OnPageLoad(string id)
        {
            if (id == null)
            {
                ModelState.AddModelError(string.Empty, "Missing authorization id.");
                return;
            }

            Authorization = await _authorizationsService.GetDeepAsync(id);

            if (Authorization == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find authorization {id}.");
                return;
            }

            if (Authorization.AccessToken == null)
            {
                ModelState.AddModelError(string.Empty, "No access token.");
                return;
            }

            if (await _authorizationsService.RefreshTokenAsync(this, Authorization)) return;

            ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
        }        
   }
}