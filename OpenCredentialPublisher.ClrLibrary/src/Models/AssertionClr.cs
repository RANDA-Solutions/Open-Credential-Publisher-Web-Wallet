using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.ClrLibrary.Models
{
    /// <summary>
    /// Join class for many-to-many relationship.
    /// </summary>
    public class AssertionClr
    {
        [ForeignKey(nameof(Clr))]
        public int ClrKey { get; set; }
        public virtual ClrDType Clr { get; set; }

        [ForeignKey(nameof(Assertion))]
        public int AssertionKey { get; set; }
        public virtual AssertionDType Assertion { get; set; }
    }
}