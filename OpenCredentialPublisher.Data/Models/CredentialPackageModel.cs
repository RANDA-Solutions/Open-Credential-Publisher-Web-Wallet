using OpenCredentialPublisher.Data.Models.Badgr;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{
    public class CredentialPackageModel
    {
        /// <summary>
        /// Primary key
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public PackageTypeEnum TypeId { get; set; }

        public bool Revoked { get; set; }
        public string RevocationReason { get; set; }
        /// <summary>
        /// Foreign key back to the authorization.
        /// </summary>
        [StringLength(450)]
        public string AuthorizationForeignKey { get; set; }
        [ForeignKey("AuthorizationForeignKey")]
        public AuthorizationModel Authorization { get; set; }
        public ClrModel Clr { get; set; }
        public ClrSetModel ClrSet { get; set; }
        public BadgrBackpackModel BadgrBackpack { get; set; }
        public VerifiableCredentialModel VerifiableCredential { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public String Name { get; set; }

        /// <summary>
        /// This Package is tied to a specific application user.
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }               

    }
}
