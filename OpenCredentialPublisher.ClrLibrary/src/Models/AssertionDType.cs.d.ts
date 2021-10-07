declare module server {
	/** Assertions are representations of an Achievement awarded to a Learner.  It is used to share information about the Achievement Assertion, such as a result and verification method. Assertions are packaged for transmission as JSON objects with a set of mandatory and optional properties. */
	interface assertionDType {
		/** Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** Gets or Sets Achievement */
		achievement: {
		};
		/** The number of credits earned, generally in semester or quarter credit hours.  This field correlates with the Achievement creditsAvailable field. Model Primitive Datatype = Float. */
		creditsEarned?: number;
		/** If present, the assertion is not valid after this date. Model Primitive Datatype = DateTime. */
		activityEndDate?: Date;
		/** Allows endorsers to make specific claims about the assertion. */
		endorsements: any[];
		/** Evidence describing the work that the recipient did to earn the achievement. This can be a webpage that links out to other pages if linking directly to the work is infeasible. */
		evidence: server.evidenceDType[];
		expires?: Date;
		/** IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
		image: string;
		/** Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime. */
		issuedOn?: Date;
		/** The license number that was issued with this assertion. Model Primitive Datatype = String. */
		licenseNumber: string;
		/** A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String. */
		narrative: string;
		/** Gets or Sets Recipient */
		recipient: {
		};
		/** The set of results being asserted. */
		results: any[];
		/** Optional published reason for revocation, if revoked. Model Primitive Datatype = String. */
		revocationReason: string;
		/** Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean. */
		revoked?: boolean;
		/** Role, position, or title of the learner when demonstrating or performing the achievement or evidence of learning being asserted. Examples include 'Student President', 'Intern', 'Captain', etc. Model Primitive Datatype = String. */
		role: string;
		/** Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. */
		signedEndorsements: string[];
		/** Gets or Sets Source */
		source: server.profileDType;
		/** If present, the assertion is not valid before this date. Model Primitive Datatype = DateTime. */
		activityStartDate?: Date;
		/** The academic term in which this assertion was achieved. Model Primitive Datatype = String. */
		term: string;
		/** Gets or Sets Verification */
		verification: {
		};
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
