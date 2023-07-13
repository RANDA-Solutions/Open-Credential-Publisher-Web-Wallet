namespace OpenCredentialPublisher.Wallet.Models.Account
{
    public class CodeVerifyModel
    {
        public string State { get; set; }
    }

    public class CodeVerifyResponseModel
    {
        public bool Expired { get; set; }
        public bool Invalid { get; set; }
        public bool Claimed { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class CodePostResponseModel
    {
        public bool Created { get; set; }
        public bool Invalid { get; set; }
        public bool Locked { get; set; }
    }
}
