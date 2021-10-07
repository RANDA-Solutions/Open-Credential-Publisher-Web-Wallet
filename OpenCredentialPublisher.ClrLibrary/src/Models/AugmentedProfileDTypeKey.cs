using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    [NotMapped]
    public class AugmentedProfileDType : ProfileDType
    {
        [Key, JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public int ProfileKey { get; set; }

        /// <summary>
        /// Reference to the associated <see cref="ProfileDType"/>.
        /// </summary>
        /// <remarks>
        /// <see cref="EndorsementProfileDType"/> and <see cref="ProfileDType"/> have
        /// a one-to-one relationship. <see cref="EndorsementProfileDType"/> only has
        /// required fields. <see cref="ProfileDType"/> is the source of truth for all
        /// fields.
        /// </remarks>
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public virtual EndorsementProfileDType EndorsementProfile { get; set; }
    }
}
