using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenCredentialPublisher.Data.Models
{

    /// <summary>
    /// Represents a CLR for an application user. The complete CLR is stored as JSON.
    /// </summary>
    public class ClrModel
    {
        public int? CredentialPackageId { get; set; }
        public int? VerifiableCredentialId { get; set; }
        public int? ClrSetId { get; set; }

        /// <summary>
        /// Number of assertions in this CLR.
        /// </summary>
        public int AssertionsCount { get; set; }

        /// <summary>
        /// The resource server authorization that was used to get this CLR.
        /// </summary>
        public AuthorizationModel Authorization { get; set; }

        /// <summary>
        /// Foreign key back to the authorization.
        /// </summary>
        public string AuthorizationForeignKey { get; set; }

        /// <summary>
        /// DateTime when this CLR was issued.
        /// </summary>
        public DateTime IssuedOn { get; set; }

        /// <summary>
        /// Primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The CLR @id.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Complete JSON of the CLR.
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// Learner of the CLR.
        /// </summary>
        public string LearnerName { get; set; }

        /// <summary>
        /// All the links tied to this CLR.
        /// </summary>
        public List<LinkModel> Links { get; set; }

        /// <summary>
        /// Optional name of CLR. Primarily for self-published CLRs.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Publisher of the CLR.
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// The date and time the CLR was retrieved from the authorization server.
        /// </summary>
        public DateTime RefreshedAt { get; set; }

        /// <summary>
        /// The Signed CLR if it was signed.
        /// </summary>
        public string SignedClr { get; set; }

        public CredentialPackageModel CredentialPackage { get; set; }
        public VerifiableCredentialModel VerifiableCredential { get; set; }
        public ClrSetModel ClrSet { get; set; }
    }
}
