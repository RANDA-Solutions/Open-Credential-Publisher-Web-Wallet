declare module server {
	interface assertionModel {
		/** Primary key. */
		assertionId: number;
		signedAssertion: string;
		/** Globally unique IRI for the Assertion. If this Assertion will be verified using Hosted verification, the value should be the URL to the hosted version of this Assertion. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'Assertion'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** The number of credits earned, generally in semester or quarter credit hours.  This field correlates with the Achievement creditsAvailable field. Model Primitive Datatype = Float. */
		creditsEarned?: number;
		/** If present, the assertion is not valid after this date. Model Primitive Datatype = DateTime. */
		activityEndDate?: Date;
		expires?: Date;
		/** IRI of an image representing the assertion. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
		image: string;
		/** Timestamp of when the achievement was awarded. Required unless the assertion is revoked. Model Primitive Datatype = DateTime. */
		issuedOn?: Date;
		/** The license number that was issued with this assertion. Model Primitive Datatype = String. */
		licenseNumber: string;
		/** A narrative that describes the connection between multiple pieces of evidence. Likely only present if evidence is a multi-value array. Markdown allowed. Model Primitive Datatype = String. */
		narrative: string;
		/** Optional published reason for revocation, if revoked. Model Primitive Datatype = String. */
		revocationReason: string;
		/** Defaults to false if this assertion is not referenced in a RevocationList. If revoked is true, only revoked and id are required properties, and many issuers strip a hosted assertion down to only those properties when revoked. Model Primitive Datatype = Boolean. */
		revoked?: boolean;
		/** Role, position, or title of the learner when demonstrating or performing the achievement or evidence of learning being asserted. Examples include 'Student President', 'Intern', 'Captain', etc. Model Primitive Datatype = String. */
		role: string;
		/** Signed endorsements in JWS Compact Serialization format. Model Primitive Datatype = String. */
		signedEndorsements: string[];
		/** If present, the assertion is not valid before this date. Model Primitive Datatype = DateTime. */
		activityStartDate?: Date;
		/** The academic term in which this assertion was achieved. Model Primitive Datatype = String. */
		term: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
		/** End From AssertionDType */
		displayName: string;
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		context: string;
		isSigned: boolean;
		isSelfPublished: boolean;
		json: string;
		/** ForeignKeys */
		sourceId?: number;
		verificationId?: number;
		recipientId?: number;
		achievementId?: number;
		/** Relationships */
		clrAssertion: {
			clrAssertionId: number;
			clrId: number;
			assertionId: number;
			order: number;
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
		/** Relationships */
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
				verificationId?: number;
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
			assertion: server.assertionModel;
		};
		assertionEvidences: any[];
		achievement: server.achievementModel;
		recipient: {
		/** Primary key. */
			identityId: number;
		/** IBaseEntity properties */
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
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
		/** End From IdentityDTypeRelationships */
			assertion: server.assertionModel;
		};
		results: server.resultModel[];
		assertionEndorsements: any[];
		source: server.profileModel;
		verification: server.verificationModel;
		parentAssertionId?: number;
		parentAssertion: server.assertionModel;
		childAssertions: server.assertionModel[];
	}
}
