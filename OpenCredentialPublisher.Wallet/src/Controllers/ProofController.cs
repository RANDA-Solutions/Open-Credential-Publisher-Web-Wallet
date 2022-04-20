using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Dtos.ProofRequest;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Shared.Commands;
using OpenCredentialPublisher.VerityRestApi.Model;
using OpenCredentialPublisher.Wallet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace OpenCredentialPublisher.Wallet.Controllers
{
    public class ProofController : ApiController<ProofController>
    {
        private readonly ProofService _proofService;
        private readonly IMediator _mediator;
        private readonly EmailService _emailService;
        private readonly EmailHelperService _emailHelperService;
        private readonly AzureBlobStoreService _azureBlobStoreService;
        private readonly SiteSettingsOptions _siteSettings;

        public ProofController(IOptions<SiteSettingsOptions> siteSettingsOptions, EmailService emailService, EmailHelperService emailHelperService,
            AzureBlobStoreService azureBlobStoreService,
            ProofService proofService, IMediator mediator, ILogger<ProofController> logger) : base(logger)
        {
            _proofService = proofService;
            _mediator = mediator;
            _emailHelperService = emailHelperService;
            _emailService = emailService;
            _azureBlobStoreService = azureBlobStoreService;
            _siteSettings = siteSettingsOptions.Value;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetPageModel()
        {
            var model = await _proofService.GetProofRequestPageModel();
            return ApiOk(model);
        }

        [HttpGet, Route("request/{id}")]
        public async Task<ProofRequestInformationModel> GetInformationPageModel(string id)
        {
            var model = await _proofService.GetProofRequestInformationModelAsync(id);
            return model;
        }


        [HttpPost, Route("request")]
        public async Task<IActionResult> CreateRequestAsync([FromBody]CreateProofRequest createProofRequest)
        {
            try
            {
                var request = await _proofService.CreateProofRequestAsync(createProofRequest.CredentialSchemaId, createProofRequest.NotificationAddress, createProofRequest.Name, null, userId: UserId);
                await _mediator.Publish(new RequestProofInvitationCommand(request.Id));

                if (!String.IsNullOrEmpty(request.NotificationAddress))
                {
                    var createdMessage = new MessageModel
                    {
                        Body = new StringBuilder($"You created a proof request titled: {request.Name}.  Please use the link below to access it.<br />")
                                    .Append($"<a href=\"{_siteSettings.SpaClientUrl}/verifier/{request.PublicId.ToLower()}\">{_siteSettings.SpaClientUrl}</a>").ToString(),
                        Recipient = request.NotificationAddress,
                        Subject = $"Proof Request - {request.Name} - Created ({request.PublicId})",
                        SendAttempts = 0,
                        StatusId = StatusEnum.Created,
                        CreatedAt = DateTime.UtcNow,
                        ProofRequestId = request.Id
                    };
                    await _emailHelperService.AddMessageAsync(createdMessage);
                    await _emailService.SendEmailAsync(createdMessage.Recipient, createdMessage.Subject, createdMessage.Body, true);
                }
                return ApiOk(new CreateProofResponse { Id = request.PublicId });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message, createProofRequest);
                return ApiOk(new CreateProofResponse { ErrorMessages = new List<string> { ex.Message } });
            }
        }

        [HttpGet, Route("{id}")]
        public async Task<ProofResponsePageModel> GetProofAsync(string id)
        {
            try
            {
                var response = await _proofService.GetProofResponsePageModelAsync(id);
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);
                throw;
            }
        }

        [HttpGet, Route("invitation/{id}")]
        public async Task<Invitation> GetInvitationAsync(string id)
        {
            var request = await _proofService.GetProofRequestAsync(id);
            var invitationUrl = new Uri(request.InvitationLink);
            var queryString = HttpUtility.ParseQueryString(invitationUrl.Query);
            var payload = queryString["oob"];

            string imageUrl = await StorageUtility.StorageAccountToDataUrl(request.InvitationQrCode, _azureBlobStoreService, _siteSettings);

            var invitation = new Invitation
            {
                InvitationId = request.InvitationId,
                InvitationLink = request.InvitationLink,
                ShortInvitationLink = request.ShortInvitationLink,
                QrCodeUrl = imageUrl,
                CredentialName = request.CredentialSchema.Name,
                Payload = payload
            };
            return invitation;
        }

        [HttpGet, Route("status/{id}")]
        public async Task<string> GetRequestStatusAsync(string id)
        {
            try
            {
                var request = await _proofService.GetProofRequestAsync(id);
                return request.StepId.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);
                return "Bad Request";
            }
        }
    }
}
