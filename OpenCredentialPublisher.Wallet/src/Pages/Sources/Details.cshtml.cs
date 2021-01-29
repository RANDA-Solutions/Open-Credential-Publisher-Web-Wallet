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
        private readonly WalletDbContext _context;
        private readonly ClrService _clrService;

        public DetailsModel(
            WalletDbContext context,
            IHttpClientFactory factory,
            IConfiguration configuration,
            ClrService clrService)
        {
            _context = context;
            _clrService = clrService;
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

            await _clrService.RefreshClrsAsync(this, id);

            // Return redirect to this age so user can display CLRs
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

            Authorization = await _context.Authorizations
                .Include(a => a.Clrs)
                .Include(a => a.Source)
                .ThenInclude(s => s.DiscoveryDocument)
                .SingleOrDefaultAsync(a => a.Id == id && a.UserId == User.UserId());

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

            if (TokenIsValid()) return;

            ModelState.AddModelError(string.Empty, "The access token has expired and cannot be refreshed.");
        }

        /// <summary>
        /// Returns false if the access token has expired and cannot be refreshed.
        /// </summary>
        private bool TokenIsValid()
        {
            // Make sure the token has not expired

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(Authorization.AccessToken);
            if (token.ValidTo >= DateTime.UtcNow)
            {
                return true;
            }

            // If it has expired, check for a refresh token

            return Authorization.RefreshToken != null;
        }
   }
}