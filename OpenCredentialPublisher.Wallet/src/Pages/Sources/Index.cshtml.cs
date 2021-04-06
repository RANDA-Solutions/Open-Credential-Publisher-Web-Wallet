using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class IndexModel : PageModel
    {
        private readonly AuthorizationsService _authorizationsService;

        public IndexModel(AuthorizationsService authorizationsService)
        {
            _authorizationsService = authorizationsService;
        }

        public IList<AuthorizationModel> Authorizations { get;set; }

        public async Task OnGet()
        {
            Authorizations = await _authorizationsService.GetAllAsync(User.UserId());
        }
    }
}