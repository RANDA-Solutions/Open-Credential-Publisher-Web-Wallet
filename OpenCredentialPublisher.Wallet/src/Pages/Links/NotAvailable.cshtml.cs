using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenCredentialPublisher.Services.Implementations;
using System.ComponentModel.DataAnnotations;
using OpenCredentialPublisher.Services.Extensions;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{ 
    public class NotAvailableModel: PageModel
    {

        public NotAvailableModel()
        {
        }

        public async Task OnGet(string id)
        {
        }
    }
}