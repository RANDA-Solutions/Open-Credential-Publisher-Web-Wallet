using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class EndorsementClaimVM
    {
        public int EndorsementClaimId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string EndorsementComment { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }


        public static EndorsementClaimVM FromModel(EndorsementClaimModel qq)
        {
            return new EndorsementClaimVM
            {
                EndorsementClaimId = qq.EndorsementClaimId,
                EndorsementComment = qq.EndorsementComment,              
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
