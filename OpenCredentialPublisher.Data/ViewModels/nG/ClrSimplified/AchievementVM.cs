using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Models.ClrEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class AchievementVM
    {

        public int AchievementId { get; set; }

        //IBaseEntity properties
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        /*********************************************************************************************
         * From AchievementDType
         *********************************************************************************************/
        public string Id { get; set; }
        public string Type { get; set; }
        public virtual string AchievementType { get; set; }
        public float? CreditsAvailable { get; set; }
        public string Description { get; set; }
        public string HumanCode { get; set; }
        public List<SystemIdentifierDType> Identifiers { get; set; }
        public string Name { get; set; }
        public string FieldOfStudy { get; set; }
        public string Image { get; set; }
        public string Level { get; set; }
        public string Specialization { get; set; }
        public List<string> Tags { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }
        public List<ResultDescriptionVM> ResultDescriptions { get; set; }
        /*********************************************************************************************
         * End From AchievementDType
         *********************************************************************************************/
        public ProfileVM Issuer { get; set; }
        public CriteriaModel Requirement { get; set; }

        public static AchievementVM FromModel(AchievementModel achievement)
        {
            return new AchievementVM
            {
                AchievementId = achievement.AchievementId,
                AchievementType = achievement.AchievementType,
                AdditionalProperties = achievement.AdditionalProperties,
                CreatedAt = achievement.CreatedAt,
                CreditsAvailable = achievement.CreditsAvailable,
                Description = achievement.Description,
                FieldOfStudy = achievement.FieldOfStudy,
                HumanCode = achievement.HumanCode,
                Id = achievement.Id,
                Identifiers = achievement.Identifiers,
                Image = achievement.Image,
                IsDeleted = achievement.IsDeleted,
                Issuer = ProfileVM.FromModel(achievement.Issuer),
                Level = achievement.Level,
                ModifiedAt = achievement.ModifiedAt,
                Name = achievement.Name,
                Requirement = achievement.Requirement,
                Specialization = achievement.Specialization,
                Tags = achievement.Tags,
                Type = achievement.Type,
                ResultDescriptions = achievement.ResultDescriptions.Select(rd => ResultDescriptionVM.FromModel(rd)).ToList()
            };
        }
    }
}
