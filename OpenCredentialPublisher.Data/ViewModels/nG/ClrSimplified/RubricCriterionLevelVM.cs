using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class RubricCriterionLevelVM
    {
        public int RubricCriterionLevelId { get; set; }
        public int Order { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public string Name { get; set; }
        public string Points { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        public int ResultDescriptionId { get; set; }


        public static RubricCriterionLevelVM FromModel(RubricCriterionLevelModel qq)
        {
            return new RubricCriterionLevelVM
            {
                Description = qq.Description,
                Level = qq.Level,
                Points = qq.Points,
                RubricCriterionLevelId = qq.RubricCriterionLevelId,                    
                Name = qq.Name,
                ResultDescriptionId = qq.ResultDescriptionId,
                AdditionalProperties = qq.AdditionalProperties,
                CreatedAt = qq.CreatedAt,
                Id = qq.Id,
                IsDeleted = qq.IsDeleted,
                ModifiedAt = qq.ModifiedAt,
                Type = qq.Type,
                Order = qq.Order
            };
        }
    }
}
