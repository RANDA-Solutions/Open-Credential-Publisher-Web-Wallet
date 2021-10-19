using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// Join class for many-to-many relationship.
    /// </summary>
    [NotMapped]
    public class AssertionClr
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssertionClrId { get; set; }
        [ForeignKey(nameof(Clr))]
        public int ClrKey { get; set; }
        public virtual ClrDType Clr { get; set; }

        [ForeignKey(nameof(Assertion))]
        public int AssertionKey { get; set; }
        public virtual AugmentedAssertionDType Assertion { get; set; }
    }
}