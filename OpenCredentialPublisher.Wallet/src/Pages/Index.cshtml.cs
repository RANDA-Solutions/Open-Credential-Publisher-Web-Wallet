using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace OpenCredentialPublisher.ClrWallet.Pages
{
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }

        public List<AuthorizationModel> Authorizations { get; set; }

        public List<CredentialPackageModel> CredentialPackages { get; set; }

        public List<LinkModel> Links { get; set; }
        
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated) return Page();


            return RedirectToPage("/Credentials/Index");
        }
    }
}
