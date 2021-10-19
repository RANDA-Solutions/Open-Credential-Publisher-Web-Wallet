using OpenCredentialPublisher.Data.Models.Enums;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class PdfRequest
    {
        public PdfRequestTypeEnum RequestType { get; set; }
        public string LinkId{get; set;}
        public int? ClrId{get; set;}
        public string AssertionId {get; set;}
        public string EvidenceName {get; set;}
        public int? ArtifactId {get; set;}
        public string ArtifactName {get; set;}
        public bool CreateLink { get; set; }
    }
}
