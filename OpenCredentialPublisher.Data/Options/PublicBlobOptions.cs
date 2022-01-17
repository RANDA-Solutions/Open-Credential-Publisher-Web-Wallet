namespace OpenCredentialPublisher.Data.Options
{
    public class PublicBlobOptions
    {
        public const string Section = "PublicBlob";

        public string StorageConnectionString { get; set; }
        public string CustomDomainName { get; set; }
    }
}
