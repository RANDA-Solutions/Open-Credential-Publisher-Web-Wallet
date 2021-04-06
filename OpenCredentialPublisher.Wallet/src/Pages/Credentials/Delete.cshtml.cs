using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class DeleteModel : PageModel
    {
        private readonly CredentialService _credentialService;

        public DeleteModel(CredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        public ClrModel Clr { get; set; }

        public async Task OnGet([Required] int? id)
        {
            await OnPageLoadAsync(id);
        }

        public async Task<IActionResult> OnPost([Required] int? id)
        {
            await OnPageLoadAsync(id);

            if (!ModelState.IsValid) return Page();

            // Remove the CLR
            await _credentialService.DeleteClrAsync(Clr.Id);

            return RedirectToPage("./Index");
        }

        private async Task OnPageLoadAsync(int? id)
        {
            if (!ModelState.IsValid) return;

            Clr = await _credentialService.GetClrAsync(id.Value);

            if (Clr == null)
            {
                ModelState.AddModelError(string.Empty, $"Cannot find CLR {id}.");
            }
        }
    }
}
