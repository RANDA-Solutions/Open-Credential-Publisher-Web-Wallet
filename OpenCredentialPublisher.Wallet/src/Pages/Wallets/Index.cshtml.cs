using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Pages.Wallets
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly WalletRelationshipService _walletRelationshipService;

        public IndexModel(IMediator mediator, WalletRelationshipService walletRelationshipService)
        {
            _mediator = mediator;
            _walletRelationshipService = walletRelationshipService;
        }

        public List<WalletRelationshipModel> Relationships { get; set; }

        public async Task OnGet()
        {
            var relationships = _walletRelationshipService.GetWalletRelationships(User.UserId()).Include(wr => wr.CredentialRequests);
            Relationships = await relationships.OrderByDescending(o => o.CreatedOn).ToListAsync(); ;
        }

        public async Task<IActionResult> OnPost()
        {
            await _mediator.Publish(new GenerateInvitationCommand(User.UserId()));
            return new OkResult();
        }
    }
}