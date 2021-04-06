using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.Wallets;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Wallet.Pages.Wallets
{
    public class SendModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly CredentialService _credentialService;
        private readonly WalletRelationshipService _walletRelationshipService;
        private readonly ILogger<SendModel> _logger;

        public SendModel(IMediator mediator, CredentialService credentialService, WalletRelationshipService walletRelationshipService, ILogger<SendModel> logger)
        {
            _mediator = mediator;
            _credentialService = credentialService;
            _walletRelationshipService = walletRelationshipService;
            _logger = logger;
        }

        public WalletRelationshipModel Wallet { get; set; }
        public List<SendCredentialViewModel> Credentials { get; set; } = new List<SendCredentialViewModel>();

        public async Task OnGet(int id)
        {
            await OnLoad(id);
        }

        private async Task OnLoad(int id)
        {
            var wallet = await _walletRelationshipService.GetWalletRelationships(User.UserId()).Include(w => w.CredentialRequests).FirstOrDefaultAsync(w => w.Id == id);
            if (wallet == null)
            {
                ModelState.AddModelError("", "No wallet found for that Id.");
            }
            else
            {
                Wallet = wallet;
                var countDictionary = wallet.CredentialRequests.Where(cr => cr.CredentialRequestStep == CredentialRequestStepEnum.OfferAccepted).GroupBy(cr => cr.CredentialPackageId).ToDictionary(cr => cr.Key, cr => cr.Count());
                var credentialPackagesQuery = _credentialService.GetAllDeep(User.UserId()).Where(cp => !cp.Revoked);
                foreach (var cp in credentialPackagesQuery)
                {
                    var cpVM = CredentialPackageViewModel.FromCredentialPackageModel(cp);
                    var viewModel = new SendCredentialViewModel(cpVM);
                    viewModel.TimesSent = countDictionary.ContainsKey(viewModel.Id) ? countDictionary[viewModel.Id] : 0;
                    Credentials.Add(viewModel);
                }
                
                Credentials = Credentials.OrderByDescending(c => Convert.ToDateTime(c.DateAdded)).ToList();
            }
        }

        public async Task<IActionResult> OnPost(int id, int packageId)
        {
            try
            {
                if (await _credentialService.PackageExistsAsync(User.UserId(), packageId)
                    && await _walletRelationshipService.ExistsAsync(User.UserId(), id))
                {
                    await _mediator.Publish(new StartCredentialOfferCommand(User.UserId(), id, packageId));
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestResult();
            }
            return new OkResult();
        }
    }
}