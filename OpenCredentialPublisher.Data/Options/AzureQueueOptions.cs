namespace OpenCredentialPublisher.Data.Options
{
    public class AzureQueueOptions
    {
        public const string Section = "AzureQueue";

        public string StorageConnectionString { get; set; }
    }
}
