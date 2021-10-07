using OpenCredentialPublisher.ObcLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Assertion payload for the GET /backpack/assertions endpoint.
    /// </summary>
    public partial class BadgrAssertionModel : BadgrAssertionDType, IBaseEntity
    {
        [JsonIgnore]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BadgrAssertionId { get; set; }
        /// <summary>
        /// Complete Issuer JSON from IssuerOpenBadgeId
        /// </summary>
        [JsonIgnore]
        public string IssuerJson { get; set; }

        /// <summary>
        /// Complete Badge JSON from OpenBadgeId 
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Complete Badge JSON from OpenBadgeId 
        /// </summary>
        [JsonIgnore]
        public string BadgeJson { get; set; }

        /// <summary>
        /// Complete User JSON from RecipientId 
        /// </summary>
        [JsonIgnore]
        public string RecipientJson { get; set; }

        /// <summary>
        /// Complete BadgeClass JSON from BadgeclassOpenBadgeId 
        /// </summary>
        [JsonIgnore]
        public string BadgeClassJson { get; set; }

        [JsonIgnore]
        public int BadgrBackpackId { get; set; }

        /// <summary>
        /// The parent backpack to this assertion
        /// </summary>
        [JsonIgnore]
        public BadgrBackpackModel BadgrBackpack { get; set; }

        /// <summary>
        /// True if SignedAssertion is not null.
        /// </summary>
        [JsonIgnore]
        public bool IsSigned {
            get
            {
                return this.SignedAssertion != null;
            }
        }
        /// <summary>
         /// True if Badgr assertion.
         /// </summary>
        public bool IsBadgr { get; set; }
        /// <summary>
        /// False if assertion json is not parsable to standard 2.0 AssertionDType.
        /// </summary>
        public bool IsValidJson { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public static BadgrAssertionModel FromBadgrAssertion(string json, BadgrObcAssertionDType badgrObcAssertionDType)
        {
            return new BadgrAssertionModel
            {
                Id = badgrObcAssertionDType.Id,
                Acceptance = null,
                AdditionalProperties = badgrObcAssertionDType.AdditionalProperties,
                BadgeClassOpenBadgeId = badgrObcAssertionDType.BadgeClassOpenBadgeId,
                Expires = badgrObcAssertionDType.Expires,
                Image = badgrObcAssertionDType.Image,
                IssuedOn = badgrObcAssertionDType.IssuedOn,
                Narrative = badgrObcAssertionDType.Narrative,
                OpenBadgeId = badgrObcAssertionDType.Id,
                Revoked = badgrObcAssertionDType.Revoked,
                RevocationReason = badgrObcAssertionDType.RevocationReason,
                Recipient = badgrObcAssertionDType.Recipient,
                Type = badgrObcAssertionDType.Type,
                Json = json,
                IsBadgr = true,
                IsValidJson = true
            };
        }
        public static BadgrAssertionModel FromObcAssertion(string json, AssertionDType obcAssertionDType)
        {
            return new BadgrAssertionModel
            {
                Id = obcAssertionDType.Id,
                Acceptance = null,
                AdditionalProperties = obcAssertionDType.AdditionalProperties,
                OpenBadgeId = obcAssertionDType.Id,
                Type = obcAssertionDType.Type,
                Json = json,
                IsBadgr = false,
                IsValidJson = true
            };
        }
        public static BadgrAssertionModel FromInvalidJson(string json, bool isSigned)
        {
            return new BadgrAssertionModel
            {
                Id = string.Empty,
                Type = string.Empty,
                Json = json,
                IsBadgr = false,
                IsValidJson = false,
                SignedAssertion = isSigned ? json : null
            };
        }
    }
}
