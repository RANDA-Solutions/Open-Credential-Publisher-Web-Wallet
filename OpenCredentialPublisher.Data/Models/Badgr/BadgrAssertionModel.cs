using OpenCredentialPublisher.ClrLibrary.Models;
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
    public partial class BadgrAssertionModel : BadgrAssertionDType
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

    }
}
