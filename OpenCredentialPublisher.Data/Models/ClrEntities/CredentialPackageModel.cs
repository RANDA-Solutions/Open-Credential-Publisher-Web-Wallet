using OpenCredentialPublisher.Data.Models.Badgr;
using OpenCredentialPublisher.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    public class CredentialPackageModel : IBaseEntity
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public PackageTypeEnum TypeId { get; set; }
        public int AssertionsCount { get; set; }
        public bool Revoked { get; set; }
        public string RevocationReason { get; set; }
        /// <summary>
        /// Foreign key back to the authorization.
        /// </summary>
        [StringLength(450)]
        public string AuthorizationForeignKey { get; set; }
        public AuthorizationModel Authorization { get; set; }
        public ClrModel Clr { get; set; }
        public ClrSetModel ClrSet { get; set; }
        public BadgrBackpackModel BadgrBackpack { get; set; }
        public VerifiableCredentialModel VerifiableCredential { get; set; }
       
        public String Name { get; set; }

        /// <summary>
        /// This Package is tied to a specific application user.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public virtual List<ClrModel> ContainedClrs { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }

    }
}
