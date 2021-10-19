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
    public class BadgrBackpackModel : IBaseEntity
    {
        public int ParentCredentialPackageId { get; set; }

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

        public bool IsBadgr { get; set; }

        public bool Revoked { get; set; }
        public string RevocationReason { get; set; }        


        public CredentialPackageModel ParentCredentialPackage { get; set; }
        public List<BadgrAssertionModel> BadgrAssertions { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public ClrModel ToClrModel()
        {
            var clr = new ClrModel();

            clr.Authorization = this.ParentCredentialPackage.Authorization;
            clr.Name = "Badgr Backpack";
            clr.IssuedOn = this.IssuedOn;
            clr.Id = this.Identifier;
            return clr;
        }
    }        
}
