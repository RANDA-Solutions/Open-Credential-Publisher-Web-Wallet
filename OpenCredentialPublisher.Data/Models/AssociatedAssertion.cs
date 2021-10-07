using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class AssociatedAssertion : AugmentedAssertionDType
    {
        public List<AssociatedAssertion> ChildAssertions { get; set; }

        public int? ClrId { get; set; }
        public bool IsSelfPublished { get; set; }
        public int AssertionId { get; set; }
        public AssociatedAssertion ParentAssertion { get; set; }         

        public static AssociatedAssertion FromAssertionDType(AugmentedAssertionDType assertion)
        {
            var associatedAssertion = new AssociatedAssertion
            {
                Achievement = assertion.Achievement,
                AdditionalProperties = assertion.AdditionalProperties,
                AssertionClrs = assertion.AssertionClrs,
                AssertionId = assertion.AssertionKey,
                Context = assertion.Context,
                CreditsEarned = assertion.CreditsEarned,
                ActivityEndDate = assertion.ActivityEndDate,
                Endorsements = assertion.Endorsements,
                Evidence = assertion.Evidence,
                Expires = assertion.Expires,
                Id = assertion.Id,
                Image = assertion.Image,
                IsSigned = assertion.IsSigned,
                IssuedOn = assertion.IssuedOn,
                LicenseNumber = assertion.LicenseNumber,
                Narrative = assertion.Narrative,
                Recipient = assertion.Recipient,
                Results = assertion.Results,
                RevocationReason = assertion.RevocationReason,
                Revoked = assertion.Revoked,
                Role = assertion.Role,
                SignedEndorsements = assertion.SignedEndorsements,
                Source = assertion.Source,
                ActivityStartDate = assertion.ActivityStartDate,
                Term = assertion.Term,
                Type = assertion.Type,
                Verification = assertion.Verification,
                ChildAssertions = new List<AssociatedAssertion>(),
                ParentAssertion = null,
                ClrId = null

            };
            return associatedAssertion;
        }
    }
}
