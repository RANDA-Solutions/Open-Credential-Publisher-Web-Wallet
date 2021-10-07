using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class AlignmentVM
    {
        public int AlignmentId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string EducationalFramework { get; set; }
        public string TargetCode { get; set; }
        public string TargetDescription { get; set; }
        public string TargetName { get; set; }
        public virtual string TargetType { get; set; }
        public string TargetUrl { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }


        public static AlignmentVM FromModel(AlignmentModel qq)
        {
            return new AlignmentVM
            {
                AlignmentId=qq.AlignmentId,
                EducationalFramework = qq.EducationalFramework,
                TargetCode = qq.TargetCode,
                TargetDescription = qq.TargetDescription,
                TargetName = qq.TargetName,
                TargetType = qq.TargetType,
                TargetUrl = qq.TargetUrl,
                AdditionalProperties = qq.AdditionalProperties,
                CreatedAt = qq.CreatedAt,
                Id = qq.Id,
                IsDeleted = qq.IsDeleted,
                ModifiedAt = qq.ModifiedAt,
                Type = qq.Type
            };
        }
    }
}
