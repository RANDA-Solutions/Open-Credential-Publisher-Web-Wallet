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

namespace OpenCredentialPublisher.ClrWallet.Pages.Clrs
{
    public class CreateModel : PageModel
    {
        private readonly WalletDbContext _context;

        public CreateModel(WalletDbContext context)
        {
            _context = context;
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
            foreach (var selectedClr in Clrs)
            {
                selectedClr.Clr = await _context.Clrs
                    .Include(x => x.Authorization)
                    .ThenInclude(x => x.Source)
                    .Where(x => x.Id == selectedClr.Clr.Id)
                    .SingleAsync();
            }

            if (Clrs.All(x => x.Selected == false))
            {
                ModelState.AddModelError(nameof(Clrs), "Please select at least one CLR.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newClr = new ClrDType
            {
                Assertions = new List<AssertionDType>(),
                Id = $"urn:uuid:{Guid.NewGuid():D}",
                IssuedOn = DateTime.Now.ToLocalTime(),
                Name = Name,
                SignedAssertions = new List<string>()
            };

            foreach (var selectedClr in Clrs)
            {
                if (selectedClr.Selected)
                {
                    var clr = JsonSerializer.Deserialize<ClrDType>(selectedClr.Clr.Json);

                    if (!string.IsNullOrEmpty(selectedClr.Clr.SignedClr))
                    {
                        clr = selectedClr.Clr.SignedClr.DeserializePayload<ClrDType>();
                    }

                    // Assume all the CLRs are for the same person

                    newClr.Learner = clr.Learner;
                    newClr.Publisher = clr.Learner;

                    foreach (var assertion in clr.Assertions ?? new List<AssertionDType>())
                    {
                        newClr.Assertions.Add(assertion);
                    }

                    foreach (var signedAssertion in clr.SignedAssertions ?? new List<string>())
                    {
                        newClr.SignedAssertions.Add(signedAssertion);
                    }
                }
            }

            var credentialPackage = new CredentialPackageModel()
            {
                UserId = User.UserId(),
                TypeId = PackageTypeEnum.Clr,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Clr = new ClrModel
                {
                    AssertionsCount = newClr.Assertions.Count + newClr.SignedAssertions.Count,
                    Identifier = newClr.Id,
                    IssuedOn = newClr.IssuedOn,
                    Json = JsonSerializer.Serialize(newClr),
                    LearnerName = newClr.Learner.Name,
                    Name = newClr.Name,
                    PublisherName = newClr.Publisher.Name,
                    RefreshedAt = newClr.IssuedOn
                }
            };
            
            await _context.CredentialPackages.AddAsync(credentialPackage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        public async Task<List<SelectedClr>> LoadClrs()
        {
            var packages = await _context.CredentialPackages
                .Include(cp => cp.VerifiableCredential)
                .ThenInclude(cp => cp.Clrs)
                .ThenInclude(clr => clr.Authorization)
                .ThenInclude(clra => clra.Source)
                .Include(cp => cp.Clr)
                .ThenInclude(c => c.Authorization)
                .ThenInclude(a => a.Source)
                .Where(package => package.UserId == User.UserId())
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var clrs = new List<SelectedClr>(
                packages.Where(p => p.TypeId == PackageTypeEnum.Clr).Select(p => new SelectedClr { Clr = p.Clr, Selected = false }));

            clrs.AddRange(packages.Where(p => p.TypeId == PackageTypeEnum.VerifiableCredential).SelectMany(p => p.VerifiableCredential.Clrs).Select(p => new SelectedClr { Clr = p, Selected = false }));

            return clrs;

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