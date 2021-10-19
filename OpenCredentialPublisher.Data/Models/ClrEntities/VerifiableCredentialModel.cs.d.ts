declare module server {
	/** Represents a Verifiable Credential for an application user. The complete Verifiable Credential is stored as JSON. */
	interface verifiableCredentialModel {
		parentCredentialPackageId: number;
		/** Number of credentials in this VC. */
		credentialsCount: number;
		/** DateTime when this VC was issued. */
		issuedOn: Date;
		/** Primary key. */
		id: number;
		/** The VC @id. */
		identifier: string;
		/** Complete JSON of the VC. */
		json: string;
		/** Issuer of the VC. */
		issuer: string;
		/** Optional name of VC. Primarily for self-published VCs. */
		name: string;
		revoked: boolean;
		revocationReason: string;
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
				credentialPackage: any;
		/** possible Parents */
				parentCredentialPackage: any;
				parentVerifiableCredential: server.verifiableCredentialModel;
				parentClrSet: {
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
				publisher: {
		/** Primary key. */
					profileId: number;
					isEndorsementProfile: boolean;
		/** IBaseEntity properties */
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
					id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
					type: string;
		/** Gets or Sets Address */
					address: {
						id: string;
						type: string;
						addressCountry: string;
						addressLocality: string;
						addressRegion: string;
						postalCode: string;
						postOfficeBoxNumber: string;
						streetAddress: string;
						additionalProperties: { [index: string]: any };
					};
					additionalName: string;
		/** NOT on EndorsementProfileDType */
					birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
					description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
					email: string;
					familyName: string;
					givenName: string;
					identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
					image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
					name: string;
					official: string;
		/** Gets or Sets PublicKey */
					publicKey: {
						context: string;
						id: string;
						type: string;
						owner: string;
						publicKeyPem: string;
						additionalProperties: { [index: string]: any };
					};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
					revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
					sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
					studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
					telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
					url: string;
		/** Additional properties of the object */
					additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
					parentProfileId?: number;
					verificationId?: number;
		/** Relationships */
					parentOrg: any;
					childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
					profileEndorsements: any[];
					verification: {
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
		/** * End From VerificationDType******************************************************************************************** */
						assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
							context: string;
							isSigned: boolean;
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
								clr: any;
								assertion: any;
							};
							assertionEvidences: any[];
							achievement: {
		/** /// Primary key./// */
								achievementId: number;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
								achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
								creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
								description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
								humanCode: string;
								identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
								name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
								fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
								image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
								level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
								criteriaId?: number;
		/** Relationships */
								clrAchievement: {
									clrAchievementId: number;
									clrId: number;
									achievementId: number;
									order: number;
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Relationships */
									clr: any;
									achievement: any;
								};
								assertion: any;
								achievementAlignments: any[];
								achievementAssociations: any[];
								resultDescriptions: any[];
								achievementEndorsements: any[];
								issuer: any;
								requirement: {
		/** Primary key. */
									criteriaId: number;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
									narrative: string;
		/** Additional properties of the object */
									additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
									achievement: any;
								};
							};
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
		/** * End From IdentityDType********************************************************************************************Relationships */
								assertion: any;
							};
							results: any[];
							assertionEndorsements: any[];
							source: any;
							verification: any;
						};
						clr: any;
						endorsement: {
							signedEndorsement: string;
		/** /// Primary key./// */
							endorsementId: number;
		/** IBaseEntity properties */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
							isSigned: boolean;
		/** /// Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString./// */
							id: string;
		/** /// The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString./// */
							type: string;
		/** /// Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime./// */
							issuedOn: Date;
		/** /// If revoked, optional reason for revocation. Model Primitive Datatype = String./// */
							revocationReason: string;
		/** /// If True the endorsement is revoked. Model Primitive Datatype = Boolean./// */
							revoked?: boolean;
		/** /// Additional properties of the object/// */
							additionalProperties: { [index: string]: any };
		/** * End From EndorsementDType********************************************************************************************ForeignKeys */
							issuerId: number;
							verificationId: number;
							endorsementClaimId: number;
		/** Relationships */
							achievementEndorsement: {
								achievementEndorsementId: number;
								achievementId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								achievement: {
		/** /// Primary key./// */
									achievementId: number;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
									achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
									creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
									description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
									humanCode: string;
									identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
									name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
									fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
									image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
									level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
									criteriaId?: number;
		/** Relationships */
									clrAchievement: {
										clrAchievementId: number;
										clrId: number;
										achievementId: number;
										order: number;
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Relationships */
										clr: any;
										achievement: any;
									};
									assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
										context: string;
										isSigned: boolean;
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
											clr: any;
											assertion: any;
										};
										assertionEvidences: any[];
										achievement: any;
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
		/** * End From IdentityDType********************************************************************************************Relationships */
											assertion: any;
										};
										results: any[];
										assertionEndorsements: any[];
										source: any;
										verification: any;
									};
									achievementAlignments: any[];
									achievementAssociations: any[];
									resultDescriptions: any[];
									achievementEndorsements: any[];
									issuer: any;
									requirement: {
		/** Primary key. */
										criteriaId: number;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
										narrative: string;
		/** Additional properties of the object */
										additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
										achievement: any;
									};
								};
								endorsement: any;
							};
							assertionEndorsement: {
								assertionEndorsementId: number;
								assertionId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
									context: string;
									isSigned: boolean;
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
										clr: any;
										assertion: any;
									};
									assertionEvidences: any[];
									achievement: {
		/** /// Primary key./// */
										achievementId: number;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
										achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
										creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
										description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
										humanCode: string;
										identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
										name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
										fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
										image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
										level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
										specialization: string;
										tags: string[];
										additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
										criteriaId?: number;
		/** Relationships */
										clrAchievement: {
											clrAchievementId: number;
											clrId: number;
											achievementId: number;
											order: number;
											isDeleted: boolean;
											createdAt: Date;
											modifiedAt: Date;
		/** Relationships */
											clr: any;
											achievement: any;
										};
										assertion: any;
										achievementAlignments: any[];
										achievementAssociations: any[];
										resultDescriptions: any[];
										achievementEndorsements: any[];
										issuer: any;
										requirement: {
		/** Primary key. */
											criteriaId: number;
		/** IBaseEntity properties */
											isDeleted: boolean;
											createdAt: Date;
											modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
											id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
											type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
											narrative: string;
		/** Additional properties of the object */
											additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
											achievement: any;
										};
									};
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
		/** * End From IdentityDType********************************************************************************************Relationships */
										assertion: any;
									};
									results: any[];
									assertionEndorsements: any[];
									source: any;
									verification: any;
								};
								endorsement: any;
							};
							clrEndorsement: {
								clrEndorsementId: number;
								clrId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								clr: any;
								endorsement: any;
							};
							profileEndorsement: {
								profileEndorsementId: number;
								profileId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								profile: any;
								endorsement: any;
							};
							endorsementClaim: {
		/** Primary key. */
								endorsementClaimId: number;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** An endorser's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String. */
								endorsementComment: string;
		/** Additional properties of the object */
								additionalProperties: { [index: string]: any };
		/** * End From EndorsementClaimDType********************************************************************************************Relationships */
								endorsement: any;
							};
							issuer: any;
							verification: any;
						};
						profile: any;
					};
				};
				learner: {
		/** Primary key. */
					profileId: number;
					isEndorsementProfile: boolean;
		/** IBaseEntity properties */
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
					id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
					type: string;
		/** Gets or Sets Address */
					address: {
						id: string;
						type: string;
						addressCountry: string;
						addressLocality: string;
						addressRegion: string;
						postalCode: string;
						postOfficeBoxNumber: string;
						streetAddress: string;
						additionalProperties: { [index: string]: any };
					};
					additionalName: string;
		/** NOT on EndorsementProfileDType */
					birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
					description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
					email: string;
					familyName: string;
					givenName: string;
					identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
					image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
					name: string;
					official: string;
		/** Gets or Sets PublicKey */
					publicKey: {
						context: string;
						id: string;
						type: string;
						owner: string;
						publicKeyPem: string;
						additionalProperties: { [index: string]: any };
					};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
					revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
					sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
					studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
					telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
					url: string;
		/** Additional properties of the object */
					additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
					parentProfileId?: number;
					verificationId?: number;
		/** Relationships */
					parentOrg: any;
					childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
					profileEndorsements: any[];
					verification: {
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
		/** * End From VerificationDType******************************************************************************************** */
						assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
							context: string;
							isSigned: boolean;
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
								clr: any;
								assertion: any;
							};
							assertionEvidences: any[];
							achievement: {
		/** /// Primary key./// */
								achievementId: number;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
								achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
								creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
								description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
								humanCode: string;
								identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
								name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
								fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
								image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
								level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
								criteriaId?: number;
		/** Relationships */
								clrAchievement: {
									clrAchievementId: number;
									clrId: number;
									achievementId: number;
									order: number;
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Relationships */
									clr: any;
									achievement: any;
								};
								assertion: any;
								achievementAlignments: any[];
								achievementAssociations: any[];
								resultDescriptions: any[];
								achievementEndorsements: any[];
								issuer: any;
								requirement: {
		/** Primary key. */
									criteriaId: number;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
									narrative: string;
		/** Additional properties of the object */
									additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
									achievement: any;
								};
							};
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
		/** * End From IdentityDType********************************************************************************************Relationships */
								assertion: any;
							};
							results: any[];
							assertionEndorsements: any[];
							source: any;
							verification: any;
						};
						clr: any;
						endorsement: {
							signedEndorsement: string;
		/** /// Primary key./// */
							endorsementId: number;
		/** IBaseEntity properties */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
							isSigned: boolean;
		/** /// Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString./// */
							id: string;
		/** /// The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString./// */
							type: string;
		/** /// Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime./// */
							issuedOn: Date;
		/** /// If revoked, optional reason for revocation. Model Primitive Datatype = String./// */
							revocationReason: string;
		/** /// If True the endorsement is revoked. Model Primitive Datatype = Boolean./// */
							revoked?: boolean;
		/** /// Additional properties of the object/// */
							additionalProperties: { [index: string]: any };
		/** * End From EndorsementDType********************************************************************************************ForeignKeys */
							issuerId: number;
							verificationId: number;
							endorsementClaimId: number;
		/** Relationships */
							achievementEndorsement: {
								achievementEndorsementId: number;
								achievementId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								achievement: {
		/** /// Primary key./// */
									achievementId: number;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
									achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
									creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
									description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
									humanCode: string;
									identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
									name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
									fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
									image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
									level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
									criteriaId?: number;
		/** Relationships */
									clrAchievement: {
										clrAchievementId: number;
										clrId: number;
										achievementId: number;
										order: number;
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Relationships */
										clr: any;
										achievement: any;
									};
									assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
										context: string;
										isSigned: boolean;
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
											clr: any;
											assertion: any;
										};
										assertionEvidences: any[];
										achievement: any;
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
		/** * End From IdentityDType********************************************************************************************Relationships */
											assertion: any;
										};
										results: any[];
										assertionEndorsements: any[];
										source: any;
										verification: any;
									};
									achievementAlignments: any[];
									achievementAssociations: any[];
									resultDescriptions: any[];
									achievementEndorsements: any[];
									issuer: any;
									requirement: {
		/** Primary key. */
										criteriaId: number;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
										narrative: string;
		/** Additional properties of the object */
										additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
										achievement: any;
									};
								};
								endorsement: any;
							};
							assertionEndorsement: {
								assertionEndorsementId: number;
								assertionId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
									context: string;
									isSigned: boolean;
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
										clr: any;
										assertion: any;
									};
									assertionEvidences: any[];
									achievement: {
		/** /// Primary key./// */
										achievementId: number;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
										achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
										creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
										description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
										humanCode: string;
										identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
										name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
										fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
										image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
										level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
										specialization: string;
										tags: string[];
										additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
										criteriaId?: number;
		/** Relationships */
										clrAchievement: {
											clrAchievementId: number;
											clrId: number;
											achievementId: number;
											order: number;
											isDeleted: boolean;
											createdAt: Date;
											modifiedAt: Date;
		/** Relationships */
											clr: any;
											achievement: any;
										};
										assertion: any;
										achievementAlignments: any[];
										achievementAssociations: any[];
										resultDescriptions: any[];
										achievementEndorsements: any[];
										issuer: any;
										requirement: {
		/** Primary key. */
											criteriaId: number;
		/** IBaseEntity properties */
											isDeleted: boolean;
											createdAt: Date;
											modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
											id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
											type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
											narrative: string;
		/** Additional properties of the object */
											additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
											achievement: any;
										};
									};
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
		/** * End From IdentityDType********************************************************************************************Relationships */
										assertion: any;
									};
									results: any[];
									assertionEndorsements: any[];
									source: any;
									verification: any;
								};
								endorsement: any;
							};
							clrEndorsement: {
								clrEndorsementId: number;
								clrId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								clr: any;
								endorsement: any;
							};
							profileEndorsement: {
								profileEndorsementId: number;
								profileId: number;
								endorsementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								profile: any;
								endorsement: any;
							};
							endorsementClaim: {
		/** Primary key. */
								endorsementClaimId: number;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** An endorser's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String. */
								endorsementComment: string;
		/** Additional properties of the object */
								additionalProperties: { [index: string]: any };
		/** * End From EndorsementClaimDType********************************************************************************************Relationships */
								endorsement: any;
							};
							issuer: any;
							verification: any;
						};
						profile: any;
					};
				};
				verification: {
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
		/** * End From VerificationDType******************************************************************************************** */
					assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
						context: string;
						isSigned: boolean;
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
							clr: any;
							assertion: any;
						};
						assertionEvidences: any[];
						achievement: {
		/** /// Primary key./// */
							achievementId: number;
		/** IBaseEntity properties */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
							id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
							type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
							achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
							creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
							description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
							humanCode: string;
							identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
							name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
							fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
							image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
							level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
							criteriaId?: number;
		/** Relationships */
							clrAchievement: {
								clrAchievementId: number;
								clrId: number;
								achievementId: number;
								order: number;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Relationships */
								clr: any;
								achievement: any;
							};
							assertion: any;
							achievementAlignments: any[];
							achievementAssociations: any[];
							resultDescriptions: any[];
							achievementEndorsements: any[];
							issuer: {
		/** Primary key. */
								profileId: number;
								isEndorsementProfile: boolean;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** Gets or Sets Address */
								address: {
									id: string;
									type: string;
									addressCountry: string;
									addressLocality: string;
									addressRegion: string;
									postalCode: string;
									postOfficeBoxNumber: string;
									streetAddress: string;
									additionalProperties: { [index: string]: any };
								};
								additionalName: string;
		/** NOT on EndorsementProfileDType */
								birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
								description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
								image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
								name: string;
								official: string;
		/** Gets or Sets PublicKey */
								publicKey: {
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
								revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
								sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
								studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
								telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
								url: string;
		/** Additional properties of the object */
								additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
								parentProfileId?: number;
								verificationId?: number;
		/** Relationships */
								parentOrg: any;
								childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
								profileEndorsements: any[];
								verification: any;
							};
							requirement: {
		/** Primary key. */
								criteriaId: number;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
								id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
								narrative: string;
		/** Additional properties of the object */
								additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
								achievement: any;
							};
						};
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
		/** * End From IdentityDType********************************************************************************************Relationships */
							assertion: any;
						};
						results: any[];
						assertionEndorsements: any[];
						source: {
		/** Primary key. */
							profileId: number;
							isEndorsementProfile: boolean;
		/** IBaseEntity properties */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
							id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
							type: string;
		/** Gets or Sets Address */
							address: {
								id: string;
								type: string;
								addressCountry: string;
								addressLocality: string;
								addressRegion: string;
								postalCode: string;
								postOfficeBoxNumber: string;
								streetAddress: string;
								additionalProperties: { [index: string]: any };
							};
							additionalName: string;
		/** NOT on EndorsementProfileDType */
							birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
							description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
							image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
							name: string;
							official: string;
		/** Gets or Sets PublicKey */
							publicKey: {
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
							revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
							sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
							studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
							telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
							url: string;
		/** Additional properties of the object */
							additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
							parentProfileId?: number;
							verificationId?: number;
		/** Relationships */
							parentOrg: any;
							childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
							profileEndorsements: any[];
							verification: any;
						};
						verification: any;
					};
					clr: any;
					endorsement: {
						signedEndorsement: string;
		/** /// Primary key./// */
						endorsementId: number;
		/** IBaseEntity properties */
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
						isSigned: boolean;
		/** /// Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString./// */
						id: string;
		/** /// The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString./// */
						type: string;
		/** /// Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime./// */
						issuedOn: Date;
		/** /// If revoked, optional reason for revocation. Model Primitive Datatype = String./// */
						revocationReason: string;
		/** /// If True the endorsement is revoked. Model Primitive Datatype = Boolean./// */
						revoked?: boolean;
		/** /// Additional properties of the object/// */
						additionalProperties: { [index: string]: any };
		/** * End From EndorsementDType********************************************************************************************ForeignKeys */
						issuerId: number;
						verificationId: number;
						endorsementClaimId: number;
		/** Relationships */
						achievementEndorsement: {
							achievementEndorsementId: number;
							achievementId: number;
							endorsementId: number;
							order: number;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Relationships */
							achievement: {
		/** /// Primary key./// */
								achievementId: number;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
								achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
								creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
								description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
								humanCode: string;
								identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
								name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
								fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
								image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
								level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
								criteriaId?: number;
		/** Relationships */
								clrAchievement: {
									clrAchievementId: number;
									clrId: number;
									achievementId: number;
									order: number;
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Relationships */
									clr: any;
									achievement: any;
								};
								assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
									context: string;
									isSigned: boolean;
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
										clr: any;
										assertion: any;
									};
									assertionEvidences: any[];
									achievement: any;
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
		/** * End From IdentityDType********************************************************************************************Relationships */
										assertion: any;
									};
									results: any[];
									assertionEndorsements: any[];
									source: {
		/** Primary key. */
										profileId: number;
										isEndorsementProfile: boolean;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** Gets or Sets Address */
										address: {
											id: string;
											type: string;
											addressCountry: string;
											addressLocality: string;
											addressRegion: string;
											postalCode: string;
											postOfficeBoxNumber: string;
											streetAddress: string;
											additionalProperties: { [index: string]: any };
										};
										additionalName: string;
		/** NOT on EndorsementProfileDType */
										birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
										description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
										email: string;
										familyName: string;
										givenName: string;
										identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
										image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
										name: string;
										official: string;
		/** Gets or Sets PublicKey */
										publicKey: {
											context: string;
											id: string;
											type: string;
											owner: string;
											publicKeyPem: string;
											additionalProperties: { [index: string]: any };
										};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
										revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
										sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
										studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
										telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
										url: string;
		/** Additional properties of the object */
										additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
										parentProfileId?: number;
										verificationId?: number;
		/** Relationships */
										parentOrg: any;
										childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
										profileEndorsements: any[];
										verification: any;
									};
									verification: any;
								};
								achievementAlignments: any[];
								achievementAssociations: any[];
								resultDescriptions: any[];
								achievementEndorsements: any[];
								issuer: {
		/** Primary key. */
									profileId: number;
									isEndorsementProfile: boolean;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** Gets or Sets Address */
									address: {
										id: string;
										type: string;
										addressCountry: string;
										addressLocality: string;
										addressRegion: string;
										postalCode: string;
										postOfficeBoxNumber: string;
										streetAddress: string;
										additionalProperties: { [index: string]: any };
									};
									additionalName: string;
		/** NOT on EndorsementProfileDType */
									birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
									description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
									email: string;
									familyName: string;
									givenName: string;
									identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
									image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
									name: string;
									official: string;
		/** Gets or Sets PublicKey */
									publicKey: {
										context: string;
										id: string;
										type: string;
										owner: string;
										publicKeyPem: string;
										additionalProperties: { [index: string]: any };
									};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
									revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
									sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
									studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
									telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
									url: string;
		/** Additional properties of the object */
									additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
									parentProfileId?: number;
									verificationId?: number;
		/** Relationships */
									parentOrg: any;
									childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
									profileEndorsements: any[];
									verification: any;
								};
								requirement: {
		/** Primary key. */
									criteriaId: number;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
									narrative: string;
		/** Additional properties of the object */
									additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
									achievement: any;
								};
							};
							endorsement: any;
						};
						assertionEndorsement: {
							assertionEndorsementId: number;
							assertionId: number;
							endorsementId: number;
							order: number;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Relationships */
							assertion: {
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
		/** * End From AssertionDType******************************************************************************************** */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
								context: string;
								isSigned: boolean;
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
									clr: any;
									assertion: any;
								};
								assertionEvidences: any[];
								achievement: {
		/** /// Primary key./// */
									achievementId: number;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Unique IRI for the Achievement. If the id is a URL it must be the location of an Achievement document. Model Primitive Datatype = NormalizedString. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Achievement'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** A CLR Achievement can represent many different types of achievement from an assessment result to membership. Use 'Achievement' to indicate an achievement not in the list of allowed values. */
									achievementType: string;
		/** Credit hours associated with this entity, or credit hours possible. For example '3.0'. Model Primitive Datatype = Float. */
									creditsAvailable?: number;
		/** A short description of the achievement. May be the same as name if no description is available. Model Primitive Datatype = String. */
									description: string;
		/** The code, generally human readable, associated with an achievement. Model Primitive Datatype = String. */
									humanCode: string;
									identifiers: any[];
		/** The name of the achievement. Model Primitive Datatype = String. */
									name: string;
		/** Category, subject, area of study,  discipline, or general branch of knowledge. Examples include Business, Education, Psychology, and Technology.  Model Primitive Datatype = String. */
									fieldOfStudy: string;
		/** IRI of an image representing the achievement. May be a Data URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
									image: string;
		/** Text that describes the level of achievement apart from how the achievement was performed or demonstrated. Examples would include 'Level 1', 'Level 2', 'Level 3', or 'Bachelors', 'Masters', 'Doctoral'. Model Primitive Datatype = String. */
									level: string;
		/** Name given to the focus, concentration, or specific area of study defined in the achievement. Examples include Entrepreneurship, Technical Communication, and Finance. Model Primitive Datatype = String. */
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
		/** End From AchievementDTypeForeignKeys */
									criteriaId?: number;
		/** Relationships */
									clrAchievement: {
										clrAchievementId: number;
										clrId: number;
										achievementId: number;
										order: number;
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Relationships */
										clr: any;
										achievement: any;
									};
									assertion: any;
									achievementAlignments: any[];
									achievementAssociations: any[];
									resultDescriptions: any[];
									achievementEndorsements: any[];
									issuer: {
		/** Primary key. */
										profileId: number;
										isEndorsementProfile: boolean;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** Gets or Sets Address */
										address: {
											id: string;
											type: string;
											addressCountry: string;
											addressLocality: string;
											addressRegion: string;
											postalCode: string;
											postOfficeBoxNumber: string;
											streetAddress: string;
											additionalProperties: { [index: string]: any };
										};
										additionalName: string;
		/** NOT on EndorsementProfileDType */
										birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
										description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
										email: string;
										familyName: string;
										givenName: string;
										identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
										image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
										name: string;
										official: string;
		/** Gets or Sets PublicKey */
										publicKey: {
											context: string;
											id: string;
											type: string;
											owner: string;
											publicKeyPem: string;
											additionalProperties: { [index: string]: any };
										};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
										revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
										sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
										studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
										telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
										url: string;
		/** Additional properties of the object */
										additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
										parentProfileId?: number;
										verificationId?: number;
		/** Relationships */
										parentOrg: any;
										childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
										profileEndorsements: any[];
										verification: any;
									};
									requirement: {
		/** Primary key. */
										criteriaId: number;
		/** IBaseEntity properties */
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
										id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
										type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
										narrative: string;
		/** Additional properties of the object */
										additionalProperties: { [index: string]: any };
		/** * End From CriteriaDType********************************************************************************************Relationships */
										achievement: any;
									};
								};
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
		/** * End From IdentityDType********************************************************************************************Relationships */
									assertion: any;
								};
								results: any[];
								assertionEndorsements: any[];
								source: {
		/** Primary key. */
									profileId: number;
									isEndorsementProfile: boolean;
		/** IBaseEntity properties */
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
									id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
									type: string;
		/** Gets or Sets Address */
									address: {
										id: string;
										type: string;
										addressCountry: string;
										addressLocality: string;
										addressRegion: string;
										postalCode: string;
										postOfficeBoxNumber: string;
										streetAddress: string;
										additionalProperties: { [index: string]: any };
									};
									additionalName: string;
		/** NOT on EndorsementProfileDType */
									birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
									description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
									email: string;
									familyName: string;
									givenName: string;
									identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
									image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
									name: string;
									official: string;
		/** Gets or Sets PublicKey */
									publicKey: {
										context: string;
										id: string;
										type: string;
										owner: string;
										publicKeyPem: string;
										additionalProperties: { [index: string]: any };
									};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
									revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
									sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
									studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
									telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
									url: string;
		/** Additional properties of the object */
									additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
									parentProfileId?: number;
									verificationId?: number;
		/** Relationships */
									parentOrg: any;
									childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
									profileEndorsements: any[];
									verification: any;
								};
								verification: any;
							};
							endorsement: any;
						};
						clrEndorsement: {
							clrEndorsementId: number;
							clrId: number;
							endorsementId: number;
							order: number;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Relationships */
							clr: any;
							endorsement: any;
						};
						profileEndorsement: {
							profileEndorsementId: number;
							profileId: number;
							endorsementId: number;
							order: number;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Relationships */
							profile: {
		/** Primary key. */
								profileId: number;
								isEndorsementProfile: boolean;
		/** IBaseEntity properties */
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
								id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
								type: string;
		/** Gets or Sets Address */
								address: {
									id: string;
									type: string;
									addressCountry: string;
									addressLocality: string;
									addressRegion: string;
									postalCode: string;
									postOfficeBoxNumber: string;
									streetAddress: string;
									additionalProperties: { [index: string]: any };
								};
								additionalName: string;
		/** NOT on EndorsementProfileDType */
								birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
								description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
								image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
								name: string;
								official: string;
		/** Gets or Sets PublicKey */
								publicKey: {
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
								revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
								sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
								studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
								telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
								url: string;
		/** Additional properties of the object */
								additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
								parentProfileId?: number;
								verificationId?: number;
		/** Relationships */
								parentOrg: any;
								childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
								profileEndorsements: any[];
								verification: any;
							};
							endorsement: any;
						};
						endorsementClaim: {
		/** Primary key. */
							endorsementClaimId: number;
		/** IBaseEntity properties */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** The 'id' of the Profile, Achievement, or Assertion being endorsed. Model Primitive Datatype = NormalizedString. */
							id: string;
		/** The JSON-LD type of this entity. Normally 'EndorsementClaim'. Model Primitive Datatype = NormalizedString. */
							type: string;
		/** An endorser's comment about the quality or fitness of the endorsed entity. Markdown allowed. Model Primitive Datatype = String. */
							endorsementComment: string;
		/** Additional properties of the object */
							additionalProperties: { [index: string]: any };
		/** * End From EndorsementClaimDType********************************************************************************************Relationships */
							endorsement: any;
						};
						issuer: {
		/** Primary key. */
							profileId: number;
							isEndorsementProfile: boolean;
		/** IBaseEntity properties */
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
							id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
							type: string;
		/** Gets or Sets Address */
							address: {
								id: string;
								type: string;
								addressCountry: string;
								addressLocality: string;
								addressRegion: string;
								postalCode: string;
								postOfficeBoxNumber: string;
								streetAddress: string;
								additionalProperties: { [index: string]: any };
							};
							additionalName: string;
		/** NOT on EndorsementProfileDType */
							birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
							description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
							image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
							name: string;
							official: string;
		/** Gets or Sets PublicKey */
							publicKey: {
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
							revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
							sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
							studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
							telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
							url: string;
		/** Additional properties of the object */
							additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
							parentProfileId?: number;
							verificationId?: number;
		/** Relationships */
							parentOrg: any;
							childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
							profileEndorsements: any[];
							verification: any;
						};
						verification: any;
					};
					profile: {
		/** Primary key. */
						profileId: number;
						isEndorsementProfile: boolean;
		/** IBaseEntity properties */
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
		/** Unique IRI for the Learner, Publisher, and Issuer Profile document. The Assertion Recipient is identified by reference to the Learner's Profile via the id, email, url, telephone, sourcedId, or studentId property. Model Primitive Datatype = NormalizedString. */
						id: string;
		/** The JSON-LD type of this object. Normally 'Profile'. Model Primitive Datatype = NormalizedString. */
						type: string;
		/** Gets or Sets Address */
						address: {
							id: string;
							type: string;
							addressCountry: string;
							addressLocality: string;
							addressRegion: string;
							postalCode: string;
							postOfficeBoxNumber: string;
							streetAddress: string;
							additionalProperties: { [index: string]: any };
						};
						additionalName: string;
		/** NOT on EndorsementProfileDType */
						birthDate: Date;
		/** A short description of the individual or organization. Model Primitive Datatype = String. */
						description: string;
		/** A contact email address for the individual or organization. Model Primitive Datatype = String. */
						email: string;
						familyName: string;
						givenName: string;
						identifiers: any[];
		/** IRI of an image representing the individual or organization. May be a DATA URI or the URL where the image may be found. Model Primitive Datatype = NormalizedString. */
						image: string;
		/** The name of the individual or organization. Model Primitive Datatype = String. */
						name: string;
						official: string;
		/** Gets or Sets PublicKey */
						publicKey: {
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
		/** The URL of the Revocation List document used for marking revocation of signed Assertions, CLRs, and Endorsements. Required for issuer profiles. Model Primitive Datatype = AnyURI. */
						revocationList: string;
		/** The individual's or organization's unique 'sourcedId' value, which is used for providing interoperability with other identity systems. Model Primitive Datatype = String. */
						sourcedId: string;
		/** An institution's student identifier for the person. This is frequently issued through a Student Information System. Model Primitive Datatype = String. */
						studentId: string;
		/** Primary phone number for the individual or organization. Model Primitive Datatype = String. */
						telephone: string;
		/** Web resource that uniquely represents or belongs to the individual. This may be a resource about the individual, hosting provided by the institution to the individual, or an web resource independently controlled by the individual. Model Primitive Datatype = AnyURI. */
						url: string;
		/** Additional properties of the object */
						additionalProperties: { [index: string]: any };
		/** * End From ProfileDType********************************************************************************************ForeignKeys */
						parentProfileId?: number;
						verificationId?: number;
		/** Relationships */
						parentOrg: any;
						childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
						profileEndorsements: any[];
						verification: any;
					};
				};
				clrAssertions: any[];
				clrAchievements: any[];
				clrEndorsements: any[];
			};
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
		clrSets: any[];
		clrs: any[];
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
	}
}
