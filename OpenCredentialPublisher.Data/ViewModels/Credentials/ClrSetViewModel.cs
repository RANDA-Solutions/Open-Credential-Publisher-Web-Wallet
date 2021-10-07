using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.ViewModels.nG;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class ClrSetViewModel
    {
        public ClrSetModel ClrSet { get; set; }
        public List<ClrViewModel> ClrVMs { get; set; }

        [JsonIgnore]
        [NotMapped]
        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();
        [NotMapped, JsonIgnore]
        public bool HasPdfs => Pdfs.Any(e => e.IsPdf);

        [NotMapped]
        public int AssertionsCount { get; set; }

        public ClrSetViewModel()
        {
            ClrVMs = new List<ClrViewModel>();
        }
        public static ClrSetViewModel FromClrSetModel(ClrSetModel clrSet)
        {
            var clrSetVM = new ClrSetViewModel() {ClrSet = clrSet, AssertionsCount = 0 };

            clrSetVM.ClrVMs = new List<ClrViewModel>();

            foreach (var clr in clrSet.Clrs)
            {
                var clrVM = ClrViewModel.FromClrModel(clr);
                clrSetVM.AssertionsCount += clrVM.AllAssertions.Count;
                clrSetVM.ClrVMs.Add(clrVM);
                clrSetVM.Pdfs.AddRange(clrVM.Pdfs);
            }
            return clrSetVM;
        }
    }
}
