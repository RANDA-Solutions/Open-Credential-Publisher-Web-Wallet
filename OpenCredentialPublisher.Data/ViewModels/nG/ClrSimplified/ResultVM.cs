using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class ResultVM
    {
        public int ResultId { get; set; }
        public int Order { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string AchievedLevel { get; set; }
        public string ResultDescription { get; set; }
        public string Status { get; set; }
        public string Value { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        /*********************************************************************************************
         * End From ResultDType
         *********************************************************************************************/
        //ForeignKeys
        public int AssertionId { get; set; }

        public static ResultVM FromModel(ResultModel qq)
        {
            return new ResultVM
            {
                AchievedLevel = qq.AchievedLevel,
                AdditionalProperties = qq.AdditionalProperties,
                AssertionId = qq.AssertionId,
                CreatedAt = qq.CreatedAt,
                Id = qq.Id,
                IsDeleted = qq.IsDeleted,
                ModifiedAt = qq.ModifiedAt,
                Type = qq.Type,
                Order = qq.Order,
                ResultDescription = qq.ResultDescription,
                ResultId = qq.ResultId,
                Status = qq.Status,
                Value = qq.Value                 
            };
        }
    }
}
