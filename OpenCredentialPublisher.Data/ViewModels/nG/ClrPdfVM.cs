namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ClrPdfVM
    {
        public string AssertionId { get; set; }
        public int ArtifactId { get; set; }
        public string EvidenceName { get; set; }
        public string ArtifactName { get; set; }

        public string ArtifactUrl { get; set; }
        public bool IsUrl { get; set; }
        public bool IsPdf { get; set; }
        public string MediaType { get; set; }
    }

    
}