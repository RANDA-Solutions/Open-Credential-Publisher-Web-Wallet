using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityModel;

namespace OpenCredentialPublisher.Data.Models
{
    /// <summary>
    /// Represents a Shareable link to a CLR.
    /// </summary>
    public class LinkModel: IBaseEntity
    {
        /// <summary>
        /// Create a new instance of the object.
        /// </summary>
        public LinkModel()
        {
            // Create an ID that can be used in the link URL.
            Id = CryptoRandom.CreateUniqueId(32).Substring(0, 16);
        }

        /// <summary>
        /// The primary key.
        /// </summary>
        [Key]
        public string Id { get; set; }

        public int ClrForeignKey { get; set; }
        /// <summary>
        /// The CLR this link points to.
        /// </summary>
        public virtual ClrModel Clr { get; set; }        

        /// <summary>
        /// The number of times this link has been used to display the CLR.
        /// </summary>
        public int DisplayCount { get; set; }

        /// <summary>
        /// A nickname for the link to help remember who it was shared with.
        /// </summary>
        public string Nickname { get; set; }

        public bool RequiresAccessKey { get; set; }

        /// <summary>
        /// Application user 
        /// </summary>
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int? CredentialRequestId { get; set; }
        [ForeignKey("CredentialRequestId")]
        public CredentialRequestModel CredentialRequest { get; set; }

        public List<ShareModel> Shares { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
    }
}
