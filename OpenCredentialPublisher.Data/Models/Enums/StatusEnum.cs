namespace OpenCredentialPublisher.Data.Models.Enums
{
    public enum StatusEnum : int
    {
        Pending = 1,
        Accepted = 2,
        Used = 3,
        Expired = 4,
        Rejected = 5,
        Created = 6,
        Deleted = 7,
        Visible = 8,
        Hidden = 9,
        Submitted = 10,
        Active = 11,
        Sent = 12,
        Error = 13,
        WaitingForScoreReport = 14,
        ReadyForVerification = 15,
        Verified = 16,
        Unused = 17,
        Success = 18,
        NeedsEndorsement = 19
    }
}
