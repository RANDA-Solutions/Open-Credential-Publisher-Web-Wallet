using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.Enums;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class WalletCredentialVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TimesSent { get; set; }
        public string DateAdded { get; set; }

        public WalletCredentialVM(CredentialPackageModel credentialPackage)
        {
            Id = credentialPackage.Id;
            DateAdded = credentialPackage.CreatedAt.ToString("g");

            if (credentialPackage.TypeId == PackageTypeEnum.Clr)
            {
                Title = credentialPackage.ContainedClrs[0].Name;
            }
            else if (credentialPackage.TypeId == PackageTypeEnum.ClrSet)
            {
                Title = String.Join(" | ", credentialPackage.ContainedClrs.Select(c => c.Name));
            }
            else if (credentialPackage.TypeId == PackageTypeEnum.VerifiableCredential)
            {
                var verifiableCredential = credentialPackage.VerifiableCredential;
                if (!String.IsNullOrEmpty(verifiableCredential.Name))
                {
                    Title = verifiableCredential.Name;
                }
                else if (credentialPackage.ContainedClrs.Any())
                {
                    Title = String.Join(" | ", credentialPackage.ContainedClrs.Select(c => c.Name));
                }

            }
        }
    }
}
