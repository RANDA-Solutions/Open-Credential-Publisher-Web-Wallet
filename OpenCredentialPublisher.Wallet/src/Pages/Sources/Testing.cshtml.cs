using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace OpenCredentialPublisher.ClrWallet.Pages.Sources
{
    public class TestingModel : PageModel
    {
        private readonly AuthorizationsService _authorizationsService;

        public TestingModel(AuthorizationsService authorizationsService)
        {
           
        }
        public async Task<IActionResult> OnPost()
        {
            

            return RedirectToPage("./Testing");
        }
        public IList<AuthorizationModel> Authorizations { get;set; }

        public async Task OnGet()
        {
           
        }
    }
}