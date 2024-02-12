using Azure.Messaging.EventGrid;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Services.SignalR;
using OpenCredentialPublisher.Shared.Commands;
using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Services.Implementations
{
    //public class EventHandlerService
    //{
    //    private readonly ILogger<EventHandlerService> _logger;
    //    private readonly IHubContext<ConnectionStatusHub> _connectionStatusHub;
    //    private readonly IHubContext<CredentialStatusHub> _credentialStatusHub;
    //    private readonly IHubContext<ProofRequestStatusHub> _proofRequestStatusHub;
    //    private readonly EmailService _emailService;
    //    private readonly SiteSettingsOptions _siteSettings;
    //    private readonly IServiceScopeFactory _scopeFactory;

    //    public EventHandlerService(IServiceScopeFactory scopeFactory, IOptions<SiteSettingsOptions> siteSettings, EmailService emailService, ILogger<EventHandlerService> logger, IHubContext<ConnectionStatusHub> connectionStatusHub, IHubContext<CredentialStatusHub> credentialStatusHub, IHubContext<ProofRequestStatusHub> proofRequestStatusHub)
    //    {
    //        _logger = logger;
    //        _connectionStatusHub = connectionStatusHub;
    //        _credentialStatusHub = credentialStatusHub;
    //        _proofRequestStatusHub = proofRequestStatusHub;
    //        _emailService = emailService;
    //        _siteSettings = siteSettings.Value;
    //        _scopeFactory = scopeFactory;
    //    }

    //    public async Task HandlerAsync(EventGridEvent eventGridEvent)
    //    {
    //        _logger.LogInformation(eventGridEvent.Data.ToString(), eventGridEvent);
    //        dynamic notification = eventGridEvent.EventType switch
    //        {
    //            InvitationGeneratedNotification.MessageType => eventGridEvent.Data.ToObjectFromJson<InvitationGeneratedNotification>(),
    //            ConnectionStatusNotification.MessageType => eventGridEvent.Data.ToObjectFromJson<ConnectionStatusNotification>(),
    //            CredentialStatusNotification.MessageType => eventGridEvent.Data.ToObjectFromJson<CredentialStatusNotification>(),
    //            RequestProofInvitationNotification.MessageType => eventGridEvent.Data.ToObjectFromJson<RequestProofInvitationNotification>(),
    //            CredentialDefinitionNeedsEndorsementNotification.MessageType => eventGridEvent.Data.ToObjectFromJson<CredentialDefinitionNeedsEndorsementNotification>(),
    //            SchemaNeedsEndorsementNotification.MessageType => eventGridEvent.Data.ToObjectFromJson<SchemaNeedsEndorsementNotification>(),
    //            _ => throw new NotImplementedException(eventGridEvent.EventType)
    //        };
    //        await HandlerAsync(notification);
    //    }

    //    public async Task HandlerAsync(string messageType, string message)
    //    {
    //        if (String.IsNullOrEmpty(message))
    //            return;
    //        _logger.LogInformation(message, messageType);
    //        var options = new JsonSerializerOptions
    //        {
    //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //        };
            
    //        dynamic notification = messageType switch
    //        {
    //            InvitationGeneratedNotification.MessageType => JsonSerializer.Deserialize<InvitationGeneratedNotification>(message, options),
    //            ConnectionStatusNotification.MessageType => JsonSerializer.Deserialize<ConnectionStatusNotification>(message, options),
    //            CredentialStatusNotification.MessageType => JsonSerializer.Deserialize<CredentialStatusNotification>(message, options),
    //            RequestProofInvitationNotification.MessageType => JsonSerializer.Deserialize<RequestProofInvitationNotification>(message, options),
    //            CredentialDefinitionNeedsEndorsementNotification.MessageType => JsonSerializer.Deserialize<CredentialDefinitionNeedsEndorsementNotification>(message, options),
    //            SchemaNeedsEndorsementNotification.MessageType => JsonSerializer.Deserialize<SchemaNeedsEndorsementNotification>(message, options),
    //            _ => throw new NotImplementedException(messageType)
    //        };
    //        await HandlerAsync(notification);
    //    }

    //    private async Task HandlerAsync(InvitationGeneratedNotification notification)
    //    {
    //        Debug.WriteLine($"MyDebug {ConnectionStatusHub.InvitationStatus} id: {notification.WalletRelationshipId} status: {Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep)} done: {((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationGenerated}");

    //        await _connectionStatusHub.Clients.Group(notification.UserId).SendAsync(ConnectionStatusHub.InvitationStatus, notification.WalletRelationshipId, Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep), ((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationGenerated);
    //    }

    //    private async Task HandlerAsync(ConnectionStatusNotification notification)
    //    {
    //        Debug.WriteLine($"MyDebug {ConnectionStatusHub.ConnectionStatus} id: {notification.WalletRelationshipId} status: {Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep)} done: {((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationCompleted}");

    //        await _connectionStatusHub.Clients.Group(notification.UserId).SendAsync(ConnectionStatusHub.ConnectionStatus, notification.WalletRelationshipId, Enum.GetName(typeof(ConnectionRequestStepEnum), notification.ConnectionStep), ((ConnectionRequestStepEnum)notification.ConnectionStep) == ConnectionRequestStepEnum.InvitationCompleted);
    //    }

    //    private async Task HandlerAsync(CredentialStatusNotification notification)
    //    {
    //        var step = (CredentialRequestStepEnum)notification.CredentialRequestStep;
    //        (bool done, bool error, bool revoked) state = step switch
    //        {
    //            CredentialRequestStepEnum.CredentialIsRevoked => (true, false, true),
    //            CredentialRequestStepEnum.OfferSent => (true, false, false),
    //            CredentialRequestStepEnum.OfferAccepted => (true, false, false),
    //            CredentialRequestStepEnum.Error => (true, true, false),
    //            CredentialRequestStepEnum.ErrorWritingSchema => (true, true, false),
    //            CredentialRequestStepEnum.ErrorWritingCredentialDefinition => (true, true, false),
    //            _ => (false, false, false)
    //        };
    //        Debug.WriteLine($"MyDebug {CredentialStatusHub.CredentialStatus} id: {notification.WalletRelationshipId} pkgId: {notification.CredentialPackageId} status: {Enum.GetName(typeof(CredentialRequestStepEnum), notification.CredentialRequestStep)} done: {state.done} error: {state.error} revoked: {state.revoked}");
    //        await _credentialStatusHub.Clients.Group(notification.UserId).SendAsync(CredentialStatusHub.CredentialStatus, notification.WalletRelationshipId, notification.CredentialPackageId, Enum.GetName(typeof(CredentialRequestStepEnum), notification.CredentialRequestStep), state.done, state.error, state.revoked);
    //    }

    //    private async Task HandlerAsync(RequestProofInvitationNotification notification)
    //    {
    //        var step = Enum.Parse<ProofRequestStepEnum>(notification.Status);
    //        var finished = step == ProofRequestStepEnum.ProofReceived;
    //        await _proofRequestStatusHub.Clients.Group(notification.Id.ToLower()).SendAsync(ProofRequestStatusHub.ProofRequestStatus, notification.Status, finished);

    //        try
    //        {
    //            using var scope = _scopeFactory.CreateScope();
    //            var proofService = scope.ServiceProvider.GetRequiredService<ProofService>();
    //            var proof = await proofService.GetProofRequestAsync(notification.Id);
    //            if (proof == null)
    //                return;

    //            if (step != ProofRequestStepEnum.ProofReceived)
    //                return;

    //            var emailHelperService = scope.ServiceProvider.GetRequiredService<EmailHelperService>();
    //            switch (step)
    //            {
    //                case ProofRequestStepEnum.ProofReceived:
    //                    var proofMessage = new MessageModel
    //                    {
    //                        Body = new StringBuilder($"You've received a response to your proof request ({proof.Name})!  Please use the link below to access it.<br />")
    //                            .Append($"<a href=\"{_siteSettings.SpaClientUrl}/verifier/{proof.PublicId.ToLower()}\">{_siteSettings.SpaClientUrl}</a>").ToString(),
    //                        Recipient = proof.NotificationAddress,
    //                        Subject = $"Response Received to Proof Request - {proof.Name} ({proof.PublicId})",
    //                        SendAttempts = 0,
    //                        StatusId = StatusEnum.Created,
    //                        CreatedAt = DateTime.UtcNow,
    //                        ProofRequestId = proof.Id
    //                    };
    //                    await emailHelperService.AddMessageAsync(proofMessage);
    //                    await _emailService.SendEmailAsync(proofMessage.Recipient, proofMessage.Subject, proofMessage.Body, true);

    //                    break;

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, ex.Message, notification);
    //        }
    //    }

    //    private async Task HandlerAsync(CredentialDefinitionNeedsEndorsementNotification notification)
    //    {
    //        try
    //        {
    //            using var scope = _scopeFactory.CreateScope();
    //            var emailHelperService = scope.ServiceProvider.GetRequiredService<EmailHelperService>();
    //            var defMessage = new MessageModel
    //            {
    //                Body = new StringBuilder($"A credential definition requires endorsement by Evernym.  Please send the following values to support@evernym.com<br />")
    //                            .Append($"IssuerDID: {notification.IssuerDid}<br />")
    //                            .Append($"Verkey: {notification.IssuerVerkey}<br />")
    //                            .Append($"CredDefId: {notification.CredentialDefinitionId}<br />")
    //                            .Append($"CredDefJson: {notification.CredDefJson}<br />")
    //                            .Append($"ThreadId: {notification.ThreadId}<br />").ToString(),
    //                Recipient = _siteSettings.AdminEmailAddress,
    //                Subject = $"Credential Definition Endorsement Required",
    //                SendAttempts = 0,
    //                StatusId = StatusEnum.Created,
    //                CreatedAt = DateTime.UtcNow
    //            };
    //            await emailHelperService.AddMessageAsync(defMessage);
    //            await _emailService.SendEmailAsync(defMessage.Recipient, defMessage.Subject, defMessage.Body, true);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, ex.Message, notification);
    //        }
    //    }

    //    private async Task HandlerAsync(SchemaNeedsEndorsementNotification notification)
    //    {
    //        try
    //        {
    //            using var scope = _scopeFactory.CreateScope();
    //            var emailHelperService = scope.ServiceProvider.GetRequiredService<EmailHelperService>();
    //            var schemaMessage = new MessageModel
    //            {
    //                Body = new StringBuilder($"A schema requires endorsement by Evernym.  Please send the following values to support@evernym.com<br />")
    //                            .Append($"IssuerDID: {notification.IssuerDid}<br />")
    //                            .Append($"Verkey: {notification.IssuerVerkey}<br />")
    //                            .Append($"SchemaId: {notification.SchemaId}<br />")
    //                            .Append($"SchemaJson: {notification.SchemaJson}<br />")
    //                            .Append($"ThreadId: {notification.ThreadId}<br />").ToString(),
    //                Recipient = _siteSettings.AdminEmailAddress,
    //                Subject = $"Schema Endorsement Required",
    //                SendAttempts = 0,
    //                StatusId = StatusEnum.Created,
    //                CreatedAt = DateTime.UtcNow
    //            };
    //            await emailHelperService.AddMessageAsync(schemaMessage);
    //            await _emailService.SendEmailAsync(schemaMessage.Recipient, schemaMessage.Subject, schemaMessage.Body, true);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, ex.Message, notification);
    //        }
    //    }
    //}
}
