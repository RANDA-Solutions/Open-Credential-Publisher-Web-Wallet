using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenCredentialPublisher.Data.ViewModels.Wallets
{
    public class SendCredentialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public String DateAdded { get; set; }
        public int TimesSent { get; set; }
        public bool HasPdfTranscript { get; set; }

        public SendCredentialViewModel() { }
        public SendCredentialViewModel(CredentialPackageViewModel credentialPackageViewModel)
        {
            var credentialPackage = credentialPackageViewModel.CredentialPackage;
            Id = credentialPackage.Id;
            DateAdded = credentialPackage.CreatedAt.ToString("g");

            if (credentialPackage.TypeId == PackageTypeEnum.Clr)
            {
                var clr = credentialPackageViewModel.ClrVM.RawClrDType;
                
                Title = $"{clr.Name}";
            }
            else if (credentialPackage.TypeId == PackageTypeEnum.ClrSet)
            {
                var clrSet = credentialPackageViewModel.ClrSetVM;
                Title = String.Join(" | ", clrSet.ClrVMs.Select(c => c.RawClrDType.Name));
            }
            else if (credentialPackage.TypeId == PackageTypeEnum.VerifiableCredential)
            {
                var verifiableCredential = credentialPackageViewModel.VerifiableCredentialVM;
                if (!String.IsNullOrEmpty(verifiableCredential.Name))
                {
                    Title = verifiableCredential.Name;
                }
                else if (verifiableCredential.AllClrs.Any())
                {
                    Title = String.Join(" | ", verifiableCredential.AllClrs.Select(c => $"{c.RawClrDType.Name}"));
                }
                
            }
        }
    }
}
