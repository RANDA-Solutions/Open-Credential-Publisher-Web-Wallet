using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OpenCredentialPublisher.ClrLibrary.Extensions;
using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class CreateModel : PageModel
    {
        private readonly CredentialService _credentialService;

        public CreateModel(CredentialService credentialService)
        {
            _credentialService = credentialService;
        }

        [BindProperty]
        public List<SelectedClr> Clrs { get;set; }

        [BindProperty, Required]
        public string Name { get; set; }

        public async Task OnGet()
        {
            Clrs = await LoadClrs();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Clrs.All(x => x.Selected == false))
            {
                ModelState.AddModelError(nameof(Clrs), "Please select at least one CLR.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _credentialService.CreateClrFromSelectedAsync(User.UserId(), Name, Clrs.Where(x => x.Selected).Select(x => x.Clr.Id).ToArray());
            return RedirectToPage("./Index");
        }

        public async Task<List<SelectedClr>> LoadClrs()
        {
            var clrs = await _credentialService.GetAllClrsAsync(User.UserId());

            var selectedClrs = clrs.Select( c => new SelectedClr { Clr = c, Selected = false }).ToList();

            return selectedClrs;

        }

        public class SelectedClr
        {
            public ClrModel Clr { get; set; }
            public bool Selected { get; set; }
        }

        public class SelectedPackage
        {
            public CredentialPackageModel CredentialPackage { get; set; }
            public bool Selected { get; set; }
        }
    }
}