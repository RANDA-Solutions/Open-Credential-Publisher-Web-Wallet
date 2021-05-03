using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Badgr;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    public class CredentialPackageViewModel 
    {
        public CredentialPackageModel CredentialPackage { get; set; }
        [NotMapped]
        public ClrViewModel ClrVM { get; set; }
        [NotMapped]
        public ClrSetViewModel ClrSetVM { get; set; }
        [NotMapped]
        public VerifiableCredentialViewModel VerifiableCredentialVM { get; set; }
        [NotMapped]
        public int AssertionsCount { get; set; }
        [NotMapped]
        public BadgrBackpackModel Backpack { get; set; }
        [JsonIgnore]
        [NotMapped]
        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();

        [NotMapped, JsonIgnore]
        public bool HasPdfs => Pdfs.Any(e => e.IsPdf);

        public static CredentialPackageViewModel FromCredentialPackageModel(CredentialPackageModel pkg)
        {
            var pkgVM = new CredentialPackageViewModel() { CredentialPackage = pkg};
            pkgVM.Pdfs = new List<PdfShareViewModel>();
            if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.Clr)
            {
                var clrVM = ClrViewModel.FromClrModel(pkg.Clr);
                pkgVM.ClrVM = clrVM;
                pkgVM.AssertionsCount = clrVM.AllAssertions.Count;
                pkgVM.Pdfs.AddRange(clrVM.Pdfs);
            }
            else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.ClrSet)
            {
                var clrSetVM = ClrSetViewModel.FromClrSetModel(pkg.ClrSet);
                pkgVM.ClrSetVM = clrSetVM;
                pkgVM.AssertionsCount = clrSetVM?.AssertionsCount ?? 0;
                pkgVM.Pdfs.AddRange(clrSetVM?.Pdfs);
            }
            else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.VerifiableCredential)
            {
                var vcVM = VerifiableCredentialViewModel.FromVerifiableCredentialModel(pkg.VerifiableCredential);
                pkgVM.VerifiableCredentialVM = vcVM;
                pkgVM.AssertionsCount = vcVM.AssertionsCount;
                pkgVM.Pdfs.AddRange(vcVM.Pdfs);
            }
            else if (pkgVM.CredentialPackage.TypeId == PackageTypeEnum.OpenBadge)
            {
                var vcVM = ClrViewModel.FromBackpack(pkg.BadgrBackpack);
                pkgVM.ClrVM = vcVM;
                pkgVM.AssertionsCount = vcVM.AllAssertions.Count;
                pkgVM.Pdfs.AddRange(vcVM.Pdfs);
            }
            return pkgVM;
        }
    }
}
