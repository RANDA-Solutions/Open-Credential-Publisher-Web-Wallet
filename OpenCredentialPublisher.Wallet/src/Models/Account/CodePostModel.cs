namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class CodePostModel
    {
        public string Email { get; set; }
    }

    public class CodeResponseModel
    {
        public bool Invalid { get; set; }
        public bool Expired { get; set; }
        public bool Success { get; set; }
    }
}
