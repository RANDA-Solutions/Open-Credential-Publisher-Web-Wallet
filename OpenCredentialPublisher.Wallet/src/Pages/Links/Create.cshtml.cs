using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class CreateModel : PageModel
    {
        private readonly CredentialService _credentialService;
        private readonly LinkService _linkService;

        public CreateModel(CredentialService credentialService, LinkService linkService)
        {
            _credentialService = credentialService;
            _linkService = linkService;
        }

        [BindProperty]
        public List<LinkViewModel> Links { get; set; }

        public async Task OnGet()
        {
            var clrs = _credentialService.GetAllClrs(User.UserId());

            Links = new List<LinkViewModel>();
            foreach (var clr in clrs.OrderByDescending(c => c.CredentialPackage.CreatedAt).ThenBy(c => c.Name))
            {
                var clrVm = ClrViewModel.FromClrModel(clr);
                Links.Add(new LinkViewModel
                {
                    ClrId = clrVm.Clr.Id,
                    AddedOn = clrVm.AncestorCredentialPackage.CreatedAt.ToLocalTime(),
                    CreatedAt = clrVm.Clr.IssuedOn,
                    Name = clrVm.Clr.Name,
                    Nickname = clrVm.Clr.Name,
                    SourceId = clrVm.Clr.Authorization?.Source?.Id,
                    SourceName = clrVm.Clr.Authorization?.Source?.Name,
                    PublisherName = clrVm.Clr.PublisherName
                });
            }
        }

        public async Task<IActionResult> OnPost([Required] int? clrId)
        {
            if (!ModelState.IsValid) return Page();

            var model = Links.Single(l => l.ClrId == clrId);
            if (model.Nickname == null)
            {
                var index = Links.FindIndex(l => l == model);
                ModelState.AddModelError($"{nameof(Links)}[{index}].{nameof(model.Nickname)}", "You must enter a nickname.");
                return Page();
            }

            var clr = await _credentialService.GetClrAsync(clrId.Value);

            var link = new LinkModel {ClrForeignKey = clr.Id, UserId = User.UserId(), Nickname = model.Nickname, CreatedAt = DateTimeOffset.UtcNow};

            await _linkService.AddAsync(link);            

            return RedirectToPage("./Index");
        }

        public class LinkViewModel
        {
            public int ClrId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime AddedOn { get; set; }
            public string Name { get; set; }
            public string Nickname { get; set; }
            public string PublisherName { get; set; }
            public int? SourceId { get; set; }
            public string SourceName { get; set; }
        }
    }
}