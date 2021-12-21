using OpenCredentialPublisher.Data.Extensions;
using OpenCredentialPublisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Legacy = OpenCredentialPublisher.Data.ViewModels.Credentials;

namespace OpenCredentialPublisher.Data.ViewModels.nG
{
    public class ClrAssertionVM
    {
        public int Id { get; set; }
        public string AchievementName { get; set; }
        public string AchievementType { get; set; }
        public string AchievementIssuerName { get; set; }
        public DateTime? IssuedOn { get; set; }
        public bool IsCollapsed { get; set; }
        public List<AchievementResult> AchievementResults { get; set; }
        public static ClrAssertionVM FromClrAssertion(AssertionModel assertion)
        {
            var results = new List<AchievementResult>();
            if (assertion.Achievement.ResultDescriptions != null)
            {
                foreach (var rd in assertion.Achievement.ResultDescriptions)
                {
                    var resultValue = assertion.Results.Where(x => x.ResultDescription == rd?.Id).FirstOrDefault();
                    results.Add(new AchievementResult { Name = rd.Name, Value = resultValue.Value });
                }
            }
            var vm = new ClrAssertionVM
            {
                Id = assertion.AssertionId,
                IssuedOn = assertion.IssuedOn == DateTime.MinValue || assertion.IssuedOn == null ? null : assertion.IssuedOn,
                AchievementName = assertion.Achievement?.Name,
                AchievementType = assertion.Achievement?.AchievementType,
                AchievementIssuerName = assertion.Achievement?.Issuer.Name,
                AchievementResults = results,
                IsCollapsed = true
            };
            return vm;
        }
    }
    public class AchievementResult
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}
