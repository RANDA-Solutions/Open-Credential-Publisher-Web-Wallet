using OpenCredentialPublisher.Data.Models.ClrEntities;
using OpenCredentialPublisher.Data.Models.ClrEntities.Relationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class ResultDescriptionVM
    {
        public int ResultDescriptionId { get; set; }
        public int Order { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public List<string> AllowedValues { get; set; }
        public string Name { get; set; }
        public string RequiredLevel { get; set; }
        public string RequiredValue { get; set; }
        public virtual string ResultType { get; set; }
        public string ValueMax { get; set; }
        public string ValueMin { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        public int AchievementId { get; set; }
        public List<AlignmentVM> Alignments { get; set; }
        public List<RubricCriterionLevelVM> RubricCriterionLevels { get; set; }

        public static ResultDescriptionVM FromModel(ResultDescriptionModel qq)
        {
            return new ResultDescriptionVM
            {
                AchievementId = qq.AchievementId,
                AllowedValues = qq.AllowedValues,
                Name = qq.Name,
                RequiredLevel = qq.RequiredLevel,
                RequiredValue = qq.RequiredValue,
                ResultDescriptionId =    qq.ResultDescriptionId,
                ResultType = qq.ResultType,
                ValueMax = qq.ValueMax,
                ValueMin = qq.ValueMin,                           
                AdditionalProperties = qq.AdditionalProperties,
                CreatedAt = qq.CreatedAt,
                Id = qq.Id,
                IsDeleted = qq.IsDeleted,
                ModifiedAt = qq.ModifiedAt,
                Type = qq.Type,
                Order = qq.Order,
                Alignments = qq.ResultDescriptionAlignments.Select(rda => AlignmentVM.FromModel(rda.Alignment)).ToList(),
                RubricCriterionLevels = qq.RubricCriterionLevels.Select(rcl => RubricCriterionLevelVM.FromModel(rcl)).ToList()
            };
        }
    }
}
