declare module server {
	/** A Profile is a collection of information that describes the person or organization using Comprehensive Learner Record (CLR). Publishers, learners, and issuers must be represented as profiles. Recipients, endorsers, or other entities may also be represented using this vocabulary. */
	interface profileDType {
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** Gets or Sets Address */
		address: server.addressDType;
		additionalName: string;
		birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
		description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
		email: string;
		/** Allows endorsers to make specific claims about the individual or organization represented by this profile. */
		endorsements: server.endorsementDType[];
		familyName: string;
		givenName: string;
		identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
		image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
		name: string;
		official: string;
		parentOrg: server.profileDType;
		/** Gets or Sets PublicKey */
		publicKey: server.cryptographicKeyDType;
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
		revocationList: string;
		/** Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. */
		signedEndorsements: string[];
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
		sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
		studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
		telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
		url: string;
		/** Gets or Sets Verification */
		verification: server.verificationDType;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
