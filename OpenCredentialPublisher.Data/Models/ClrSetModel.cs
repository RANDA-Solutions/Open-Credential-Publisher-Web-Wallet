using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class ClrSetModel
    {
        public int Id { get; set; }

        public int? CredentialPackageId { get; set; }
        public int? VerifiableCredentialId { get; set; }
        public int ClrsCount { get; set; }
        public string Identifier { get; set; }
        public string Json { get; set; }

        public CredentialPackageModel CredentialPackage { get; set; }
        public VerifiableCredentialModel VerifiableCredential { get; set; }
        public List<ClrModel> Clrs { get; set; }

        [NotMapped]
        public List<ClrViewModel> ClrViewModels 
        {
            get; set;
        }

        [JsonIgnore]
        [NotMapped]
        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();

        [NotMapped]
        public int AssertionsCount { get; set; }


        public void BuildViews()
        {
            foreach (var clr in Clrs)
            {
                var viewModel = JsonSerializer.Deserialize<ClrViewModel>(clr.Json);
                viewModel.ClrId = clr.Id;
                viewModel.BuildAssertionsTree();
                AssertionsCount += viewModel.AllAssertions.Count;
                Pdfs.AddRange(viewModel.Pdfs);
                ClrViewModels.Add(viewModel);
            }
        }
    }
}
