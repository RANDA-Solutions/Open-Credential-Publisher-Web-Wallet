using OpenCredentialPublisher.ClrLibrary.Models;
using System.Collections.Generic;
using OpenCredentialPublisher.Data.Models.Badgr;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{

    /// <summary>
    /// Represents an Assertion of a CLR for an application user
    /// </summary>
    public class BadgrAssertionViewModel 
    {
        public BadgrAssertionModel Assertion { get; set; }
        public List<EndorsementDType> AllEndorsements { get; set; }
        public AchievementViewModel AchievementVM { get; set; }
        public BadgrAssertionViewModel() : base()
        {
            AllEndorsements = new List<EndorsementDType>();
        }
        public static AssertionViewModel FromAssertionDType(AssertionDType assertion, bool isSigned)
        {
            assertion.IsSigned = isSigned;
            return new AssertionViewModel()
            {
                Assertion = assertion,
                AllEndorsements = new List<EndorsementDType>()
            };
        }
        public static AssertionViewModel FromBadgrAssertion(BadgrAssertionModel assertion)
        {
            return new AssertionViewModel()
            {
                BadgrAssertion = assertion,
                AllEndorsements = new List<EndorsementDType>()
            };
        }
    }
}
