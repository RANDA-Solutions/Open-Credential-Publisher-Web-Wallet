using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class CredentialPackageModel
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
        public PackageTypeEnum TypeId { get; set; }

        public bool Revoked { get; set; }
        public string RevocationReason { get; set; }

        public ClrModel Clr { get; set; }
        public ClrSetModel ClrSet { get; set; }
        public VerifiableCredentialModel VerifiableCredential { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public String Name { get; set; }

        /// <summary>
        /// This Package is tied to a specific application user.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [NotMapped]
        public ClrViewModel ClrViewModel { get; set; }

        [NotMapped]
        public int AssertionsCount { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();

        [NotMapped]
        public bool HasPdfs => Pdfs.Any();

        public void BuildView()
        {
            if (TypeId == PackageTypeEnum.Clr)
            {
                ClrViewModel = JsonSerializer.Deserialize<ClrViewModel>(Clr.Json);
                ClrViewModel.ClrId = Clr.Id;
                ClrViewModel.BuildAssertionsTree();
                AssertionsCount = ClrViewModel.AllAssertions.Count;
                Pdfs.AddRange(ClrViewModel.Pdfs);
            }
            else if (TypeId == PackageTypeEnum.ClrSet)
            {
                ClrSet?.BuildViews();
                AssertionsCount = ClrSet?.AssertionsCount ?? 0;
                Pdfs.AddRange(ClrSet?.Pdfs);
            }
            else if (TypeId == PackageTypeEnum.VerifiableCredential)
            {
                VerifiableCredential.BuildViews();
                AssertionsCount = VerifiableCredential.AssertionsCount;
                Pdfs.AddRange(VerifiableCredential.Pdfs);
            }
        }

    }
}
