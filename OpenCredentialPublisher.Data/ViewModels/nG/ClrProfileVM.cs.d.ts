declare module server {
	interface clrProfileVM {
		id: string;
		name: string;
		email: string;
		telephone: string;
		studentId: string;
		sourcedId: string;
		url: string;
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
					id: string;
					type: any;
					allowedOrigins: string[];
					creator: string;
					startsWith: string[];
					verificationProperty: string;
					additionalProperties: { [index: string]: any };
					verificationKey: number;
					assertionKey?: number;
					assertion: {
						id: string;
						type: string;
						achievement: {
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
								profileKey: number;
								endorsementProfile: any;
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
							};
							level: string;
							requirement: {
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
							achievementKey: number;
							achievementClrs: any[];
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
							profileKey: number;
							endorsementProfile: any;
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
						};
						activityStartDate?: Date;
						term: string;
						verification: any;
						additionalProperties: { [index: string]: any };
						assertionKey: number;
						isSigned: boolean;
						context: string;
						assertionClrs: any[];
					};
					clrKey?: number;
					clr: {
						clrKey: number;
						achievementClrs: any[];
						assertionClrs: any[];
						context: string;
						id: string;
						type: string;
						achievements: any[];
						assertions: any[];
						issuedOn: Date;
						learner: {
							profileKey: number;
							endorsementProfile: any;
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
						};
						name: string;
						partial?: boolean;
						publisher: {
							profileKey: number;
							endorsementProfile: any;
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
						};
						revocationReason: string;
						revoked?: boolean;
						signedAssertions: string[];
						signedEndorsements: string[];
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					endorsementKey?: number;
					endorsement: {
						id: string;
						type: string;
						claim: {
							endorsementClaimKey: number;
							endorsementKey: number;
							endorsement: any;
							id: string;
							type: string;
							endorsementComment: string;
							additionalProperties: { [index: string]: any };
						};
						issuedOn: Date;
						issuer: any;
						revocationReason: string;
						revoked?: boolean;
						verification: any;
						additionalProperties: { [index: string]: any };
						endorsementKey: number;
						context: string;
						achievementKey?: number;
						achievement: {
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
								profileKey: number;
								endorsementProfile: any;
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
							};
							level: string;
							requirement: {
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
							achievementKey: number;
							achievementClrs: any[];
						};
						assertionKey?: number;
						assertion: {
							id: string;
							type: string;
							achievement: {
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
									profileKey: number;
									endorsementProfile: any;
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
								};
								level: string;
								requirement: {
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
								achievementKey: number;
								achievementClrs: any[];
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
								profileKey: number;
								endorsementProfile: any;
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
							};
							activityStartDate?: Date;
							term: string;
							verification: any;
							additionalProperties: { [index: string]: any };
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
						};
						profileKey?: number;
						profile: {
							profileKey: number;
							endorsementProfile: any;
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
						};
					};
					profileKey?: number;
					profile: {
						profileKey: number;
						endorsementProfile: any;
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
					};
				};
				additionalProperties: { [index: string]: any };
				endorsementProfileKey: number;
				profileKey?: number;
				profile: {
					profileKey: number;
					endorsementProfile: any;
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
					verification: {
						id: string;
						type: any;
						allowedOrigins: string[];
						creator: string;
						startsWith: string[];
						verificationProperty: string;
						additionalProperties: { [index: string]: any };
						verificationKey: number;
						assertionKey?: number;
						assertion: {
							id: string;
							type: string;
							achievement: {
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
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
								achievementKey: number;
								achievementClrs: any[];
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
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
						};
						clrKey?: number;
						clr: {
							clrKey: number;
							achievementClrs: any[];
							assertionClrs: any[];
							context: string;
							id: string;
							type: string;
							achievements: any[];
							assertions: any[];
							issuedOn: Date;
							learner: any;
							name: string;
							partial?: boolean;
							publisher: any;
							revocationReason: string;
							revoked?: boolean;
							signedAssertions: string[];
							signedEndorsements: string[];
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						endorsementKey?: number;
						endorsement: {
							id: string;
							type: string;
							claim: {
								endorsementClaimKey: number;
								endorsementKey: number;
								endorsement: any;
								id: string;
								type: string;
								endorsementComment: string;
								additionalProperties: { [index: string]: any };
							};
							issuedOn: Date;
							issuer: any;
							revocationReason: string;
							revoked?: boolean;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementKey: number;
							context: string;
							achievementKey?: number;
							achievement: {
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
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
								achievementKey: number;
								achievementClrs: any[];
							};
							assertionKey?: number;
							assertion: {
								id: string;
								type: string;
								achievement: {
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
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
									achievementKey: number;
									achievementClrs: any[];
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
								assertionKey: number;
								isSigned: boolean;
								context: string;
								assertionClrs: any[];
							};
							profileKey?: number;
							profile: any;
						};
						profileKey?: number;
						profile: any;
					};
					additionalProperties: { [index: string]: any };
				};
			};
			profileKey?: number;
			profile: {
				profileKey: number;
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
						id: string;
						type: any;
						allowedOrigins: string[];
						creator: string;
						startsWith: string[];
						verificationProperty: string;
						additionalProperties: { [index: string]: any };
						verificationKey: number;
						assertionKey?: number;
						assertion: {
							id: string;
							type: string;
							achievement: {
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
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
								achievementKey: number;
								achievementClrs: any[];
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
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
						};
						clrKey?: number;
						clr: {
							clrKey: number;
							achievementClrs: any[];
							assertionClrs: any[];
							context: string;
							id: string;
							type: string;
							achievements: any[];
							assertions: any[];
							issuedOn: Date;
							learner: any;
							name: string;
							partial?: boolean;
							publisher: any;
							revocationReason: string;
							revoked?: boolean;
							signedAssertions: string[];
							signedEndorsements: string[];
							verification: any;
							additionalProperties: { [index: string]: any };
						};
						endorsementKey?: number;
						endorsement: {
							id: string;
							type: string;
							claim: {
								endorsementClaimKey: number;
								endorsementKey: number;
								endorsement: any;
								id: string;
								type: string;
								endorsementComment: string;
								additionalProperties: { [index: string]: any };
							};
							issuedOn: Date;
							issuer: any;
							revocationReason: string;
							revoked?: boolean;
							verification: any;
							additionalProperties: { [index: string]: any };
							endorsementKey: number;
							context: string;
							achievementKey?: number;
							achievement: {
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
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
								achievementKey: number;
								achievementClrs: any[];
							};
							assertionKey?: number;
							assertion: {
								id: string;
								type: string;
								achievement: {
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
										id: string;
										type: string;
										narrative: string;
										additionalProperties: { [index: string]: any };
										criteriaKey: number;
										achievementKey?: number;
										achievement: any;
									};
									resultDescriptions: any[];
									signedEndorsements: string[];
									specialization: string;
									tags: string[];
									additionalProperties: { [index: string]: any };
									achievementKey: number;
									achievementClrs: any[];
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
								assertionKey: number;
								isSigned: boolean;
								context: string;
								assertionClrs: any[];
							};
							profileKey?: number;
							profile: any;
						};
						profileKey?: number;
						profile: any;
					};
					additionalProperties: { [index: string]: any };
					endorsementProfileKey: number;
					profileKey?: number;
					profile: any;
				};
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
				verification: {
					id: string;
					type: any;
					allowedOrigins: string[];
					creator: string;
					startsWith: string[];
					verificationProperty: string;
					additionalProperties: { [index: string]: any };
					verificationKey: number;
					assertionKey?: number;
					assertion: {
						id: string;
						type: string;
						achievement: {
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
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
							achievementKey: number;
							achievementClrs: any[];
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
						assertionKey: number;
						isSigned: boolean;
						context: string;
						assertionClrs: any[];
					};
					clrKey?: number;
					clr: {
						clrKey: number;
						achievementClrs: any[];
						assertionClrs: any[];
						context: string;
						id: string;
						type: string;
						achievements: any[];
						assertions: any[];
						issuedOn: Date;
						learner: any;
						name: string;
						partial?: boolean;
						publisher: any;
						revocationReason: string;
						revoked?: boolean;
						signedAssertions: string[];
						signedEndorsements: string[];
						verification: any;
						additionalProperties: { [index: string]: any };
					};
					endorsementKey?: number;
					endorsement: {
						id: string;
						type: string;
						claim: {
							endorsementClaimKey: number;
							endorsementKey: number;
							endorsement: any;
							id: string;
							type: string;
							endorsementComment: string;
							additionalProperties: { [index: string]: any };
						};
						issuedOn: Date;
						issuer: {
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
						revocationReason: string;
						revoked?: boolean;
						verification: any;
						additionalProperties: { [index: string]: any };
						endorsementKey: number;
						context: string;
						achievementKey?: number;
						achievement: {
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
								id: string;
								type: string;
								narrative: string;
								additionalProperties: { [index: string]: any };
								criteriaKey: number;
								achievementKey?: number;
								achievement: any;
							};
							resultDescriptions: any[];
							signedEndorsements: string[];
							specialization: string;
							tags: string[];
							additionalProperties: { [index: string]: any };
							achievementKey: number;
							achievementClrs: any[];
						};
						assertionKey?: number;
						assertion: {
							id: string;
							type: string;
							achievement: {
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
									id: string;
									type: string;
									narrative: string;
									additionalProperties: { [index: string]: any };
									criteriaKey: number;
									achievementKey?: number;
									achievement: any;
								};
								resultDescriptions: any[];
								signedEndorsements: string[];
								specialization: string;
								tags: string[];
								additionalProperties: { [index: string]: any };
								achievementKey: number;
								achievementClrs: any[];
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
							assertionKey: number;
							isSigned: boolean;
							context: string;
							assertionClrs: any[];
						};
						profileKey?: number;
						profile: any;
					};
					profileKey?: number;
					profile: any;
				};
				additionalProperties: { [index: string]: any };
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
		additionalProperties: { [index: string]: any };
	}
}