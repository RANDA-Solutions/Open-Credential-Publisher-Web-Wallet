using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class LinkListVM
    {
        public List<CredentialLinkVM> Credentials { get; set; }
    }

    public class CredentialLinkVM
    {
        [Required]
        public int ClrId { get; set; }
        public int CredentialPackageId { get; set; }
        public string ClrPublisherName { get; set; }
        public string ClrName { get; set; }

        public DateTime ClrIssuedOn { get; set; }

        public DateTime PackageCreatedAt { get; set; }

        public List<PdfShareViewModel> Pdfs { get; set; }
        public List<ShortLinkVM> Links { get; set; }
    }

    public class ShortLinkVM
    {
        public string Id { get; set; }
        [Required]
        public string Nickname { get; set; }
        public int DisplayCount { get; set; }
        
        public string Url { get; set; }
        
    }

    public class LinkVM
    {
        public string Id { get; set; }
        [Required]
        public string Nickname { get; set; }
        public int DisplayCount { get; set; }
        [Required]
        public int ClrId { get; set; }
        public string ClrPublisherName { get; set; }
        public DateTime ClrIssuedOn { get; set; }
        public string Url { get; set; }
        public DateTime PackageCreatedAt { get; set; }
        public List<PdfShareViewModel> Pdfs { get; set; }
    }


}
