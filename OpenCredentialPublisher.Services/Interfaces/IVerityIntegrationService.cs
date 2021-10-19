using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Shared.Interfaces;
using System.Threading.Tasks;
using VeritySDK.Utils;

namespace OpenCredentialPublisher.Services.Interfaces
{
    public interface IVerityIntegrationService
    {
        Task CreateCredentialDefinitionAsync(CredentialDefinition credentialDefinition);
        Task CreateProofRequestInvitationAsync(ProofRequest proofRequest);
        Task CreateRelationshipAsync(int requestId);
        Task CreateRelationshipAsync(string threadId);

        Task<AgentContextModel> GetAgentContextAsync();
        Task<Context> GetContextAsync(AgentContextModel agentContext = null);
        Task GetIssuerAsync(Context context);
        Task IssueCredentialAsync(CredentialRequestModel request, ICredential credential);
        Task ProcessMessageAsync(byte[] messageBytes);
        Task RegisterSchemaAsync(CredentialSchema credentialSchema);
        Task SendCredentialOfferAsync<T>(string userId, int walletRelationshipId, int credentialPackageId);
        Task StartIssuerSetupAsync();
        Task SetupIssuerAsync();
        Task UpdateConfigAsync(AgentContextModel agentContext = null);
        Task UpdateEndpointAsync();
    }
}