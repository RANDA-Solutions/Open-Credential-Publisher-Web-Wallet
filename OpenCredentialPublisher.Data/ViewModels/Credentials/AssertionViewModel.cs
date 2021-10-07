using OpenCredentialPublisher.ClrLibrary.Models;
using System.Collections.Generic;
using OpenCredentialPublisher.Data.Models.Badgr;

namespace OpenCredentialPublisher.Data.ViewModels.Credentials
{

    /// <summary>
    /// Represents an Assertion of a CLR for an application user
    /// </summary>
    public class AssertionViewModel 
    {
        public AugmentedAssertionDType Assertion { get; set; }
        public BadgrAssertionModel BadgrAssertion { get; set; }
        public List<EndorsementDType> AllEndorsements { get; set; }
        public AchievementViewModel AchievementVM { get; set; }
        public string SignedAssertion { get; set; }
        public AssertionViewModel() : base()
        {
            AllEndorsements = new List<EndorsementDType>();
        }
        public static AssertionViewModel FromAssertionDType(AugmentedAssertionDType assertion, bool isSigned, string signedAssertion = null)
        {
            assertion.IsSigned = isSigned;
            return new AssertionViewModel()
            {
                SignedAssertion = signedAssertion,
                Assertion = assertion,
                AllEndorsements = new List<EndorsementDType>()
            };
        }
        public static AssertionViewModel FromBadgrAssertion(BadgrAssertionModel assertion, bool isSigned)
        {
            return new AssertionViewModel()
            {
                BadgrAssertion = assertion,
                AllEndorsements = new List<EndorsementDType>()
            };
        }
    }
}
