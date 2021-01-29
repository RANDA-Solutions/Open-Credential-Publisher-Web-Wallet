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

namespace OpenCredentialPublisher.ClrWallet.Pages.Links
{
    public class CreateModel : PageModel
    {
        private readonly WalletDbContext _context;

        public CreateModel(WalletDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<LinkViewModel> Links { get; set; }

        public async Task OnGet()
        {
            var clrs = _context.Clrs
                .Include(clr => clr.CredentialPackage)
                .Include(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(c => c.CredentialPackage.UserId == User.UserId());

            Links = new List<LinkViewModel>();

            foreach (var clr in clrs.OrderBy(c => c.IssuedOn).ThenBy(c => c.Name))
            {
                Links.Add(new LinkViewModel
                {
                    ClrId = clr.Id,
                    AddedOn = clr.CredentialPackage.CreatedAt.ToLocalTime(),
                    CreatedAt = clr.IssuedOn,
                    Name = clr.Name,
                    Nickname = clr.Name,
                    SourceId =  clr.Authorization?.Source?.Id,
                    SourceName = clr.Authorization?.Source?.Name,
                    PublisherName = clr.PublisherName
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

            var clr = await _context.Clrs.SingleAsync(c => c.Id == clrId);

            var link = new LinkModel {Clr = clr, UserId = User.UserId(), Nickname = model.Nickname, CreatedAt = DateTimeOffset.UtcNow};

            await _context.Links.AddAsync(link);
            await _context.SaveChangesAsync();

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