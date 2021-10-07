export module server {
	interface clrViewModel {
		clr: {
		/** START Actual persisted dataForeign Keys */
			parentCredentialPackageId?: number;
			parentVerifiableCredentialId?: number;
			parentClrSetId?: number;
		/** Foreign key back to the authorization. */
			authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
			assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
			authorization: server.authorizationModel;
		/** DateTime when this CLR was issued. */
			issuedOn: Date;
		/** Primary key. */
			id: number;
		/** The CLR @id. */
			identifier: string;
		/** Complete JSON of the CLR. */
			json: string;
		/** Learner of the CLR. */
			learnerName: string;
		/** All the links tied to this CLR. */
			links: server.linkModel[];
		/** Optional name of CLR. Primarily for self-published CLRs. */
			name: string;
		/** Publisher of the CLR. */
			publisherName: string;
		/** The date and time the CLR was retrieved from the authorization server. */
			refreshedAt: Date;
		/** The Signed CLR if it was signed. */
			signedClr: string;
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
			credentialPackageId: number;
			assertions: any[];
		/** END Actual persisted data */
			credentialPackage: {
		/** Primary key */
				id: number;
				typeId: server.packageTypeEnum;
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
					parentVerifiableCredential: {
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
						parentCredentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
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
				verifiableCredential: {
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
					parentCredentialPackage: any;
					clrSets: any[];
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
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
				typeId: server.packageTypeEnum;
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
					parentVerifiableCredential: {
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
						parentCredentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
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
				verifiableCredential: {
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
					parentCredentialPackage: any;
					clrSets: any[];
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				name: string;
		/** This Package is tied to a specific application user. */
				userId: string;
				user: server.applicationUser;
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
				containedClrs: any[];
			};
			parentVerifiableCredential: {
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
					typeId: server.packageTypeEnum;
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
						parentVerifiableCredential: any;
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
					verifiableCredential: any;
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
			};
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
					typeId: server.packageTypeEnum;
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
					verifiableCredential: {
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
						parentCredentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					name: string;
		/** This Package is tied to a specific application user. */
					userId: string;
					user: server.applicationUser;
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
					containedClrs: any[];
				};
				parentVerifiableCredential: {
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
						typeId: server.packageTypeEnum;
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
						verifiableCredential: any;
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
				};
				clrs: any[];
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
			};
		/** true indicates this CLR's Id was received in a revocation list from the source */
			isRevoked: boolean;
		};
		/** The raw ClrDType hydrated from JSON . */
		rawClrDType: {
			context: string;
			id: string;
			type: string;
			achievements: any[];
			assertions: any[];
			issuedOn: Date;
			learner: {
				id: string;
				type: string;
				address: {
					addressKey: number;
					endorsementProfileKey?: number;
					endorsementProfile: {
						id: string;
						type: string;
						additionalName: string;
						address: any;
						description: string;
						email: string;
						familyName: string;
						givenName: string;
						identifiers: any[];
						image: string;
						name: string;
						official: string;
						publicKey: {
							cryptographicKeyKey: number;
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
						revocationList: string;
						sourcedId: string;
						studentId: string;
						telephone: string;
						url: string;
						verification: {
							verificationKey: number;
							assertionKey?: number;
							assertion: {
								assertionKey: number;
								isSigned: boolean;
								context: string;
								assertionClrs: any[];
								id: string;
								type: string;
								achievement: {
									achievementKey: number;
									achievementClrs: any[];
									id: string;
									type: string;
									achievementType: string;
									alignments: any[];
									associations: any[];
									creditsAvailable?: number;
									description: string;
									endorsements: any[];
									humanCode: string;
									identifiers: any[];
									name: string;
									fieldOfStudy: string;
									image: string;
									issuer: any;
									level: string;
									requirement: {
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
								};
								creditsEarned?: number;
								activityEndDate?: Date;
								endorsements: any[];
								evidence: any[];
								expires?: Date;
								image: string;
								issuedOn?: Date;
								licenseNumber: string;
								narrative: string;
								recipient: {
									identityKey: number;
									id: string;
									type: string;
									identity: string;
									hashed: boolean;
									salt: string;
									additionalProperties: { [index: string]: any };
								};
								results: any[];
								revocationReason: string;
								revoked?: boolean;
								role: string;
								signedEndorsements: string[];
								source: any;
								activityStartDate?: Date;
								term: string;
								verification: any;
								additionalProperties: { [index: string]: any };
							};
							clrKey?: number;
							clr: any;
							endorsementKey?: number;
							endorsement: {
								endorsementKey: number;
								context: string;
								achievementKey?: number;
								achievement: {
									achievementKey: number;
									achievementClrs: any[];
									id: string;
									type: string;
									achievementType: string;
									alignments: any[];
									associations: any[];
									creditsAvailable?: number;
									description: string;
									endorsements: any[];
									humanCode: string;
									identifiers: any[];
									name: string;
									fieldOfStudy: string;
									image: string;
									issuer: any;
									level: string;
									requirement: {
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
								};
								assertionKey?: number;
								assertion: {
									assertionKey: number;
									isSigned: boolean;
									context: string;
									assertionClrs: any[];
									id: string;
									type: string;
									achievement: {
										achievementKey: number;
										achievementClrs: any[];
										id: string;
										type: string;
										achievementType: string;
										alignments: any[];
										associations: any[];
										creditsAvailable?: number;
										description: string;
										endorsements: any[];
										humanCode: string;
										identifiers: any[];
										name: string;
										fieldOfStudy: string;
										image: string;
										issuer: any;
										level: string;
										requirement: {
											criteriaKey: number;
											achievementKey?: number;
											achievement: any;
											id: string;
											type: string;
											narrative: string;
											additionalProperties: { [index: string]: any };
										};
										resultDescriptions: any[];
										signedEndorsements: string[];
										specialization: string;
										tags: string[];
										additionalProperties: { [index: string]: any };
									};
									creditsEarned?: number;
									activityEndDate?: Date;
									endorsements: any[];
									evidence: any[];
									expires?: Date;
									image: string;
									issuedOn?: Date;
									licenseNumber: string;
									narrative: string;
									recipient: {
										identityKey: number;
										id: string;
										type: string;
										identity: string;
										hashed: boolean;
										salt: string;
										additionalProperties: { [index: string]: any };
									};
									results: any[];
									revocationReason: string;
									revoked?: boolean;
									role: string;
									signedEndorsements: string[];
									source: any;
									activityStartDate?: Date;
									term: string;
									verification: any;
									additionalProperties: { [index: string]: any };
								};
								profileKey?: number;
								profile: any;
								id: string;
								type: string;
								claim: {
									id: string;
									type: string;
									endorsementComment: string;
									additionalProperties: { [index: string]: any };
									endorsementClaimKey: number;
									endorsementKey: number;
									endorsement: any;
								};
								issuedOn: Date;
								issuer: any;
								revocationReason: string;
								revoked?: boolean;
								verification: any;
								additionalProperties: { [index: string]: any };
							};
							profileKey?: number;
							profile: any;
							id: string;
							type: any;
							allowedOrigins: string[];
							creator: string;
							startsWith: string[];
							verificationProperty: string;
							additionalProperties: { [index: string]: any };
						};
						additionalProperties: { [index: string]: any };
						endorsementProfileKey: number;
						profileKey?: number;
						profile: any;
					};
					profileKey?: number;
					profile: any;
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
				birthDate: Date;
				description: string;
				email: string;
				endorsements: any[];
				familyName: string;
				givenName: string;
				identifiers: any[];
				image: string;
				name: string;
				official: string;
				parentOrg: any;
				publicKey: {
					cryptographicKeyKey: number;
					context: string;
					id: string;
					type: string;
					owner: string;
					publicKeyPem: string;
					additionalProperties: { [index: string]: any };
				};
				revocationList: string;
				signedEndorsements: string[];
				sourcedId: string;
				studentId: string;
				telephone: string;
				url: string;
				verification: {
					verificationKey: number;
					assertionKey?: number;
					assertion: {
						assertionKey: number;
						isSigned: boolean;
						context: string;
						assertionClrs: any[];
						id: string;
						type: string;
						achievement: {
							achievementKey: number;
							achievementClrs: any[];
							id: string;
							type: string;
							achievementType: string;
							alignments: any[];
							associations: any[];
							creditsAvailable?: number;
							description: string;
							endorsements: any[];
							humanCode: string;
							identifiers: any[];
							name: string;
							fieldOfStudy: string;
							image: string;
							issuer: any;
							level: string;
							requirement: {
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
						};
						creditsEarned?: number;
						activityEndDate?: Date;
						endorsements: any[];
						evidence: any[];
						expires?: Date;
						image: string;
						issuedOn?: Date;
						licenseNumber: string;
						narrative: string;
						recipient: {
							identityKey: number;
							id: string;
							type: string;
							identity: string;
							hashed: boolean;
							salt: string;
							additionalProperties: { [index: string]: any };
						};
						results: any[];
						revocationReason: string;
						revoked?: boolean;
						role: string;
						signedEndorsements: string[];
						source: any;
						activityStartDate?: Date;
						term: string;
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					clrKey?: number;
					clr: any;
					endorsementKey?: number;
					endorsement: {
						endorsementKey: number;
						context: string;
						achievementKey?: number;
						achievement: {
							achievementKey: number;
							achievementClrs: any[];
							id: string;
							type: string;
							achievementType: string;
							alignments: any[];
							associations: any[];
							creditsAvailable?: number;
							description: string;
							endorsements: any[];
							humanCode: string;
							identifiers: any[];
							name: string;
							fieldOfStudy: string;
							image: string;
							issuer: any;
							level: string;
							requirement: {
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
						};
						assertionKey?: number;
						assertion: {
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
							id: string;
							type: string;
							achievement: {
								achievementKey: number;
								achievementClrs: any[];
								id: string;
								type: string;
								achievementType: string;
								alignments: any[];
								associations: any[];
								creditsAvailable?: number;
								description: string;
								endorsements: any[];
								humanCode: string;
								identifiers: any[];
								name: string;
								fieldOfStudy: string;
								image: string;
								issuer: any;
								level: string;
								requirement: {
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
							};
							creditsEarned?: number;
							activityEndDate?: Date;
							endorsements: any[];
							evidence: any[];
							expires?: Date;
							image: string;
							issuedOn?: Date;
							licenseNumber: string;
							narrative: string;
							recipient: {
								identityKey: number;
								id: string;
								type: string;
								identity: string;
								hashed: boolean;
								salt: string;
								additionalProperties: { [index: string]: any };
							};
							results: any[];
							revocationReason: string;
							revoked?: boolean;
							role: string;
							signedEndorsements: string[];
							source: any;
							activityStartDate?: Date;
							term: string;
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						profileKey?: number;
						profile: any;
						id: string;
						type: string;
						claim: {
							id: string;
							type: string;
							endorsementComment: string;
							additionalProperties: { [index: string]: any };
							endorsementClaimKey: number;
							endorsementKey: number;
							endorsement: any;
						};
						issuedOn: Date;
						issuer: {
							id: string;
							type: string;
							additionalName: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: any;
								profileKey?: number;
								profile: any;
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
							description: string;
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementProfileKey: number;
							profileKey?: number;
							profile: any;
						};
						revocationReason: string;
						revoked?: boolean;
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					profileKey?: number;
					profile: any;
					id: string;
					type: any;
					allowedOrigins: string[];
					creator: string;
					startsWith: string[];
					verificationProperty: string;
					additionalProperties: { [index: string]: any };
				};
				additionalProperties: { [index: string]: any };
				profileKey: number;
				endorsementProfile: {
					id: string;
					type: string;
					additionalName: string;
					address: {
						addressKey: number;
						endorsementProfileKey?: number;
						endorsementProfile: any;
						profileKey?: number;
						profile: any;
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
					description: string;
					email: string;
					familyName: string;
					givenName: string;
					identifiers: any[];
					image: string;
					name: string;
					official: string;
					publicKey: {
						cryptographicKeyKey: number;
						context: string;
						id: string;
						type: string;
						owner: string;
						publicKeyPem: string;
						additionalProperties: { [index: string]: any };
					};
					revocationList: string;
					sourcedId: string;
					studentId: string;
					telephone: string;
					url: string;
					verification: {
						verificationKey: number;
						assertionKey?: number;
						assertion: {
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
							id: string;
							type: string;
							achievement: {
								achievementKey: number;
								achievementClrs: any[];
								id: string;
								type: string;
								achievementType: string;
								alignments: any[];
								associations: any[];
								creditsAvailable?: number;
								description: string;
								endorsements: any[];
								humanCode: string;
								identifiers: any[];
								name: string;
								fieldOfStudy: string;
								image: string;
								issuer: any;
								level: string;
								requirement: {
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
							};
							creditsEarned?: number;
							activityEndDate?: Date;
							endorsements: any[];
							evidence: any[];
							expires?: Date;
							image: string;
							issuedOn?: Date;
							licenseNumber: string;
							narrative: string;
							recipient: {
								identityKey: number;
								id: string;
								type: string;
								identity: string;
								hashed: boolean;
								salt: string;
								additionalProperties: { [index: string]: any };
							};
							results: any[];
							revocationReason: string;
							revoked?: boolean;
							role: string;
							signedEndorsements: string[];
							source: any;
							activityStartDate?: Date;
							term: string;
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						clrKey?: number;
						clr: any;
						endorsementKey?: number;
						endorsement: {
							endorsementKey: number;
							context: string;
							achievementKey?: number;
							achievement: {
								achievementKey: number;
								achievementClrs: any[];
								id: string;
								type: string;
								achievementType: string;
								alignments: any[];
								associations: any[];
								creditsAvailable?: number;
								description: string;
								endorsements: any[];
								humanCode: string;
								identifiers: any[];
								name: string;
								fieldOfStudy: string;
								image: string;
								issuer: any;
								level: string;
								requirement: {
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
							};
							assertionKey?: number;
							assertion: {
								assertionKey: number;
								isSigned: boolean;
								context: string;
								assertionClrs: any[];
								id: string;
								type: string;
								achievement: {
									achievementKey: number;
									achievementClrs: any[];
									id: string;
									type: string;
									achievementType: string;
									alignments: any[];
									associations: any[];
									creditsAvailable?: number;
									description: string;
									endorsements: any[];
									humanCode: string;
									identifiers: any[];
									name: string;
									fieldOfStudy: string;
									image: string;
									issuer: any;
									level: string;
									requirement: {
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
								};
								creditsEarned?: number;
								activityEndDate?: Date;
								endorsements: any[];
								evidence: any[];
								expires?: Date;
								image: string;
								issuedOn?: Date;
								licenseNumber: string;
								narrative: string;
								recipient: {
									identityKey: number;
									id: string;
									type: string;
									identity: string;
									hashed: boolean;
									salt: string;
									additionalProperties: { [index: string]: any };
								};
								results: any[];
								revocationReason: string;
								revoked?: boolean;
								role: string;
								signedEndorsements: string[];
								source: any;
								activityStartDate?: Date;
								term: string;
								verification: any;
								additionalProperties: { [index: string]: any };
							};
							profileKey?: number;
							profile: any;
							id: string;
							type: string;
							claim: {
								id: string;
								type: string;
								endorsementComment: string;
								additionalProperties: { [index: string]: any };
								endorsementClaimKey: number;
								endorsementKey: number;
								endorsement: any;
							};
							issuedOn: Date;
							issuer: any;
							revocationReason: string;
							revoked?: boolean;
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						profileKey?: number;
						profile: any;
						id: string;
						type: any;
						allowedOrigins: string[];
						creator: string;
						startsWith: string[];
						verificationProperty: string;
						additionalProperties: { [index: string]: any };
					};
					additionalProperties: { [index: string]: any };
					endorsementProfileKey: number;
					profileKey?: number;
					profile: any;
				};
			};
			name: string;
			partial?: boolean;
			publisher: {
				id: string;
				type: string;
				address: {
					addressKey: number;
					endorsementProfileKey?: number;
					endorsementProfile: {
						id: string;
						type: string;
						additionalName: string;
						address: any;
						description: string;
						email: string;
						familyName: string;
						givenName: string;
						identifiers: any[];
						image: string;
						name: string;
						official: string;
						publicKey: {
							cryptographicKeyKey: number;
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
						revocationList: string;
						sourcedId: string;
						studentId: string;
						telephone: string;
						url: string;
						verification: {
							verificationKey: number;
							assertionKey?: number;
							assertion: {
								assertionKey: number;
								isSigned: boolean;
								context: string;
								assertionClrs: any[];
								id: string;
								type: string;
								achievement: {
									achievementKey: number;
									achievementClrs: any[];
									id: string;
									type: string;
									achievementType: string;
									alignments: any[];
									associations: any[];
									creditsAvailable?: number;
									description: string;
									endorsements: any[];
									humanCode: string;
									identifiers: any[];
									name: string;
									fieldOfStudy: string;
									image: string;
									issuer: any;
									level: string;
									requirement: {
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
								};
								creditsEarned?: number;
								activityEndDate?: Date;
								endorsements: any[];
								evidence: any[];
								expires?: Date;
								image: string;
								issuedOn?: Date;
								licenseNumber: string;
								narrative: string;
								recipient: {
									identityKey: number;
									id: string;
									type: string;
									identity: string;
									hashed: boolean;
									salt: string;
									additionalProperties: { [index: string]: any };
								};
								results: any[];
								revocationReason: string;
								revoked?: boolean;
								role: string;
								signedEndorsements: string[];
								source: any;
								activityStartDate?: Date;
								term: string;
								verification: any;
								additionalProperties: { [index: string]: any };
							};
							clrKey?: number;
							clr: any;
							endorsementKey?: number;
							endorsement: {
								endorsementKey: number;
								context: string;
								achievementKey?: number;
								achievement: {
									achievementKey: number;
									achievementClrs: any[];
									id: string;
									type: string;
									achievementType: string;
									alignments: any[];
									associations: any[];
									creditsAvailable?: number;
									description: string;
									endorsements: any[];
									humanCode: string;
									identifiers: any[];
									name: string;
									fieldOfStudy: string;
									image: string;
									issuer: any;
									level: string;
									requirement: {
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
								};
								assertionKey?: number;
								assertion: {
									assertionKey: number;
									isSigned: boolean;
									context: string;
									assertionClrs: any[];
									id: string;
									type: string;
									achievement: {
										achievementKey: number;
										achievementClrs: any[];
										id: string;
										type: string;
										achievementType: string;
										alignments: any[];
										associations: any[];
										creditsAvailable?: number;
										description: string;
										endorsements: any[];
										humanCode: string;
										identifiers: any[];
										name: string;
										fieldOfStudy: string;
										image: string;
										issuer: any;
										level: string;
										requirement: {
											criteriaKey: number;
											achievementKey?: number;
											achievement: any;
											id: string;
											type: string;
											narrative: string;
											additionalProperties: { [index: string]: any };
										};
										resultDescriptions: any[];
										signedEndorsements: string[];
										specialization: string;
										tags: string[];
										additionalProperties: { [index: string]: any };
									};
									creditsEarned?: number;
									activityEndDate?: Date;
									endorsements: any[];
									evidence: any[];
									expires?: Date;
									image: string;
									issuedOn?: Date;
									licenseNumber: string;
									narrative: string;
									recipient: {
										identityKey: number;
										id: string;
										type: string;
										identity: string;
										hashed: boolean;
										salt: string;
										additionalProperties: { [index: string]: any };
									};
									results: any[];
									revocationReason: string;
									revoked?: boolean;
									role: string;
									signedEndorsements: string[];
									source: any;
									activityStartDate?: Date;
									term: string;
									verification: any;
									additionalProperties: { [index: string]: any };
								};
								profileKey?: number;
								profile: any;
								id: string;
								type: string;
								claim: {
									id: string;
									type: string;
									endorsementComment: string;
									additionalProperties: { [index: string]: any };
									endorsementClaimKey: number;
									endorsementKey: number;
									endorsement: any;
								};
								issuedOn: Date;
								issuer: any;
								revocationReason: string;
								revoked?: boolean;
								verification: any;
								additionalProperties: { [index: string]: any };
							};
							profileKey?: number;
							profile: any;
							id: string;
							type: any;
							allowedOrigins: string[];
							creator: string;
							startsWith: string[];
							verificationProperty: string;
							additionalProperties: { [index: string]: any };
						};
						additionalProperties: { [index: string]: any };
						endorsementProfileKey: number;
						profileKey?: number;
						profile: any;
					};
					profileKey?: number;
					profile: any;
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
				birthDate: Date;
				description: string;
				email: string;
				endorsements: any[];
				familyName: string;
				givenName: string;
				identifiers: any[];
				image: string;
				name: string;
				official: string;
				parentOrg: any;
				publicKey: {
					cryptographicKeyKey: number;
					context: string;
					id: string;
					type: string;
					owner: string;
					publicKeyPem: string;
					additionalProperties: { [index: string]: any };
				};
				revocationList: string;
				signedEndorsements: string[];
				sourcedId: string;
				studentId: string;
				telephone: string;
				url: string;
				verification: {
					verificationKey: number;
					assertionKey?: number;
					assertion: {
						assertionKey: number;
						isSigned: boolean;
						context: string;
						assertionClrs: any[];
						id: string;
						type: string;
						achievement: {
							achievementKey: number;
							achievementClrs: any[];
							id: string;
							type: string;
							achievementType: string;
							alignments: any[];
							associations: any[];
							creditsAvailable?: number;
							description: string;
							endorsements: any[];
							humanCode: string;
							identifiers: any[];
							name: string;
							fieldOfStudy: string;
							image: string;
							issuer: any;
							level: string;
							requirement: {
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
						};
						creditsEarned?: number;
						activityEndDate?: Date;
						endorsements: any[];
						evidence: any[];
						expires?: Date;
						image: string;
						issuedOn?: Date;
						licenseNumber: string;
						narrative: string;
						recipient: {
							identityKey: number;
							id: string;
							type: string;
							identity: string;
							hashed: boolean;
							salt: string;
							additionalProperties: { [index: string]: any };
						};
						results: any[];
						revocationReason: string;
						revoked?: boolean;
						role: string;
						signedEndorsements: string[];
						source: any;
						activityStartDate?: Date;
						term: string;
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					clrKey?: number;
					clr: any;
					endorsementKey?: number;
					endorsement: {
						endorsementKey: number;
						context: string;
						achievementKey?: number;
						achievement: {
							achievementKey: number;
							achievementClrs: any[];
							id: string;
							type: string;
							achievementType: string;
							alignments: any[];
							associations: any[];
							creditsAvailable?: number;
							description: string;
							endorsements: any[];
							humanCode: string;
							identifiers: any[];
							name: string;
							fieldOfStudy: string;
							image: string;
							issuer: any;
							level: string;
							requirement: {
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
						};
						assertionKey?: number;
						assertion: {
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
							id: string;
							type: string;
							achievement: {
								achievementKey: number;
								achievementClrs: any[];
								id: string;
								type: string;
								achievementType: string;
								alignments: any[];
								associations: any[];
								creditsAvailable?: number;
								description: string;
								endorsements: any[];
								humanCode: string;
								identifiers: any[];
								name: string;
								fieldOfStudy: string;
								image: string;
								issuer: any;
								level: string;
								requirement: {
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
							};
							creditsEarned?: number;
							activityEndDate?: Date;
							endorsements: any[];
							evidence: any[];
							expires?: Date;
							image: string;
							issuedOn?: Date;
							licenseNumber: string;
							narrative: string;
							recipient: {
								identityKey: number;
								id: string;
								type: string;
								identity: string;
								hashed: boolean;
								salt: string;
								additionalProperties: { [index: string]: any };
							};
							results: any[];
							revocationReason: string;
							revoked?: boolean;
							role: string;
							signedEndorsements: string[];
							source: any;
							activityStartDate?: Date;
							term: string;
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						profileKey?: number;
						profile: any;
						id: string;
						type: string;
						claim: {
							id: string;
							type: string;
							endorsementComment: string;
							additionalProperties: { [index: string]: any };
							endorsementClaimKey: number;
							endorsementKey: number;
							endorsement: any;
						};
						issuedOn: Date;
						issuer: {
							id: string;
							type: string;
							additionalName: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: any;
								profileKey?: number;
								profile: any;
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
							description: string;
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementProfileKey: number;
							profileKey?: number;
							profile: any;
						};
						revocationReason: string;
						revoked?: boolean;
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					profileKey?: number;
					profile: any;
					id: string;
					type: any;
					allowedOrigins: string[];
					creator: string;
					startsWith: string[];
					verificationProperty: string;
					additionalProperties: { [index: string]: any };
				};
				additionalProperties: { [index: string]: any };
				profileKey: number;
				endorsementProfile: {
					id: string;
					type: string;
					additionalName: string;
					address: {
						addressKey: number;
						endorsementProfileKey?: number;
						endorsementProfile: any;
						profileKey?: number;
						profile: any;
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
					description: string;
					email: string;
					familyName: string;
					givenName: string;
					identifiers: any[];
					image: string;
					name: string;
					official: string;
					publicKey: {
						cryptographicKeyKey: number;
						context: string;
						id: string;
						type: string;
						owner: string;
						publicKeyPem: string;
						additionalProperties: { [index: string]: any };
					};
					revocationList: string;
					sourcedId: string;
					studentId: string;
					telephone: string;
					url: string;
					verification: {
						verificationKey: number;
						assertionKey?: number;
						assertion: {
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
							id: string;
							type: string;
							achievement: {
								achievementKey: number;
								achievementClrs: any[];
								id: string;
								type: string;
								achievementType: string;
								alignments: any[];
								associations: any[];
								creditsAvailable?: number;
								description: string;
								endorsements: any[];
								humanCode: string;
								identifiers: any[];
								name: string;
								fieldOfStudy: string;
								image: string;
								issuer: any;
								level: string;
								requirement: {
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
							};
							creditsEarned?: number;
							activityEndDate?: Date;
							endorsements: any[];
							evidence: any[];
							expires?: Date;
							image: string;
							issuedOn?: Date;
							licenseNumber: string;
							narrative: string;
							recipient: {
								identityKey: number;
								id: string;
								type: string;
								identity: string;
								hashed: boolean;
								salt: string;
								additionalProperties: { [index: string]: any };
							};
							results: any[];
							revocationReason: string;
							revoked?: boolean;
							role: string;
							signedEndorsements: string[];
							source: any;
							activityStartDate?: Date;
							term: string;
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						clrKey?: number;
						clr: any;
						endorsementKey?: number;
						endorsement: {
							endorsementKey: number;
							context: string;
							achievementKey?: number;
							achievement: {
								achievementKey: number;
								achievementClrs: any[];
								id: string;
								type: string;
								achievementType: string;
								alignments: any[];
								associations: any[];
								creditsAvailable?: number;
								description: string;
								endorsements: any[];
								humanCode: string;
								identifiers: any[];
								name: string;
								fieldOfStudy: string;
								image: string;
								issuer: any;
								level: string;
								requirement: {
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
							};
							assertionKey?: number;
							assertion: {
								assertionKey: number;
								isSigned: boolean;
								context: string;
								assertionClrs: any[];
								id: string;
								type: string;
								achievement: {
									achievementKey: number;
									achievementClrs: any[];
									id: string;
									type: string;
									achievementType: string;
									alignments: any[];
									associations: any[];
									creditsAvailable?: number;
									description: string;
									endorsements: any[];
									humanCode: string;
									identifiers: any[];
									name: string;
									fieldOfStudy: string;
									image: string;
									issuer: any;
									level: string;
									requirement: {
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
								};
								creditsEarned?: number;
								activityEndDate?: Date;
								endorsements: any[];
								evidence: any[];
								expires?: Date;
								image: string;
								issuedOn?: Date;
								licenseNumber: string;
								narrative: string;
								recipient: {
									identityKey: number;
									id: string;
									type: string;
									identity: string;
									hashed: boolean;
									salt: string;
									additionalProperties: { [index: string]: any };
								};
								results: any[];
								revocationReason: string;
								revoked?: boolean;
								role: string;
								signedEndorsements: string[];
								source: any;
								activityStartDate?: Date;
								term: string;
								verification: any;
								additionalProperties: { [index: string]: any };
							};
							profileKey?: number;
							profile: any;
							id: string;
							type: string;
							claim: {
								id: string;
								type: string;
								endorsementComment: string;
								additionalProperties: { [index: string]: any };
								endorsementClaimKey: number;
								endorsementKey: number;
								endorsement: any;
							};
							issuedOn: Date;
							issuer: any;
							revocationReason: string;
							revoked?: boolean;
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						profileKey?: number;
						profile: any;
						id: string;
						type: any;
						allowedOrigins: string[];
						creator: string;
						startsWith: string[];
						verificationProperty: string;
						additionalProperties: { [index: string]: any };
					};
					additionalProperties: { [index: string]: any };
					endorsementProfileKey: number;
					profileKey?: number;
					profile: any;
				};
			};
			revocationReason: string;
			revoked?: boolean;
			signedAssertions: string[];
			signedEndorsements: string[];
			verification: {
				verificationKey: number;
				assertionKey?: number;
				assertion: {
					assertionKey: number;
					isSigned: boolean;
					context: string;
					assertionClrs: any[];
					id: string;
					type: string;
					achievement: {
						achievementKey: number;
						achievementClrs: any[];
						id: string;
						type: string;
						achievementType: string;
						alignments: any[];
						associations: any[];
						creditsAvailable?: number;
						description: string;
						endorsements: any[];
						humanCode: string;
						identifiers: any[];
						name: string;
						fieldOfStudy: string;
						image: string;
						issuer: {
							id: string;
							type: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: {
									id: string;
									type: string;
									additionalName: string;
									address: any;
									description: string;
									email: string;
									familyName: string;
									givenName: string;
									identifiers: any[];
									image: string;
									name: string;
									official: string;
									publicKey: {
										cryptographicKeyKey: number;
										context: string;
										id: string;
										type: string;
										owner: string;
										publicKeyPem: string;
										additionalProperties: { [index: string]: any };
									};
									revocationList: string;
									sourcedId: string;
									studentId: string;
									telephone: string;
									url: string;
									verification: any;
									additionalProperties: { [index: string]: any };
									endorsementProfileKey: number;
									profileKey?: number;
									profile: any;
								};
								profileKey?: number;
								profile: any;
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
							birthDate: Date;
							description: string;
							email: string;
							endorsements: any[];
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							parentOrg: any;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							signedEndorsements: string[];
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							profileKey: number;
							endorsementProfile: {
								id: string;
								type: string;
								additionalName: string;
								address: {
									addressKey: number;
									endorsementProfileKey?: number;
									endorsementProfile: any;
									profileKey?: number;
									profile: any;
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
								description: string;
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								endorsementProfileKey: number;
								profileKey?: number;
								profile: any;
							};
						};
						level: string;
						requirement: {
							criteriaKey: number;
							achievementKey?: number;
							achievement: any;
							id: string;
							type: string;
							narrative: string;
							additionalProperties: { [index: string]: any };
						};
						resultDescriptions: any[];
						signedEndorsements: string[];
						specialization: string;
						tags: string[];
						additionalProperties: { [index: string]: any };
					};
					creditsEarned?: number;
					activityEndDate?: Date;
					endorsements: any[];
					evidence: any[];
					expires?: Date;
					image: string;
					issuedOn?: Date;
					licenseNumber: string;
					narrative: string;
					recipient: {
						identityKey: number;
						id: string;
						type: string;
						identity: string;
						hashed: boolean;
						salt: string;
						additionalProperties: { [index: string]: any };
					};
					results: any[];
					revocationReason: string;
					revoked?: boolean;
					role: string;
					signedEndorsements: string[];
					source: {
						id: string;
						type: string;
						address: {
							addressKey: number;
							endorsementProfileKey?: number;
							endorsementProfile: {
								id: string;
								type: string;
								additionalName: string;
								address: any;
								description: string;
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								endorsementProfileKey: number;
								profileKey?: number;
								profile: any;
							};
							profileKey?: number;
							profile: any;
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
						birthDate: Date;
						description: string;
						email: string;
						endorsements: any[];
						familyName: string;
						givenName: string;
						identifiers: any[];
						image: string;
						name: string;
						official: string;
						parentOrg: any;
						publicKey: {
							cryptographicKeyKey: number;
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
						revocationList: string;
						signedEndorsements: string[];
						sourcedId: string;
						studentId: string;
						telephone: string;
						url: string;
						verification: any;
						additionalProperties: { [index: string]: any };
						profileKey: number;
						endorsementProfile: {
							id: string;
							type: string;
							additionalName: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: any;
								profileKey?: number;
								profile: any;
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
							description: string;
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementProfileKey: number;
							profileKey?: number;
							profile: any;
						};
					};
					activityStartDate?: Date;
					term: string;
					verification: any;
					additionalProperties: { [index: string]: any };
				};
				clrKey?: number;
				clr: any;
				endorsementKey?: number;
				endorsement: {
					endorsementKey: number;
					context: string;
					achievementKey?: number;
					achievement: {
						achievementKey: number;
						achievementClrs: any[];
						id: string;
						type: string;
						achievementType: string;
						alignments: any[];
						associations: any[];
						creditsAvailable?: number;
						description: string;
						endorsements: any[];
						humanCode: string;
						identifiers: any[];
						name: string;
						fieldOfStudy: string;
						image: string;
						issuer: {
							id: string;
							type: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: {
									id: string;
									type: string;
									additionalName: string;
									address: any;
									description: string;
									email: string;
									familyName: string;
									givenName: string;
									identifiers: any[];
									image: string;
									name: string;
									official: string;
									publicKey: {
										cryptographicKeyKey: number;
										context: string;
										id: string;
										type: string;
										owner: string;
										publicKeyPem: string;
										additionalProperties: { [index: string]: any };
									};
									revocationList: string;
									sourcedId: string;
									studentId: string;
									telephone: string;
									url: string;
									verification: any;
									additionalProperties: { [index: string]: any };
									endorsementProfileKey: number;
									profileKey?: number;
									profile: any;
								};
								profileKey?: number;
								profile: any;
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
							birthDate: Date;
							description: string;
							email: string;
							endorsements: any[];
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							parentOrg: any;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							signedEndorsements: string[];
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							profileKey: number;
							endorsementProfile: {
								id: string;
								type: string;
								additionalName: string;
								address: {
									addressKey: number;
									endorsementProfileKey?: number;
									endorsementProfile: any;
									profileKey?: number;
									profile: any;
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
								description: string;
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								endorsementProfileKey: number;
								profileKey?: number;
								profile: any;
							};
						};
						level: string;
						requirement: {
							criteriaKey: number;
							achievementKey?: number;
							achievement: any;
							id: string;
							type: string;
							narrative: string;
							additionalProperties: { [index: string]: any };
						};
						resultDescriptions: any[];
						signedEndorsements: string[];
						specialization: string;
						tags: string[];
						additionalProperties: { [index: string]: any };
					};
					assertionKey?: number;
					assertion: {
						assertionKey: number;
						isSigned: boolean;
						context: string;
						assertionClrs: any[];
						id: string;
						type: string;
						achievement: {
							achievementKey: number;
							achievementClrs: any[];
							id: string;
							type: string;
							achievementType: string;
							alignments: any[];
							associations: any[];
							creditsAvailable?: number;
							description: string;
							endorsements: any[];
							humanCode: string;
							identifiers: any[];
							name: string;
							fieldOfStudy: string;
							image: string;
							issuer: {
								id: string;
								type: string;
								address: {
									addressKey: number;
									endorsementProfileKey?: number;
									endorsementProfile: {
										id: string;
										type: string;
										additionalName: string;
										address: any;
										description: string;
										email: string;
										familyName: string;
										givenName: string;
										identifiers: any[];
										image: string;
										name: string;
										official: string;
										publicKey: {
											cryptographicKeyKey: number;
											context: string;
											id: string;
											type: string;
											owner: string;
											publicKeyPem: string;
											additionalProperties: { [index: string]: any };
										};
										revocationList: string;
										sourcedId: string;
										studentId: string;
										telephone: string;
										url: string;
										verification: any;
										additionalProperties: { [index: string]: any };
										endorsementProfileKey: number;
										profileKey?: number;
										profile: any;
									};
									profileKey?: number;
									profile: any;
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
								birthDate: Date;
								description: string;
								email: string;
								endorsements: any[];
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								parentOrg: any;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								signedEndorsements: string[];
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								profileKey: number;
								endorsementProfile: {
									id: string;
									type: string;
									additionalName: string;
									address: {
										addressKey: number;
										endorsementProfileKey?: number;
										endorsementProfile: any;
										profileKey?: number;
										profile: any;
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
									description: string;
									email: string;
									familyName: string;
									givenName: string;
									identifiers: any[];
									image: string;
									name: string;
									official: string;
									publicKey: {
										cryptographicKeyKey: number;
										context: string;
										id: string;
										type: string;
										owner: string;
										publicKeyPem: string;
										additionalProperties: { [index: string]: any };
									};
									revocationList: string;
									sourcedId: string;
									studentId: string;
									telephone: string;
									url: string;
									verification: any;
									additionalProperties: { [index: string]: any };
									endorsementProfileKey: number;
									profileKey?: number;
									profile: any;
								};
							};
							level: string;
							requirement: {
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
						};
						creditsEarned?: number;
						activityEndDate?: Date;
						endorsements: any[];
						evidence: any[];
						expires?: Date;
						image: string;
						issuedOn?: Date;
						licenseNumber: string;
						narrative: string;
						recipient: {
							identityKey: number;
							id: string;
							type: string;
							identity: string;
							hashed: boolean;
							salt: string;
							additionalProperties: { [index: string]: any };
						};
						results: any[];
						revocationReason: string;
						revoked?: boolean;
						role: string;
						signedEndorsements: string[];
						source: {
							id: string;
							type: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: {
									id: string;
									type: string;
									additionalName: string;
									address: any;
									description: string;
									email: string;
									familyName: string;
									givenName: string;
									identifiers: any[];
									image: string;
									name: string;
									official: string;
									publicKey: {
										cryptographicKeyKey: number;
										context: string;
										id: string;
										type: string;
										owner: string;
										publicKeyPem: string;
										additionalProperties: { [index: string]: any };
									};
									revocationList: string;
									sourcedId: string;
									studentId: string;
									telephone: string;
									url: string;
									verification: any;
									additionalProperties: { [index: string]: any };
									endorsementProfileKey: number;
									profileKey?: number;
									profile: any;
								};
								profileKey?: number;
								profile: any;
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
							birthDate: Date;
							description: string;
							email: string;
							endorsements: any[];
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							parentOrg: any;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							signedEndorsements: string[];
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							profileKey: number;
							endorsementProfile: {
								id: string;
								type: string;
								additionalName: string;
								address: {
									addressKey: number;
									endorsementProfileKey?: number;
									endorsementProfile: any;
									profileKey?: number;
									profile: any;
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
								description: string;
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								endorsementProfileKey: number;
								profileKey?: number;
								profile: any;
							};
						};
						activityStartDate?: Date;
						term: string;
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					profileKey?: number;
					profile: {
						id: string;
						type: string;
						address: {
							addressKey: number;
							endorsementProfileKey?: number;
							endorsementProfile: {
								id: string;
								type: string;
								additionalName: string;
								address: any;
								description: string;
								email: string;
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								endorsementProfileKey: number;
								profileKey?: number;
								profile: any;
							};
							profileKey?: number;
							profile: any;
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
						birthDate: Date;
						description: string;
						email: string;
						endorsements: any[];
						familyName: string;
						givenName: string;
						identifiers: any[];
						image: string;
						name: string;
						official: string;
						parentOrg: any;
						publicKey: {
							cryptographicKeyKey: number;
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
						revocationList: string;
						signedEndorsements: string[];
						sourcedId: string;
						studentId: string;
						telephone: string;
						url: string;
						verification: any;
						additionalProperties: { [index: string]: any };
						profileKey: number;
						endorsementProfile: {
							id: string;
							type: string;
							additionalName: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: any;
								profileKey?: number;
								profile: any;
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
							description: string;
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementProfileKey: number;
							profileKey?: number;
							profile: any;
						};
					};
					id: string;
					type: string;
					claim: {
						id: string;
						type: string;
						endorsementComment: string;
						additionalProperties: { [index: string]: any };
						endorsementClaimKey: number;
						endorsementKey: number;
						endorsement: any;
					};
					issuedOn: Date;
					issuer: {
						id: string;
						type: string;
						additionalName: string;
						address: {
							addressKey: number;
							endorsementProfileKey?: number;
							endorsementProfile: any;
							profileKey?: number;
							profile: {
								id: string;
								type: string;
								address: any;
								additionalName: string;
								birthDate: Date;
								description: string;
								email: string;
								endorsements: any[];
								familyName: string;
								givenName: string;
								identifiers: any[];
								image: string;
								name: string;
								official: string;
								parentOrg: any;
								publicKey: {
									cryptographicKeyKey: number;
									context: string;
									id: string;
									type: string;
									owner: string;
									publicKeyPem: string;
									additionalProperties: { [index: string]: any };
								};
								revocationList: string;
								signedEndorsements: string[];
								sourcedId: string;
								studentId: string;
								telephone: string;
								url: string;
								verification: any;
								additionalProperties: { [index: string]: any };
								profileKey: number;
								endorsementProfile: any;
							};
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
						description: string;
						email: string;
						familyName: string;
						givenName: string;
						identifiers: any[];
						image: string;
						name: string;
						official: string;
						publicKey: {
							cryptographicKeyKey: number;
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
						revocationList: string;
						sourcedId: string;
						studentId: string;
						telephone: string;
						url: string;
						verification: any;
						additionalProperties: { [index: string]: any };
						endorsementProfileKey: number;
						profileKey?: number;
						profile: {
							id: string;
							type: string;
							address: {
								addressKey: number;
								endorsementProfileKey?: number;
								endorsementProfile: any;
								profileKey?: number;
								profile: any;
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
							birthDate: Date;
							description: string;
							email: string;
							endorsements: any[];
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							parentOrg: any;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							signedEndorsements: string[];
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							profileKey: number;
							endorsementProfile: any;
						};
					};
					revocationReason: string;
					revoked?: boolean;
					verification: any;
					additionalProperties: { [index: string]: any };
				};
				profileKey?: number;
				profile: {
					id: string;
					type: string;
					address: {
						addressKey: number;
						endorsementProfileKey?: number;
						endorsementProfile: {
							id: string;
							type: string;
							additionalName: string;
							address: any;
							description: string;
							email: string;
							familyName: string;
							givenName: string;
							identifiers: any[];
							image: string;
							name: string;
							official: string;
							publicKey: {
								cryptographicKeyKey: number;
								context: string;
								id: string;
								type: string;
								owner: string;
								publicKeyPem: string;
								additionalProperties: { [index: string]: any };
							};
							revocationList: string;
							sourcedId: string;
							studentId: string;
							telephone: string;
							url: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementProfileKey: number;
							profileKey?: number;
							profile: any;
						};
						profileKey?: number;
						profile: any;
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
					birthDate: Date;
					description: string;
					email: string;
					endorsements: any[];
					familyName: string;
					givenName: string;
					identifiers: any[];
					image: string;
					name: string;
					official: string;
					parentOrg: any;
					publicKey: {
						cryptographicKeyKey: number;
						context: string;
						id: string;
						type: string;
						owner: string;
						publicKeyPem: string;
						additionalProperties: { [index: string]: any };
					};
					revocationList: string;
					signedEndorsements: string[];
					sourcedId: string;
					studentId: string;
					telephone: string;
					url: string;
					verification: any;
					additionalProperties: { [index: string]: any };
					profileKey: number;
					endorsementProfile: {
						id: string;
						type: string;
						additionalName: string;
						address: {
							addressKey: number;
							endorsementProfileKey?: number;
							endorsementProfile: any;
							profileKey?: number;
							profile: any;
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
						description: string;
						email: string;
						familyName: string;
						givenName: string;
						identifiers: any[];
						image: string;
						name: string;
						official: string;
						publicKey: {
							cryptographicKeyKey: number;
							context: string;
							id: string;
							type: string;
							owner: string;
							publicKeyPem: string;
							additionalProperties: { [index: string]: any };
						};
						revocationList: string;
						sourcedId: string;
						studentId: string;
						telephone: string;
						url: string;
						verification: any;
						additionalProperties: { [index: string]: any };
						endorsementProfileKey: number;
						profileKey?: number;
						profile: any;
					};
				};
				id: string;
				type: any;
				allowedOrigins: string[];
				creator: string;
				startsWith: string[];
				verificationProperty: string;
				additionalProperties: { [index: string]: any };
			};
			additionalProperties: { [index: string]: any };
			clrKey: number;
			achievementClrs: any[];
			assertionClrs: any[];
		};
		hasPdf: boolean;
		ancestorCredentialPackage: {
		/** Primary key */
			id: number;
			typeId: server.packageTypeEnum;
			revoked: boolean;
			revocationReason: string;
		/** Foreign key back to the authorization. */
			authorizationForeignKey: string;
			authorization: server.authorizationModel;
			clr: {
		/** START Actual persisted dataForeign Keys */
				parentCredentialPackageId?: number;
				parentVerifiableCredentialId?: number;
				parentClrSetId?: number;
		/** Foreign key back to the authorization. */
				authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
				assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
				authorization: server.authorizationModel;
		/** DateTime when this CLR was issued. */
				issuedOn: Date;
		/** Primary key. */
				id: number;
		/** The CLR @id. */
				identifier: string;
		/** Complete JSON of the CLR. */
				json: string;
		/** Learner of the CLR. */
				learnerName: string;
		/** All the links tied to this CLR. */
				links: server.linkModel[];
		/** Optional name of CLR. Primarily for self-published CLRs. */
				name: string;
		/** Publisher of the CLR. */
				publisherName: string;
		/** The date and time the CLR was retrieved from the authorization server. */
				refreshedAt: Date;
		/** The Signed CLR if it was signed. */
				signedClr: string;
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
				credentialPackageId: number;
				assertions: any[];
		/** END Actual persisted data */
				credentialPackage: any;
		/** possible Parents */
				parentCredentialPackage: any;
				parentVerifiableCredential: {
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
					parentCredentialPackage: any;
					clrSets: any[];
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				parentClrSet: {
					id: number;
					parentCredentialPackageId?: number;
					parentVerifiableCredentialId?: number;
					clrsCount: number;
					identifier: string;
					json: string;
					parentCredentialPackage: any;
					parentVerifiableCredential: {
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
						parentCredentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
		/** true indicates this CLR's Id was received in a revocation list from the source */
				isRevoked: boolean;
			};
			clrSet: {
				id: number;
				parentCredentialPackageId?: number;
				parentVerifiableCredentialId?: number;
				clrsCount: number;
				identifier: string;
				json: string;
				parentCredentialPackage: any;
				parentVerifiableCredential: {
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
					parentCredentialPackage: any;
					clrSets: any[];
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
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
			verifiableCredential: {
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
				parentCredentialPackage: any;
				clrSets: any[];
				clrs: any[];
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
			};
			name: string;
		/** This Package is tied to a specific application user. */
			userId: string;
			user: server.applicationUser;
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
			containedClrs: any[];
		};
		allAssertions: any[];
		parentAssertions: any[];
		pdfs: any[];
	}
}
