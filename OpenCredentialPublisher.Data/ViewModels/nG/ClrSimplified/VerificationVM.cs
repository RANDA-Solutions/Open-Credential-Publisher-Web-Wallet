using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenCredentialPublisher.ClrLibrary.Models.VerificationDType;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class VerificationVM
    {
        public int VerificationId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public TypeEnum Type { get; set; }
        public List<string> AllowedOrigins { get; set; }
        public string Creator { get; set; }
        public List<string> StartsWith { get; set; }
        public string VerificationProperty { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        public static VerificationVM FromModel(VerificationModel qq)
        {
            if (qq == null)
            {
                return null;
            }
            return new VerificationVM
            {
                AllowedOrigins = qq.AllowedOrigins,
                Creator = qq.Creator,
                StartsWith = qq.StartsWith,
                VerificationId = qq.VerificationId,
                VerificationProperty = qq.VerificationProperty,
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
