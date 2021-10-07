declare module server {
	/** Represents an Verification entity in the CLR model. */
	interface verificationModel {
		/** Primary key. */
		verificationId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		/** Unique IRI for the Verification. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. The strongly typed value indicates the verification method. */
		type: any;
		/** The host registered name subcomponent of an allowed origin. Any given id URI will be considered valid. Model Primitive Datatype = String. */
		allowedOrigins: string[];
		/** The (HTTP) id of the key used to sign the Assertion, CLR, or Endorsement. If not present, verifiers will check the public key declared in the referenced issuer Profile. If a key is declared here, it must be authorized in the issuer Profile as well. creator is expected to be the dereferencable URI of a document that describes a CryptographicKey. Model Primitive Datatype = AnyURI. */
		creator: string;
		/** The URI fragment that the verification property must start with. Valid Assertions, Clrs, and Endorsements must have an id within this scope. Multiple values allowed, and Assertions, Clrs, and Endorsements will be considered valid if their id starts with one of these values. Model Primitive Datatype = String. */
		startsWith: string[];
		/** The property to be used for verification. Only 'id' is supported. Verifiers will consider 'id' the default value if verificationProperty is omitted or if an issuer Profile has no explicit verification instructions, so it may be safely omitted. Model Primitive Datatype = String. */
		verificationProperty: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
		/** End From VerificationDType */
		assertion: server.assertionModel;
		clr: {
		/** Foreign key back to the authorization. */
			authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
			assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
			authorization: server.authorizationModel;
		/** Primary key. */
			clrId: number;
		/** Complete JSON of the CLR. */
			json: string;
		/** Learner of the CLR. */
			learnerName: string;
		/** Publisher of the CLR. */
			publisherName: string;
		/** The date and time the CLR was retrieved from the authorization server. */
			refreshedAt: Date;
		/** URL to the JSON-LD context file. */
			context: string;
		/** Unique IRI for the CLR. If the CLR is meant to be verified using Hosted verification, the id must conform to the getClr endpoint format. Model Primitive Datatype = NormalizedString. */
			id: string;
		/** The JSON-LD type of this object. Normally 'CLR'. Model Primitive Datatype = NormalizedString. */
			type: string;
		/** Timestamp of when the CLR was published. Model Primitive Datatype = DateTime. */
			issuedOn: Date;
		/** Optional name of the CLR. Model Primitive Datatype = String. */
			name: string;
		/** True if CLR does not contain all the assertions known by the publisher for the learner at the time the CLR is issued. Useful if you are sending a small set of achievements in real time when they are achieved. If False or omitted, the CLR SHOULD be interpreted as containing all the assertions for the learner known by the publisher at the time of issue. Model Primitive Datatype = Boolean. */
			partial?: boolean;
		/** If revoked, optional reason for revocation. Model Primitive Datatype = String. */
			revocationReason: string;
		/** If True the CLR is revoked. Model Primitive Datatype = Boolean. */
			revoked?: boolean;
		/** Additional properties of the object */
			additionalProperties: { [index: string]: any };
		/** The Signed CLR if it was signed. */
			signedClr: string;
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
			credentialPackageId: number;
		/** true indicates this CLR's Id was received in a revocation list from the source */
			isRevoked: boolean;
		/** Foreign Keys */
			parentCredentialPackageId?: number;
			parentVerifiableCredentialId?: number;
			parentClrSetId?: number;
			learnerId: number;
			publisherId: number;
			verificationId: number;
		/** Relationships */
			links: server.linkModel[];
			credentialPackage: {
		/** Primary key */
				id: number;
				typeId: any;
				assertionsCount: number;
				revoked: boolean;
				revocationReason: string;
		/** Foreign key back to the authorization. */
				authorizationForeignKey: string;
				authorization: server.authorizationModel;
				clr: any;
				clrSet: {
					id: number;
					parentCredentialPackageId?: number;
					parentVerifiableCredentialId?: number;
					clrsCount: number;
					identifier: string;
					json: string;
					parentCredentialPackage: any;
					parentVerifiableCredential: server.verifiableCredentialModel;
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				badgrBackpack: {
					parentCredentialPackageId: number;
		/** Number of assertions in this Backpack. */
					assertionsCount: number;
		/** DateTime when this Backpack was issued. */
					issuedOn: Date;
		/** Primary key. */
					id: number;
		/** The VC @id. */
					identifier: string;
		/** Complete JSON of the VC. */
					json: string;
		/** Issuer of the VC. */
					provider: string;
		/** Optional name of VC. Primarily for self-published VCs. */
					name: string;
					revoked: boolean;
					revocationReason: string;
					parentCredentialPackage: any;
					badgrAssertions: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				verifiableCredential: server.verifiableCredentialModel;
				name: string;
		/** This Package is tied to a specific application user. */
				userId: string;
				user: server.applicationUser;
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
				containedClrs: any[];
			};
		/** possible Parents */
			parentCredentialPackage: {
		/** Primary key */
				id: number;
				typeId: any;
				assertionsCount: number;
				revoked: boolean;
				revocationReason: string;
		/** Foreign key back to the authorization. */
				authorizationForeignKey: string;
				authorization: server.authorizationModel;
				clr: any;
				clrSet: {
					id: number;
					parentCredentialPackageId?: number;
					parentVerifiableCredentialId?: number;
					clrsCount: number;
					identifier: string;
					json: string;
					parentCredentialPackage: any;
					parentVerifiableCredential: server.verifiableCredentialModel;
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				badgrBackpack: {
					parentCredentialPackageId: number;
		/** Number of assertions in this Backpack. */
					assertionsCount: number;
		/** DateTime when this Backpack was issued. */
					issuedOn: Date;
		/** Primary key. */
					id: number;
		/** The VC @id. */
					identifier: string;
		/** Complete JSON of the VC. */
					json: string;
		/** Issuer of the VC. */
					provider: string;
		/** Optional name of VC. Primarily for self-published VCs. */
					name: string;
					revoked: boolean;
					revocationReason: string;
					parentCredentialPackage: any;
					badgrAssertions: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				verifiableCredential: server.verifiableCredentialModel;
				name: string;
		/** This Package is tied to a specific application user. */
				userId: string;
				user: server.applicationUser;
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
				containedClrs: any[];
			};
			parentVerifiableCredential: server.verifiableCredentialModel;
			parentClrSet: {
				id: number;
				parentCredentialPackageId?: number;
				parentVerifiableCredentialId?: number;
				clrsCount: number;
				identifier: string;
				json: string;
				parentCredentialPackage: {
		/** Primary key */
					id: number;
					typeId: any;
					assertionsCount: number;
					revoked: boolean;
					revocationReason: string;
		/** Foreign key back to the authorization. */
					authorizationForeignKey: string;
					authorization: server.authorizationModel;
					clr: any;
					clrSet: any;
					badgrBackpack: {
						parentCredentialPackageId: number;
		/** Number of assertions in this Backpack. */
						assertionsCount: number;
		/** DateTime when this Backpack was issued. */
						issuedOn: Date;
		/** Primary key. */
						id: number;
		/** The VC @id. */
						identifier: string;
		/** Complete JSON of the VC. */
						json: string;
		/** Issuer of the VC. */
						provider: string;
		/** Optional name of VC. Primarily for self-published VCs. */
						name: string;
						revoked: boolean;
						revocationReason: string;
						parentCredentialPackage: any;
						badgrAssertions: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					verifiableCredential: server.verifiableCredentialModel;
					name: string;
		/** This Package is tied to a specific application user. */
					userId: string;
					user: server.applicationUser;
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
					containedClrs: any[];
				};
				parentVerifiableCredential: server.verifiableCredentialModel;
				clrs: any[];
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
			};
			publisher: server.profileModel;
			learner: server.profileModel;
			verification: server.verificationModel;
			clrAssertions: any[];
			clrAchievements: any[];
			clrEndorsements: any[];
		};
		endorsement: server.endorsementModel;
		profile: server.profileModel;
	}
}
