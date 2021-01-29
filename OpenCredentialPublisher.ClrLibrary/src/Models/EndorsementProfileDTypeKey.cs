using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
// ReSharper disable UnusedMember.Global

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    public partial class EndorsementProfileDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int EndorsementProfileKey { get; set; }

        /// <summary>
        /// Reference to the associated <see cref="ProfileDType"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="EndorsementProfileDType"/> and <see cref="ProfileDType"/> have
        /// a one-to-one relationship. <see cref="EndorsementProfileDType"/> only has
        /// required fields. <see cref="ProfileDType"/> is the source of truth for all
        /// fields.
        /// </remarks>
        [ForeignKey(nameof(Profile)), JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int? ProfileKey { get; set; }
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual ProfileDType Profile { get; set; }
    }
}