namespace OpenCredentialPublisher.Data.Models
{
    public enum ConnectionRequestStepEnum
    {
        Initiated = 0, PendingAgent = 1, StartingInvitation = 2, RequestingInvitation = 3, InvitationGenerated = 4, InvitationAccepted = 5, InvitationCompleted = 6, Error = 13
    }
}
