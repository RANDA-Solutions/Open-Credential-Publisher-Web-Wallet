using OpenCredentialPublisher.ClrLibrary.Models;
using OpenCredentialPublisher.Data.ViewModels.Credentials;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.Data.Models
{
    public class ClrSetModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CredentialPackageId { get; set; }
        public int? VerifiableCredentialId { get; set; }
        public int ClrsCount { get; set; }
        public string Identifier { get; set; }
        public string Json { get; set; }
        [ForeignKey("CredentialPackageId")]
        public CredentialPackageModel CredentialPackage { get; set; }
        [ForeignKey("VerifiableCredentialId")]
        public VerifiableCredentialModel VerifiableCredential { get; set; }
        public List<ClrModel> Clrs { get; set; }

        public ClrSetModel()
        {
            Clrs = new List<ClrModel>();
        }        
    }
}
