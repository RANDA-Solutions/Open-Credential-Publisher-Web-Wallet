using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;

namespace OpenCredentialPublisher.Data.ViewModels.nG.ClrSimplified
{
    public class ClrVM
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public string Name { get; set; }
        public string PublisherName { get; set; }
        public bool IsRevoked { get; set; }
        public string Identifier { get; set; }
        public bool IsSelected { get; set; } = false;
        public string AuthorizationId { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime RefreshedAt { get; set; }
        public int AssertionsCount { get; set; }
        public bool IsCollapsed { get; set; } = true;
        public List<string> AchievementIds { get; set; }
        public ProfileVM Learner { get; set; }
        public ProfileVM Publisher { get; set; }

        public static ClrVM FromModel(ClrModel clr, List<string> achievementIds = null)
        {
            var authId = clr.Authorization == null ? null : clr.Authorization.Id;
            return new ClrVM
            {
                Id = clr.ClrId,
                IsRevoked = clr.IsRevoked,
                IssuedOn = clr.IssuedOn,
                Name = clr.Name,
                PackageId = clr.CredentialPackageId,
                PublisherName = clr.PublisherName,
                AuthorizationId = authId,
                RefreshedAt = clr.RefreshedAt,
                AssertionsCount = clr.AssertionsCount,
                Identifier = clr.Id,
                AchievementIds = achievementIds ?? new List<string>(),
                Learner = ProfileVM.FromModel(clr.Learner),
                Publisher = ProfileVM.FromModel(clr.Publisher)
            };
        }
    }
}
