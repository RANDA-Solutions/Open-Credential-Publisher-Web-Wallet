declare module server {
	/** A collection of information about the recipient of an achievement assertion. */
	interface identityDType {
		/** Unique IRI for the Identity. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The type should identify the property by which the recipient of an Assertion is identified. This value should be an IRI mapped in the present context. For example, 'id' indicates that the identity property value is the id of the recipient's profile. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** Either the hash of the identity or the plaintext value. If it's possible that the plaintext transmission and storage of the identity value would leak personally identifiable information where there is an expectation of privacy, it is strongly recommended that an IdentityHash be used. Model Primitive Datatype = String. */
		identity: string;
		/** Whether or not the identity value is hashed. Model Primitive Datatype = Boolean. */
		hashed: boolean;
		/** If the recipient is hashed, this should contain the string used to salt the hash. If this value is not provided, it should be assumed that the hash was not salted. Model Primitive Datatype = String. */
		salt: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
