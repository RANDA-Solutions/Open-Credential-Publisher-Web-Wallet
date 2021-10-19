using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenCredentialPublisher.ClrLibrary.Models.AssociationDType;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class AssociationVM
    {
        public int AssociationId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public AssociationTypeEnum AssociationType { get; set; }
        public string TargetId { get; set; }
        public string Uri { get; set; }
        public string Title { get; set; }
        public string TargetAchievementName { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        public static AssociationVM FromModel(AssociationModel qq)
        {
            Uri uri;
            string targetAchievementName = null;
            System.Uri.TryCreate(qq.TargetId, UriKind.Absolute, out uri);

            // note: the following slows things down to a crawl
            //var targetAssertion = qq.AchievementAssociation.Achievement.ClrAchievement.Clr.ClrAssertions.Select(ca => ca.Assertion).FirstOrDefault(a => a.Achievement.Id == qq.TargetId);
            //if (targetAssertion != null)
            //{
            //    targetAchievementName = targetAssertion.Achievement?.Name ?? "";
            //}
            
            return new AssociationVM
            {
                AssociationId = qq.AssociationId,
                AssociationType = qq.AssociationType,
                TargetId = qq.TargetId,
                Title = qq.Title,                  
                AdditionalProperties = qq.AdditionalProperties,
                CreatedAt = qq.CreatedAt,
                IsDeleted = qq.IsDeleted,
                ModifiedAt = qq.ModifiedAt,
                Uri = "http|https".Contains(uri.Scheme) ? uri.AbsolutePath : null,
                TargetAchievementName = targetAchievementName ?? String.Empty
            };
        }
    }
}
