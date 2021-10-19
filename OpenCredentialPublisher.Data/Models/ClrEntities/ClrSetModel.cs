using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class ClrSetModel : IBaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentCredentialPackageId { get; set; }
        public int? ParentVerifiableCredentialId { get; set; }
        public int ClrsCount { get; set; }
        public string Identifier { get; set; }
        public string Json { get; set; }
        
        [ForeignKey("ParentCredentialPackageId")]
        public CredentialPackageModel ParentCredentialPackage { get; set; }
        [ForeignKey("ParentVerifiableCredentialId")]
        public VerifiableCredentialModel ParentVerifiableCredential { get; set; }
        public List<ClrModel> Clrs { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public void Delete()
        {
            this.IsDeleted = true;
            this.ModifiedAt = DateTime.UtcNow;
        }
        public ClrSetModel()
        {
            Clrs = new List<ClrModel>();
        }        
    }
}
