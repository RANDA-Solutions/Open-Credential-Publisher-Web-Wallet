using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedAssertionDType: AssertionDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int AssertionKey { get; set; }

        [NotMapped]
        public bool IsSigned { get; set; }        
        /// <summary>
        /// URL to the JSON-LD context file.
        /// </summary>
        /// <value>URL to the JSON-LD context file.</value>
        [JsonPropertyName("@context"), Newtonsoft.Json.JsonProperty("@context")]
        [Description("URL to the JSON-LD context file.")]
        public string Context { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual IList<AssertionClr> AssertionClrs { get; set; }

        public static AugmentedAssertionDType FromAssertionDType(AssertionDType assertion)
        {
            var augmentedAssertion = new AugmentedAssertionDType
            {
                Achievement = assertion.Achievement,
                ActivityEndDate = assertion.ActivityEndDate,
                ActivityStartDate = assertion.ActivityStartDate,
                AdditionalProperties = assertion.AdditionalProperties,
                CreditsEarned = assertion.CreditsEarned,
                Endorsements = assertion.Endorsements,
                Evidence = assertion.Evidence,
                Expires = assertion.Expires,
                Id = assertion.Id,
                Image = assertion.Image,
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
                Term = assertion.Term,
                Type = assertion.Type,
                Verification = assertion.Verification
            };
            return augmentedAssertion;
        }
    }
}
