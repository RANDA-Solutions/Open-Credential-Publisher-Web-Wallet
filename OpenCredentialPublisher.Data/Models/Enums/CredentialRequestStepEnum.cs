namespace OpenCredentialPublisher.Data.Models.Enums
{
    public enum CredentialRequestStepEnum
    {
        Initiated = 0, PendingAgent = 1, PendingSchema = 2, PendingCredentialDefinition = 3, ReadyToSend = 4, SendingOffer = 5, OfferSent = 6, OfferAccepted = 7, CheckingRevocationStatus = 8, CredentialIsRevoked = 9, CredentialIsStillValid = 10, Error = 13, ErrorWritingSchema = 14, ErrorWritingCredentialDefinition = 15
    }
}
