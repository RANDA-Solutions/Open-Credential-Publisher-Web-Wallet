using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using OpenCredentialPublisher.ClrLibrary.Converters;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class ResultDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int ResultKey { get; set; }

        [ForeignKey(nameof(Assertion)), JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Assertion")]
        public int? AssertionKey { get; set; }

        [JsonIgnore, Newtonsoft.Json.JsonIgnore, Display(Name = "Assertion")]
        public virtual AssertionDType Assertion { get; set; }
    }
}
