using OpenCredentialPublisher.ClrLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.Models
{
    /// <summary>
    /// Represents a CLR for an application user. The complete CLR is stored as JSON.
    /// </summary>
    public class VerifiableCredentialModel
    {
        public int CredentialPackageId { get; set; }

        /// <summary>
        /// Number of credentials in this VC.
        /// </summary>
        public int CredentialsCount { get; set; }

        /// <summary>
        /// DateTime when this VC was issued.
        /// </summary>
        public DateTime IssuedOn { get; set; }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The VC @id.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Complete JSON of the VC.
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Issuer of the VC.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Optional name of VC. Primarily for self-published VCs.
        /// </summary>
        public string Name { get; set; }

        public bool Revoked { get; set; }
        public string RevocationReason { get; set; }


        public CredentialPackageModel CredentialPackage { get; set; }
        public List<ClrSetModel> ClrSets { get; set; }
        public List<ClrModel> Clrs { get; set; }

            //public void BuildViews()
            //{
            //    VerifiableCredential = Newtonsoft.Json.JsonConvert.DeserializeObject<VerifiableCredential>(Json);
            //    foreach (var clrSet in ClrSets)
            //    {
            //        foreach (var clr in clrSet.Clrs)
            //        {
            //            clr.EnhanceClr();
            //            AssertionsCount += clr.AllAssertions.Count;
            //            Pdfs.AddRange(clr.Pdfs);
            //            AllClrs.Add(clr);
            //        }
            //    }

            //    foreach (var clr in Clrs)
            //    {
            //        clr.EnhanceClr();
            //        AssertionsCount += clr.AllAssertions.Count;
            //        Pdfs.AddRange(clr.Pdfs);
            //        AllClrs.Add(clr);
            //    }
            //}
        }
}
