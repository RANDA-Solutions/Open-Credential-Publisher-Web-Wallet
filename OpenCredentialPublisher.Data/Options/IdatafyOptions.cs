namespace OpenCredentialPublisher.Data.Options
{
    public class IdatafyOptions
    {
        public const string Section = "Idatafy";
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string DropFolder { get; set; }
        public int Port { get; set; }
        public string SmartResumeUrl { get; set; }
        public bool UseUserEmail { get; set; }
    }
}
