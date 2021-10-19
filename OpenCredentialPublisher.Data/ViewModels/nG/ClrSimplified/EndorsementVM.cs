using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class EndorsementVM
    {
        public string SignedEndorsement { get; set; }public int EndorsementId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsSigned { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime IssuedOn { get; set; }
        public string RevocationReason { get; set; }
        public bool? Revoked { get; set; }
        public Dictionary<string, object> AdditionalProperties { get; set; }
        public EndorsementClaimVM EndorsementClaim { get; set; }
        public ProfileVM Issuer { get; set; }
        public VerificationVM Verification { get; set; }

        public static EndorsementVM FromModel(EndorsementModel qq)
        {
            return new EndorsementVM
            {
                EndorsementClaim = EndorsementClaimVM.FromModel(qq.EndorsementClaim),
                EndorsementId = qq.EndorsementId,
                IsSigned = qq.IsSigned,
                IssuedOn = qq.IssuedOn,
                Issuer = ProfileVM.FromModel(qq.Issuer),
                RevocationReason = qq.RevocationReason,
                Revoked = qq.Revoked,
                SignedEndorsement = qq.SignedEndorsement,
                Verification = VerificationVM.FromModel(qq.Verification),
                AdditionalProperties = qq.AdditionalProperties,
                CreatedAt = qq.CreatedAt,
                Id = qq.Id,
                IsDeleted = qq.IsDeleted,
                ModifiedAt = qq.ModifiedAt,
                Type = qq.Type,
            };
        }
    }
}
