using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class AssertionWithAchievementVM
    {
        public int AssertionId { get; set; }
        public string SignedAssertion { get; set; }

        public string Id { get; set; }
        public string Type { get; set; }
        public float? CreditsEarned { get; set; }
        public DateTime? ActivityEndDate { get; set; }
        public DateTime? Expires { get; set; }
        public string Image { get; set; }
        public DateTime? IssuedOn { get; set; }
        public string LicenseNumber { get; set; }
        public string Narrative { get; set; }
        public string RevocationReason { get; set; }
        public bool? Revoked { get; set; }
        public string Role { get; set; }
        public DateTime? ActivityStartDate { get; set; }
        public string Term { get; set; }
        public Dictionary<String, Object> AdditionalProperties { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public string Context { get; set; }
        public bool IsSigned { get; set; }
        public bool IsSelfPublished { get; set; }

        public AchievementVM Achievement { get; set; }
        public IdentityVM Recipient { get; set; }
        public VerificationVM Verification { get; set; }

        public static AssertionWithAchievementVM FromAssertion(AssertionModel assertion)
        {
            if (assertion == null)
            {
                throw new Exception("wtf");
            }
            return new AssertionWithAchievementVM
            {
                Achievement = AchievementVM.FromModel(assertion.Achievement),
                ActivityEndDate = assertion.ActivityEndDate,
                ActivityStartDate = assertion.ActivityStartDate,
                AdditionalProperties = assertion.AdditionalProperties,
                AssertionId = assertion.AssertionId,
                Context = assertion.Context,
                CreatedAt = assertion.CreatedAt,
                CreditsEarned = assertion.CreditsEarned,
                Expires = assertion.Expires,
                Id = assertion.Id,
                Image = assertion.Image,
                IsDeleted = assertion.IsDeleted,
                IsSelfPublished = assertion.IsSelfPublished,
                IsSigned = assertion.IsSigned,
                IssuedOn = assertion.IssuedOn,
                LicenseNumber = assertion.LicenseNumber,
                ModifiedAt = assertion.ModifiedAt,
                Narrative = assertion.Narrative,
                RevocationReason = assertion.RevocationReason,
                Revoked = assertion.Revoked,
                Role = assertion.Role,
                SignedAssertion = assertion.SignedAssertion,
                Term = assertion.Term,
                Type = assertion.Type
            };
        }
    }
}

