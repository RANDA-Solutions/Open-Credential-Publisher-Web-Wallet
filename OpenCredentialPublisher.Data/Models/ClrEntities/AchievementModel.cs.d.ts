declare module server {
	interface achievementModel {
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
				publisher: server.profileModel;
				learner: server.profileModel;
				verification: server.verificationModel;
				clrAssertions: any[];
				clrAchievements: any[];
				clrEndorsements: any[];
			};
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
				clrAchievement: any;
				assertion: server.assertionModel;
				achievementAlignments: any[];
				achievementAssociations: any[];
				resultDescriptions: any[];
				achievementEndorsements: any[];
				issuer: server.profileModel;
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
		};
		assertion: server.assertionModel;
		achievementAlignments: any[];
		achievementAssociations: any[];
		resultDescriptions: any[];
		achievementEndorsements: any[];
		issuer: server.profileModel;
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
						publisher: server.profileModel;
						learner: server.profileModel;
						verification: server.verificationModel;
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
				issuer: server.profileModel;
				requirement: any;
			};
		};
	}
}