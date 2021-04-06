using OpenCredentialPublisher.Data.Models.Badgr;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models.Badgr
{
    /// <summary>
    /// Represents a Open Badge Backpack for an application user. The complete Backpack is stored as JSON.
    /// </summary>
    public class BadgrBackpackModel
    {
        public int CredentialPackageId { get; set; }

        /// <summary>
        /// Number of assertions in this Backpack.
        /// </summary>
        public int AssertionsCount { get; set; }

        /// <summary>
        /// DateTime when this Backpack was issued.
        /// </summary>
        public DateTime IssuedOn { get; set; }

        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The VC @id.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Complete JSON of the VC.
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Issuer of the VC.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Optional name of VC. Primarily for self-published VCs.
        /// </summary>
        public string Name { get; set; }

        public bool Revoked { get; set; }
        public string RevocationReason { get; set; }


        public CredentialPackageModel CredentialPackage { get; set; }
        public List<BadgrAssertionModel> BadgrAssertions { get; set; }

        public ClrModel ToClrModel()
        {
            var clr = new ClrModel();

            clr.Authorization = this.CredentialPackage.Authorization;
            clr.Name = "Badgr Backpack";
            clr.IssuedOn = this.IssuedOn;
            clr.Identifier = this.Identifier;
            return clr;
        }
    }        
}
