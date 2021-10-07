declare module server {
	/** Represents an Profile entity in the CLR model. */
	interface profileModel {
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
		/** End From ProfileDTypeForeignKeys */
		parentProfileId?: number;
		verificationId?: number;
		/** Relationships */
		parentOrg: {
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
		/** End From ProfileDTypeForeignKeys */
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
					publisher: any;
					learner: any;
					verification: any;
					clrAssertions: any[];
					clrAchievements: any[];
					clrEndorsements: any[];
				};
				endorsement: {
					signedEndorsement: string;
		/** Primary key. */
					endorsementId: number;
		/** IBaseEntity properties */
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
					isSigned: boolean;
		/** Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString. */
					id: string;
		/** The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString. */
					type: string;
		/** Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime. */
					issuedOn: Date;
		/** If revoked, optional reason for revocation. Model Primitive Datatype = String. */
					revocationReason: string;
		/** If True the endorsement is revoked. Model Primitive Datatype = Boolean. */
					revoked?: boolean;
		/** Additional properties of the object */
					additionalProperties: { [index: string]: any };
		/** End From EndorsementDTypeForeignKeys */
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
		/** Primary key. */
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
									publisher: any;
									learner: any;
									verification: any;
									clrAssertions: any[];
									clrAchievements: any[];
									clrEndorsements: any[];
								};
								achievement: any;
							};
							assertion: server.assertionModel;
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
		/** End From CriteriaDTypeRelationships */
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
						assertion: server.assertionModel;
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
							publisher: any;
							learner: any;
							verification: any;
							clrAssertions: any[];
							clrAchievements: any[];
							clrEndorsements: any[];
						};
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
		/** End From EndorsementClaimDTypeRelationships */
						endorsement: any;
					};
					issuer: any;
					verification: any;
				};
				profile: any;
			};
		};
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
		/** End From ProfileDTypeForeignKeys */
					parentProfileId?: number;
					verificationId?: number;
		/** Relationships */
					parentOrg: any;
					childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
					profileEndorsements: any[];
					verification: any;
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
		/** End From ProfileDTypeForeignKeys */
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
				clrAssertions: any[];
				clrAchievements: any[];
				clrEndorsements: any[];
			};
			endorsement: {
				signedEndorsement: string;
		/** Primary key. */
				endorsementId: number;
		/** IBaseEntity properties */
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
				isSigned: boolean;
		/** Globally unique IRI for the Endorsement. If this Endorsement will be verified using Hosted verification, the value should be the URL of the hosted version of the Endorsement. Model Primitive Datatype = NormalizedString. */
				id: string;
		/** The JSON-LD type of this entity. Normally 'Endorsement'. Model Primitive Datatype = NormalizedString. */
				type: string;
		/** Timestamp of when the endorsement was published. Model Primitive Datatype = DateTime. */
				issuedOn: Date;
		/** If revoked, optional reason for revocation. Model Primitive Datatype = String. */
				revocationReason: string;
		/** If True the endorsement is revoked. Model Primitive Datatype = Boolean. */
				revoked?: boolean;
		/** Additional properties of the object */
				additionalProperties: { [index: string]: any };
		/** End From EndorsementDTypeForeignKeys */
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
		/** Primary key. */
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
		/** End From ProfileDTypeForeignKeys */
									parentProfileId?: number;
									verificationId?: number;
		/** Relationships */
									parentOrg: any;
									childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
									profileEndorsements: any[];
									verification: any;
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
		/** End From ProfileDTypeForeignKeys */
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
								clrAssertions: any[];
								clrAchievements: any[];
								clrEndorsements: any[];
							};
							achievement: any;
						};
						assertion: server.assertionModel;
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
		/** End From ProfileDTypeForeignKeys */
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
		/** End From CriteriaDTypeRelationships */
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
					assertion: server.assertionModel;
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
		/** End From ProfileDTypeForeignKeys */
							parentProfileId?: number;
							verificationId?: number;
		/** Relationships */
							parentOrg: any;
							childrenOrgs: any[];
		/** public virtual AchievementModel Achievement { get; set; }public virtual AssertionModel Assertion { get; set; }public virtual ClrModel Clr { get; set; } */
							profileEndorsements: any[];
							verification: any;
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
		/** End From ProfileDTypeForeignKeys */
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
						clrAssertions: any[];
						clrAchievements: any[];
						clrEndorsements: any[];
					};
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
		/** End From ProfileDTypeForeignKeys */
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
		/** End From EndorsementClaimDTypeRelationships */
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
		/** End From ProfileDTypeForeignKeys */
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
		/** End From ProfileDTypeForeignKeys */
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
	}
}
