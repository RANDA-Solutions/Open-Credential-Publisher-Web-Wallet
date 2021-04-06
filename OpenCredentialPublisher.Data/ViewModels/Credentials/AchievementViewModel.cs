using OpenCredentialPublisher.ClrLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{

    /// <summary>
    /// Represents an Assertion of a CLR for an application user
    /// </summary>
    public class AchievementViewModel
    {
        public AchievementDType Achievement { get; set; }
        public List<EndorsementDType> AllEndorsements { get; set; }
        public AchievementViewModel() : base()
        {
            AllEndorsements = new List<EndorsementDType>();
        }

        public static AchievementViewModel FromAchievementDType(AchievementDType achievement)
        {
            return new AchievementViewModel()
            {
                Achievement = achievement,
                AllEndorsements = new List<EndorsementDType>()
            };
        }
    }
}
