using OpenCredentialPublisher.ClrLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{
    /// <summary>
    /// Represents a CLR for an application user. The complete CLR is stored as JSON.
    /// </summary>
    public class VerifiableCredentialViewModel : VerifiableCredentialModel
    {
        public CredentialPackageViewModel CredentialPackageVM { get; set; }
        public List<ClrSetViewModel> ClrSetVMs { get; set; }
        public List<ClrViewModel> ClrVMs { get; set; }
        [JsonIgnore]
        [NotMapped]
        public List<ClrViewModel> AllClrs { get; set; } = new List<ClrViewModel>();
        [JsonIgnore]
        [NotMapped]
        public VerifiableCredential VerifiableCredential { get; set; }

        [JsonIgnore]
        [NotMapped]
        public int AssertionsCount { get; set; }
        [JsonIgnore]
        [NotMapped]
        public List<PdfShareViewModel> Pdfs { get; set; } = new List<PdfShareViewModel>();

        public static VerifiableCredentialViewModel FromVerifiableCredentialModel(VerifiableCredentialModel vc)
        {
            var vcVM = new VerifiableCredentialViewModel();
            //vcVM.CredentialPackageVM = CredentialPackageViewModel.FromCredentialPackageModel(vc.CredentialPackage);
            vcVM.CredentialPackageId = vc.CredentialPackageId;
            vcVM.Identifier = vc.Identifier;
            vcVM.Issuer = vc.Issuer;
            vcVM.IssuedOn = vc.IssuedOn;
            vcVM.CredentialsCount = vc.CredentialsCount;
            vcVM.VerifiableCredential = Newtonsoft.Json.JsonConvert.DeserializeObject<VerifiableCredential>(vc.Json);
            vcVM.ClrSetVMs = new List<ClrSetViewModel>();
            vcVM.ClrVMs = new List<ClrViewModel>();
            foreach (var clrSet in vc.ClrSets)
            {
                var csVM = ClrSetViewModel.FromClrSetModel(clrSet);
                vcVM.ClrSetVMs.Add(csVM);
                vcVM.AssertionsCount += csVM.AssertionsCount;
                vcVM.Pdfs.AddRange(csVM.Pdfs);
                vcVM.AllClrs.AddRange(csVM.ClrVMs);
            }

            foreach (var clr in vc.Clrs)
            {
                var clrVMc = ClrViewModel.FromClrModel(clr);
                vcVM.AssertionsCount += clrVMc.AllAssertions.Count;
                vcVM.Pdfs.AddRange(clrVMc.Pdfs);
                vcVM.AllClrs.Add(clrVMc);
            }
            return vcVM;
        }
    }
}
