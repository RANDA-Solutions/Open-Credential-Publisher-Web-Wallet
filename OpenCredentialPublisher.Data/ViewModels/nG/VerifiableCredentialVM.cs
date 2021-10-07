using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class VerifiableCredentialVM
    {
        public int Id { get; set; }
        public string Identifier { get; set; }
        public string Issuer { get; set; }
        public DateTime IssuedOn { get; set; }
        public int CredentialsCount { get; set; }
        public Proof Proof { get; set; }
        public List<int> ClrIds { get; set; }

        public static VerifiableCredentialVM FromVC(VerifiableCredentialModel vc)
        {
            var vcDType = Newtonsoft.Json.JsonConvert.DeserializeObject<VerifiableCredential>(vc.Json);
            var clrIds = new List<int>();

            clrIds.AddRange(vc.ClrSets.SelectMany(cs => cs.Clrs).Select(c => c.ClrId));
            clrIds.AddRange(vc.Clrs.Select(c => c.ClrId));

            return new VerifiableCredentialVM
            {
                Id = vc.Id,
                Identifier = vc.Identifier,
                Issuer = vc.Issuer,
                IssuedOn = vc.IssuedOn,
                CredentialsCount = vc.CredentialsCount,
                Proof = vcDType.Proof,
                ClrIds = clrIds
            };
        }
    }
}
