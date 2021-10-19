declare module server {
	interface profileModel {
		hasProfileImage: boolean;
		profileImageUrl: string;
		displayName: string;
		missingDisplayName: boolean;
		credentials: number;
		achievements: number;
		scores: number;
		activeLinks: number;
		additionalData: { [index: string]: string };
	}
	interface dashboardModel {
		showShareableLinksSection: boolean;
		showLatestShareableLink: boolean;
		latestShareableLink: {
		/** The primary key. */
			id: string;
			clrForeignKey: number;
		/** The CLR this link points to. */
			clr: {
		/** START Actual persisted dataForeign Keys */
				credentialPackageId?: number;
				verifiableCredentialId?: number;
				clrSetId?: number;
		/** Foreign key back to the authorization. */
				authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
				assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
				authorization: {
		/** Access token. */
					accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
					authorizationCode: string;
		/** All the CLRs tied to this authorization. */
					clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
					codeVerifier: string;
					endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
					id: string;
					method: string;
					payload: string;
		/** Refresh token (if issued by authorization server). */
					refreshToken: string;
		/** Scopes this user has permission to use. */
					scopes: string[];
		/** Resource server these credentials work with. */
					source: {
		/** All the authorizations tied to this resource server. */
						authorizations: any[];
		/** OAuth 2.0 client identifier string. */
						clientId: string;
		/** OAuth 2.0 client secret string. */
						clientSecret: string;
		/** The discovery document from the resource server. */
						discoveryDocument: {
							key: number;
							source: any;
							sourceForeignKey: number;
		/** BadgeConnect additional properties */
							id: string;
							apiBase: string;
							tokenRevocationUrl: string;
							version: string;
						};
		/** Primary key. */
						id: number;
		/** The name of the resource server (also in the Discovery Document). */
						name: string;
		/** All the revocations tied to this resource server. */
						revocations: any[];
		/** The scopes the authorization server and resource server support. */
						scope: string;
		/** The base URL for the resource server. */
						url: string;
		/** BitMap of entitytypes the source provides */
						sourceTypeId: any;
						isDeletable: boolean;
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
		/** Foreign key back to the resource server. */
					sourceForeignKey: number;
					userId: string;
					user: {
						displayName: string;
						profileImageUrl: string;
						addresses: any[];
						emails: any[];
						phoneNumbers: any[];
						paymentRequests: any[];
						credits: any[];
					};
					validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
					credentialPackages: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
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
				links: any[];
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
		/** END Actual persisted datapossible Parents */
				credentialPackage: {
		/** Primary key */
					id: number;
					typeId: any;
					revoked: boolean;
					revocationReason: string;
		/** Foreign key back to the authorization. */
					authorizationForeignKey: string;
					authorization: {
		/** Access token. */
						accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
						authorizationCode: string;
		/** All the CLRs tied to this authorization. */
						clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
						codeVerifier: string;
						endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
						id: string;
						method: string;
						payload: string;
		/** Refresh token (if issued by authorization server). */
						refreshToken: string;
		/** Scopes this user has permission to use. */
						scopes: string[];
		/** Resource server these credentials work with. */
						source: {
		/** All the authorizations tied to this resource server. */
							authorizations: any[];
		/** OAuth 2.0 client identifier string. */
							clientId: string;
		/** OAuth 2.0 client secret string. */
							clientSecret: string;
		/** The discovery document from the resource server. */
							discoveryDocument: {
								key: number;
								source: any;
								sourceForeignKey: number;
		/** BadgeConnect additional properties */
								id: string;
								apiBase: string;
								tokenRevocationUrl: string;
								version: string;
							};
		/** Primary key. */
							id: number;
		/** The name of the resource server (also in the Discovery Document). */
							name: string;
		/** All the revocations tied to this resource server. */
							revocations: any[];
		/** The scopes the authorization server and resource server support. */
							scope: string;
		/** The base URL for the resource server. */
							url: string;
		/** BitMap of entitytypes the source provides */
							sourceTypeId: any;
							isDeletable: boolean;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
		/** Foreign key back to the resource server. */
						sourceForeignKey: number;
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
						credentialPackages: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clr: any;
					clrSet: {
						id: number;
						credentialPackageId?: number;
						verifiableCredentialId?: number;
						clrsCount: number;
						identifier: string;
						json: string;
						credentialPackage: any;
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
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
						credentialPackageId: number;
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
						credentialPackage: any;
						badgrAssertions: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					verifiableCredential: {
						credentialPackageId: number;
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
						credentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					name: string;
		/** This Package is tied to a specific application user. */
					userId: string;
					user: {
						displayName: string;
						profileImageUrl: string;
						addresses: any[];
						emails: any[];
						phoneNumbers: any[];
						paymentRequests: any[];
						credits: any[];
					};
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				verifiableCredential: {
					credentialPackageId: number;
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
					credentialPackage: {
		/** Primary key */
						id: number;
						typeId: any;
						revoked: boolean;
						revocationReason: string;
		/** Foreign key back to the authorization. */
						authorizationForeignKey: string;
						authorization: {
		/** Access token. */
							accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
							authorizationCode: string;
		/** All the CLRs tied to this authorization. */
							clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
							codeVerifier: string;
							endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
							id: string;
							method: string;
							payload: string;
		/** Refresh token (if issued by authorization server). */
							refreshToken: string;
		/** Scopes this user has permission to use. */
							scopes: string[];
		/** Resource server these credentials work with. */
							source: {
		/** All the authorizations tied to this resource server. */
								authorizations: any[];
		/** OAuth 2.0 client identifier string. */
								clientId: string;
		/** OAuth 2.0 client secret string. */
								clientSecret: string;
		/** The discovery document from the resource server. */
								discoveryDocument: {
									key: number;
									source: any;
									sourceForeignKey: number;
		/** BadgeConnect additional properties */
									id: string;
									apiBase: string;
									tokenRevocationUrl: string;
									version: string;
								};
		/** Primary key. */
								id: number;
		/** The name of the resource server (also in the Discovery Document). */
								name: string;
		/** All the revocations tied to this resource server. */
								revocations: any[];
		/** The scopes the authorization server and resource server support. */
								scope: string;
		/** The base URL for the resource server. */
								url: string;
		/** BitMap of entitytypes the source provides */
								sourceTypeId: any;
								isDeletable: boolean;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
		/** Foreign key back to the resource server. */
							sourceForeignKey: number;
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
							credentialPackages: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						clr: any;
						clrSet: {
							id: number;
							credentialPackageId?: number;
							verifiableCredentialId?: number;
							clrsCount: number;
							identifier: string;
							json: string;
							credentialPackage: any;
							verifiableCredential: any;
							clrs: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						badgrBackpack: {
							credentialPackageId: number;
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
							credentialPackage: any;
							badgrAssertions: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						verifiableCredential: any;
						name: string;
		/** This Package is tied to a specific application user. */
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clrSets: any[];
					clrs: any[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				clrSet: {
					id: number;
					credentialPackageId?: number;
					verifiableCredentialId?: number;
					clrsCount: number;
					identifier: string;
					json: string;
					credentialPackage: {
		/** Primary key */
						id: number;
						typeId: any;
						revoked: boolean;
						revocationReason: string;
		/** Foreign key back to the authorization. */
						authorizationForeignKey: string;
						authorization: {
		/** Access token. */
							accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
							authorizationCode: string;
		/** All the CLRs tied to this authorization. */
							clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
							codeVerifier: string;
							endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
							id: string;
							method: string;
							payload: string;
		/** Refresh token (if issued by authorization server). */
							refreshToken: string;
		/** Scopes this user has permission to use. */
							scopes: string[];
		/** Resource server these credentials work with. */
							source: {
		/** All the authorizations tied to this resource server. */
								authorizations: any[];
		/** OAuth 2.0 client identifier string. */
								clientId: string;
		/** OAuth 2.0 client secret string. */
								clientSecret: string;
		/** The discovery document from the resource server. */
								discoveryDocument: {
									key: number;
									source: any;
									sourceForeignKey: number;
		/** BadgeConnect additional properties */
									id: string;
									apiBase: string;
									tokenRevocationUrl: string;
									version: string;
								};
		/** Primary key. */
								id: number;
		/** The name of the resource server (also in the Discovery Document). */
								name: string;
		/** All the revocations tied to this resource server. */
								revocations: any[];
		/** The scopes the authorization server and resource server support. */
								scope: string;
		/** The base URL for the resource server. */
								url: string;
		/** BitMap of entitytypes the source provides */
								sourceTypeId: any;
								isDeletable: boolean;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
		/** Foreign key back to the resource server. */
							sourceForeignKey: number;
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
							credentialPackages: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						clr: any;
						clrSet: any;
						badgrBackpack: {
							credentialPackageId: number;
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
							credentialPackage: any;
							badgrAssertions: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
							clrSets: any[];
							clrs: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						name: string;
		/** This Package is tied to a specific application user. */
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					verifiableCredential: {
						credentialPackageId: number;
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
						credentialPackage: {
		/** Primary key */
							id: number;
							typeId: any;
							revoked: boolean;
							revocationReason: string;
		/** Foreign key back to the authorization. */
							authorizationForeignKey: string;
							authorization: {
		/** Access token. */
								accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
								authorizationCode: string;
		/** All the CLRs tied to this authorization. */
								clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
								codeVerifier: string;
								endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
								id: string;
								method: string;
								payload: string;
		/** Refresh token (if issued by authorization server). */
								refreshToken: string;
		/** Scopes this user has permission to use. */
								scopes: string[];
		/** Resource server these credentials work with. */
								source: {
		/** All the authorizations tied to this resource server. */
									authorizations: any[];
		/** OAuth 2.0 client identifier string. */
									clientId: string;
		/** OAuth 2.0 client secret string. */
									clientSecret: string;
		/** The discovery document from the resource server. */
									discoveryDocument: {
										key: number;
										source: any;
										sourceForeignKey: number;
		/** BadgeConnect additional properties */
										id: string;
										apiBase: string;
										tokenRevocationUrl: string;
										version: string;
									};
		/** Primary key. */
									id: number;
		/** The name of the resource server (also in the Discovery Document). */
									name: string;
		/** All the revocations tied to this resource server. */
									revocations: any[];
		/** The scopes the authorization server and resource server support. */
									scope: string;
		/** The base URL for the resource server. */
									url: string;
		/** BitMap of entitytypes the source provides */
									sourceTypeId: any;
									isDeletable: boolean;
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
								};
		/** Foreign key back to the resource server. */
								sourceForeignKey: number;
								userId: string;
								user: {
									displayName: string;
									profileImageUrl: string;
									addresses: any[];
									emails: any[];
									phoneNumbers: any[];
									paymentRequests: any[];
									credits: any[];
								};
								validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
								credentialPackages: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							clr: any;
							clrSet: any;
							badgrBackpack: {
								credentialPackageId: number;
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
								credentialPackage: any;
								badgrAssertions: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							verifiableCredential: any;
							name: string;
		/** This Package is tied to a specific application user. */
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
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
		/** The number of times this link has been used to display the CLR. */
			displayCount: number;
		/** A nickname for the link to help remember who it was shared with. */
			nickname: string;
			requiresAccessKey: boolean;
		/** Application user */
			userId: string;
			user: {
				displayName: string;
				profileImageUrl: string;
				addresses: any[];
				emails: any[];
				phoneNumbers: any[];
				paymentRequests: any[];
				credits: any[];
			};
			credentialRequestId?: number;
			credentialRequest: {
				id: number;
				walletRelationshipId: number;
				threadId: string;
		/** Application user */
				userId: string;
				credentialRequestStep: any;
				errorMessage: string;
				credentialPackageId: number;
				credentialDefinitionId?: number;
				credentialSchemaId?: number;
				createdOn: Date;
				modifiedOn?: Date;
				walletRelationship: {
					id: number;
					walletName: string;
					relationshipDid: string;
					relationshipVerKey: string;
					inviteUrl: string;
					isConnected: boolean;
					agentContextId: any;
		/** Application user */
					userId: string;
					createdOn: Date;
					modifiedOn?: Date;
					user: {
						displayName: string;
						profileImageUrl: string;
						addresses: any[];
						emails: any[];
						phoneNumbers: any[];
						paymentRequests: any[];
						credits: any[];
					};
					agentContext: {
						id: any;
						agentName: string;
						apiKey: string;
						contextJson: string;
						domainDid: string;
						endpointUrl: string;
						issuerDid: string;
						issuerRegistered: boolean;
						issuerVerKey: string;
						network: string;
						sdkVerKey: string;
						sdkVerKeyId: string;
						tokenHash: string;
						threadId: string;
						verityAgentVerKey: string;
						verityPublicDid: string;
						verityPublicVerKey: string;
						verityUrl: string;
						version: string;
						walletKey: string;
						walletPath: string;
						provisioningTokenId?: number;
						active: boolean;
						createdOn: Date;
						modifiedOn?: Date;
						provisioningToken: {
							id: number;
							sponseeId: string;
							sponsorId: string;
							nonce: string;
							timestamp: string;
							sig: string;
							sponsorVerKey: string;
							createdOn: Date;
							modifiedOn?: Date;
							agentContextId: any;
							agentContext: any;
						};
					};
					credentialRequests: any[];
					credentialsSent: number;
				};
				agentContext: {
					id: any;
					agentName: string;
					apiKey: string;
					contextJson: string;
					domainDid: string;
					endpointUrl: string;
					issuerDid: string;
					issuerRegistered: boolean;
					issuerVerKey: string;
					network: string;
					sdkVerKey: string;
					sdkVerKeyId: string;
					tokenHash: string;
					threadId: string;
					verityAgentVerKey: string;
					verityPublicDid: string;
					verityPublicVerKey: string;
					verityUrl: string;
					version: string;
					walletKey: string;
					walletPath: string;
					provisioningTokenId?: number;
					active: boolean;
					createdOn: Date;
					modifiedOn?: Date;
					provisioningToken: {
						id: number;
						sponseeId: string;
						sponsorId: string;
						nonce: string;
						timestamp: string;
						sig: string;
						sponsorVerKey: string;
						createdOn: Date;
						modifiedOn?: Date;
						agentContextId: any;
						agentContext: any;
					};
				};
				user: {
					displayName: string;
					profileImageUrl: string;
					addresses: any[];
					emails: any[];
					phoneNumbers: any[];
					paymentRequests: any[];
					credits: any[];
				};
				credentialPackage: {
		/** Primary key */
					id: number;
					typeId: any;
					revoked: boolean;
					revocationReason: string;
		/** Foreign key back to the authorization. */
					authorizationForeignKey: string;
					authorization: {
		/** Access token. */
						accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
						authorizationCode: string;
		/** All the CLRs tied to this authorization. */
						clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
						codeVerifier: string;
						endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
						id: string;
						method: string;
						payload: string;
		/** Refresh token (if issued by authorization server). */
						refreshToken: string;
		/** Scopes this user has permission to use. */
						scopes: string[];
		/** Resource server these credentials work with. */
						source: {
		/** All the authorizations tied to this resource server. */
							authorizations: any[];
		/** OAuth 2.0 client identifier string. */
							clientId: string;
		/** OAuth 2.0 client secret string. */
							clientSecret: string;
		/** The discovery document from the resource server. */
							discoveryDocument: {
								key: number;
								source: any;
								sourceForeignKey: number;
		/** BadgeConnect additional properties */
								id: string;
								apiBase: string;
								tokenRevocationUrl: string;
								version: string;
							};
		/** Primary key. */
							id: number;
		/** The name of the resource server (also in the Discovery Document). */
							name: string;
		/** All the revocations tied to this resource server. */
							revocations: any[];
		/** The scopes the authorization server and resource server support. */
							scope: string;
		/** The base URL for the resource server. */
							url: string;
		/** BitMap of entitytypes the source provides */
							sourceTypeId: any;
							isDeletable: boolean;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
		/** Foreign key back to the resource server. */
						sourceForeignKey: number;
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
						credentialPackages: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clr: {
		/** START Actual persisted dataForeign Keys */
						credentialPackageId?: number;
						verifiableCredentialId?: number;
						clrSetId?: number;
		/** Foreign key back to the authorization. */
						authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
						assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
						authorization: {
		/** Access token. */
							accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
							authorizationCode: string;
		/** All the CLRs tied to this authorization. */
							clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
							codeVerifier: string;
							endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
							id: string;
							method: string;
							payload: string;
		/** Refresh token (if issued by authorization server). */
							refreshToken: string;
		/** Scopes this user has permission to use. */
							scopes: string[];
		/** Resource server these credentials work with. */
							source: {
		/** All the authorizations tied to this resource server. */
								authorizations: any[];
		/** OAuth 2.0 client identifier string. */
								clientId: string;
		/** OAuth 2.0 client secret string. */
								clientSecret: string;
		/** The discovery document from the resource server. */
								discoveryDocument: {
									key: number;
									source: any;
									sourceForeignKey: number;
		/** BadgeConnect additional properties */
									id: string;
									apiBase: string;
									tokenRevocationUrl: string;
									version: string;
								};
		/** Primary key. */
								id: number;
		/** The name of the resource server (also in the Discovery Document). */
								name: string;
		/** All the revocations tied to this resource server. */
								revocations: any[];
		/** The scopes the authorization server and resource server support. */
								scope: string;
		/** The base URL for the resource server. */
								url: string;
		/** BitMap of entitytypes the source provides */
								sourceTypeId: any;
								isDeletable: boolean;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
		/** Foreign key back to the resource server. */
							sourceForeignKey: number;
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
							credentialPackages: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
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
						links: any[];
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
		/** END Actual persisted datapossible Parents */
						credentialPackage: any;
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
							clrSets: any[];
							clrs: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						clrSet: {
							id: number;
							credentialPackageId?: number;
							verifiableCredentialId?: number;
							clrsCount: number;
							identifier: string;
							json: string;
							credentialPackage: any;
							verifiableCredential: {
								credentialPackageId: number;
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
								credentialPackage: any;
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
						credentialPackageId?: number;
						verifiableCredentialId?: number;
						clrsCount: number;
						identifier: string;
						json: string;
						credentialPackage: any;
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
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
						credentialPackageId: number;
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
						credentialPackage: any;
						badgrAssertions: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					verifiableCredential: {
						credentialPackageId: number;
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
						credentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					name: string;
		/** This Package is tied to a specific application user. */
					userId: string;
					user: {
						displayName: string;
						profileImageUrl: string;
						addresses: any[];
						emails: any[];
						phoneNumbers: any[];
						paymentRequests: any[];
						credits: any[];
					};
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				credentialSchema: {
					id: number;
					schemaId: string;
					threadId: string;
					typeName: string;
					name: string;
					version: string;
					attributes: string;
					hash: string;
					statusId: any;
					createdOn: Date;
					modifiedOn?: Date;
					credentialDefinitions: any[];
				};
				credentialDefinition: {
					id: number;
					agentContextId: any;
					credentialSchemaId: number;
					threadId: string;
					credentialDefinitionId: string;
					name: string;
					tag: string;
					statusId: any;
					createdOn: Date;
					modifiedOn?: Date;
					credentialSchema: {
						id: number;
						schemaId: string;
						threadId: string;
						typeName: string;
						name: string;
						version: string;
						attributes: string;
						hash: string;
						statusId: any;
						createdOn: Date;
						modifiedOn?: Date;
						credentialDefinitions: any[];
					};
					agentContext: {
						id: any;
						agentName: string;
						apiKey: string;
						contextJson: string;
						domainDid: string;
						endpointUrl: string;
						issuerDid: string;
						issuerRegistered: boolean;
						issuerVerKey: string;
						network: string;
						sdkVerKey: string;
						sdkVerKeyId: string;
						tokenHash: string;
						threadId: string;
						verityAgentVerKey: string;
						verityPublicDid: string;
						verityPublicVerKey: string;
						verityUrl: string;
						version: string;
						walletKey: string;
						walletPath: string;
						provisioningTokenId?: number;
						active: boolean;
						createdOn: Date;
						modifiedOn?: Date;
						provisioningToken: {
							id: number;
							sponseeId: string;
							sponsorId: string;
							nonce: string;
							timestamp: string;
							sig: string;
							sponsorVerKey: string;
							createdOn: Date;
							modifiedOn?: Date;
							agentContextId: any;
							agentContext: any;
						};
					};
				};
			};
			shares: any[];
			createdAt: Date;
			modifiedAt?: Date;
		};
		showNewestPdfTranscript: boolean;
		newestPdfTranscript: {
			clrVM: {
				clr: {
		/** START Actual persisted dataForeign Keys */
					credentialPackageId?: number;
					verifiableCredentialId?: number;
					clrSetId?: number;
		/** Foreign key back to the authorization. */
					authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
					assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
					authorization: {
		/** Access token. */
						accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
						authorizationCode: string;
		/** All the CLRs tied to this authorization. */
						clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
						codeVerifier: string;
						endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
						id: string;
						method: string;
						payload: string;
		/** Refresh token (if issued by authorization server). */
						refreshToken: string;
		/** Scopes this user has permission to use. */
						scopes: string[];
		/** Resource server these credentials work with. */
						source: {
		/** All the authorizations tied to this resource server. */
							authorizations: any[];
		/** OAuth 2.0 client identifier string. */
							clientId: string;
		/** OAuth 2.0 client secret string. */
							clientSecret: string;
		/** The discovery document from the resource server. */
							discoveryDocument: {
								key: number;
								source: any;
								sourceForeignKey: number;
		/** BadgeConnect additional properties */
								id: string;
								apiBase: string;
								tokenRevocationUrl: string;
								version: string;
							};
		/** Primary key. */
							id: number;
		/** The name of the resource server (also in the Discovery Document). */
							name: string;
		/** All the revocations tied to this resource server. */
							revocations: any[];
		/** The scopes the authorization server and resource server support. */
							scope: string;
		/** The base URL for the resource server. */
							url: string;
		/** BitMap of entitytypes the source provides */
							sourceTypeId: any;
							isDeletable: boolean;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
		/** Foreign key back to the resource server. */
						sourceForeignKey: number;
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
						credentialPackages: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
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
					links: any[];
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
		/** END Actual persisted datapossible Parents */
					credentialPackage: {
		/** Primary key */
						id: number;
						typeId: any;
						revoked: boolean;
						revocationReason: string;
		/** Foreign key back to the authorization. */
						authorizationForeignKey: string;
						authorization: {
		/** Access token. */
							accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
							authorizationCode: string;
		/** All the CLRs tied to this authorization. */
							clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
							codeVerifier: string;
							endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
							id: string;
							method: string;
							payload: string;
		/** Refresh token (if issued by authorization server). */
							refreshToken: string;
		/** Scopes this user has permission to use. */
							scopes: string[];
		/** Resource server these credentials work with. */
							source: {
		/** All the authorizations tied to this resource server. */
								authorizations: any[];
		/** OAuth 2.0 client identifier string. */
								clientId: string;
		/** OAuth 2.0 client secret string. */
								clientSecret: string;
		/** The discovery document from the resource server. */
								discoveryDocument: {
									key: number;
									source: any;
									sourceForeignKey: number;
		/** BadgeConnect additional properties */
									id: string;
									apiBase: string;
									tokenRevocationUrl: string;
									version: string;
								};
		/** Primary key. */
								id: number;
		/** The name of the resource server (also in the Discovery Document). */
								name: string;
		/** All the revocations tied to this resource server. */
								revocations: any[];
		/** The scopes the authorization server and resource server support. */
								scope: string;
		/** The base URL for the resource server. */
								url: string;
		/** BitMap of entitytypes the source provides */
								sourceTypeId: any;
								isDeletable: boolean;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
		/** Foreign key back to the resource server. */
							sourceForeignKey: number;
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
							credentialPackages: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						clr: any;
						clrSet: {
							id: number;
							credentialPackageId?: number;
							verifiableCredentialId?: number;
							clrsCount: number;
							identifier: string;
							json: string;
							credentialPackage: any;
							verifiableCredential: {
								credentialPackageId: number;
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
								credentialPackage: any;
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
							credentialPackageId: number;
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
							credentialPackage: any;
							badgrAssertions: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
							clrSets: any[];
							clrs: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						name: string;
		/** This Package is tied to a specific application user. */
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					verifiableCredential: {
						credentialPackageId: number;
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
						credentialPackage: {
		/** Primary key */
							id: number;
							typeId: any;
							revoked: boolean;
							revocationReason: string;
		/** Foreign key back to the authorization. */
							authorizationForeignKey: string;
							authorization: {
		/** Access token. */
								accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
								authorizationCode: string;
		/** All the CLRs tied to this authorization. */
								clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
								codeVerifier: string;
								endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
								id: string;
								method: string;
								payload: string;
		/** Refresh token (if issued by authorization server). */
								refreshToken: string;
		/** Scopes this user has permission to use. */
								scopes: string[];
		/** Resource server these credentials work with. */
								source: {
		/** All the authorizations tied to this resource server. */
									authorizations: any[];
		/** OAuth 2.0 client identifier string. */
									clientId: string;
		/** OAuth 2.0 client secret string. */
									clientSecret: string;
		/** The discovery document from the resource server. */
									discoveryDocument: {
										key: number;
										source: any;
										sourceForeignKey: number;
		/** BadgeConnect additional properties */
										id: string;
										apiBase: string;
										tokenRevocationUrl: string;
										version: string;
									};
		/** Primary key. */
									id: number;
		/** The name of the resource server (also in the Discovery Document). */
									name: string;
		/** All the revocations tied to this resource server. */
									revocations: any[];
		/** The scopes the authorization server and resource server support. */
									scope: string;
		/** The base URL for the resource server. */
									url: string;
		/** BitMap of entitytypes the source provides */
									sourceTypeId: any;
									isDeletable: boolean;
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
								};
		/** Foreign key back to the resource server. */
								sourceForeignKey: number;
								userId: string;
								user: {
									displayName: string;
									profileImageUrl: string;
									addresses: any[];
									emails: any[];
									phoneNumbers: any[];
									paymentRequests: any[];
									credits: any[];
								};
								validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
								credentialPackages: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							clr: any;
							clrSet: {
								id: number;
								credentialPackageId?: number;
								verifiableCredentialId?: number;
								clrsCount: number;
								identifier: string;
								json: string;
								credentialPackage: any;
								verifiableCredential: any;
								clrs: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							badgrBackpack: {
								credentialPackageId: number;
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
								credentialPackage: any;
								badgrAssertions: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							verifiableCredential: any;
							name: string;
		/** This Package is tied to a specific application user. */
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clrSet: {
						id: number;
						credentialPackageId?: number;
						verifiableCredentialId?: number;
						clrsCount: number;
						identifier: string;
						json: string;
						credentialPackage: {
		/** Primary key */
							id: number;
							typeId: any;
							revoked: boolean;
							revocationReason: string;
		/** Foreign key back to the authorization. */
							authorizationForeignKey: string;
							authorization: {
		/** Access token. */
								accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
								authorizationCode: string;
		/** All the CLRs tied to this authorization. */
								clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
								codeVerifier: string;
								endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
								id: string;
								method: string;
								payload: string;
		/** Refresh token (if issued by authorization server). */
								refreshToken: string;
		/** Scopes this user has permission to use. */
								scopes: string[];
		/** Resource server these credentials work with. */
								source: {
		/** All the authorizations tied to this resource server. */
									authorizations: any[];
		/** OAuth 2.0 client identifier string. */
									clientId: string;
		/** OAuth 2.0 client secret string. */
									clientSecret: string;
		/** The discovery document from the resource server. */
									discoveryDocument: {
										key: number;
										source: any;
										sourceForeignKey: number;
		/** BadgeConnect additional properties */
										id: string;
										apiBase: string;
										tokenRevocationUrl: string;
										version: string;
									};
		/** Primary key. */
									id: number;
		/** The name of the resource server (also in the Discovery Document). */
									name: string;
		/** All the revocations tied to this resource server. */
									revocations: any[];
		/** The scopes the authorization server and resource server support. */
									scope: string;
		/** The base URL for the resource server. */
									url: string;
		/** BitMap of entitytypes the source provides */
									sourceTypeId: any;
									isDeletable: boolean;
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
								};
		/** Foreign key back to the resource server. */
								sourceForeignKey: number;
								userId: string;
								user: {
									displayName: string;
									profileImageUrl: string;
									addresses: any[];
									emails: any[];
									phoneNumbers: any[];
									paymentRequests: any[];
									credits: any[];
								};
								validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
								credentialPackages: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							clr: any;
							clrSet: any;
							badgrBackpack: {
								credentialPackageId: number;
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
								credentialPackage: any;
								badgrAssertions: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							verifiableCredential: {
								credentialPackageId: number;
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
								credentialPackage: any;
								clrSets: any[];
								clrs: any[];
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
							name: string;
		/** This Package is tied to a specific application user. */
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: {
		/** Primary key */
								id: number;
								typeId: any;
								revoked: boolean;
								revocationReason: string;
		/** Foreign key back to the authorization. */
								authorizationForeignKey: string;
								authorization: {
		/** Access token. */
									accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
									authorizationCode: string;
		/** All the CLRs tied to this authorization. */
									clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
									codeVerifier: string;
									endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
									id: string;
									method: string;
									payload: string;
		/** Refresh token (if issued by authorization server). */
									refreshToken: string;
		/** Scopes this user has permission to use. */
									scopes: string[];
		/** Resource server these credentials work with. */
									source: {
		/** All the authorizations tied to this resource server. */
										authorizations: any[];
		/** OAuth 2.0 client identifier string. */
										clientId: string;
		/** OAuth 2.0 client secret string. */
										clientSecret: string;
		/** The discovery document from the resource server. */
										discoveryDocument: {
											key: number;
											source: any;
											sourceForeignKey: number;
		/** BadgeConnect additional properties */
											id: string;
											apiBase: string;
											tokenRevocationUrl: string;
											version: string;
										};
		/** Primary key. */
										id: number;
		/** The name of the resource server (also in the Discovery Document). */
										name: string;
		/** All the revocations tied to this resource server. */
										revocations: any[];
		/** The scopes the authorization server and resource server support. */
										scope: string;
		/** The base URL for the resource server. */
										url: string;
		/** BitMap of entitytypes the source provides */
										sourceTypeId: any;
										isDeletable: boolean;
										isDeleted: boolean;
										createdAt: Date;
										modifiedAt: Date;
									};
		/** Foreign key back to the resource server. */
									sourceForeignKey: number;
									userId: string;
									user: {
										displayName: string;
										profileImageUrl: string;
										addresses: any[];
										emails: any[];
										phoneNumbers: any[];
										paymentRequests: any[];
										credits: any[];
									};
									validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
									credentialPackages: any[];
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
								};
								clr: any;
								clrSet: any;
								badgrBackpack: {
									credentialPackageId: number;
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
									credentialPackage: any;
									badgrAssertions: any[];
									isDeleted: boolean;
									createdAt: Date;
									modifiedAt: Date;
								};
								verifiableCredential: any;
								name: string;
		/** This Package is tied to a specific application user. */
								userId: string;
								user: {
									displayName: string;
									profileImageUrl: string;
									addresses: any[];
									emails: any[];
									phoneNumbers: any[];
									paymentRequests: any[];
									credits: any[];
								};
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
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
					typeId: any;
					revoked: boolean;
					revocationReason: string;
		/** Foreign key back to the authorization. */
					authorizationForeignKey: string;
					authorization: {
		/** Access token. */
						accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
						authorizationCode: string;
		/** All the CLRs tied to this authorization. */
						clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
						codeVerifier: string;
						endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
						id: string;
						method: string;
						payload: string;
		/** Refresh token (if issued by authorization server). */
						refreshToken: string;
		/** Scopes this user has permission to use. */
						scopes: string[];
		/** Resource server these credentials work with. */
						source: {
		/** All the authorizations tied to this resource server. */
							authorizations: any[];
		/** OAuth 2.0 client identifier string. */
							clientId: string;
		/** OAuth 2.0 client secret string. */
							clientSecret: string;
		/** The discovery document from the resource server. */
							discoveryDocument: {
								key: number;
								source: any;
								sourceForeignKey: number;
		/** BadgeConnect additional properties */
								id: string;
								apiBase: string;
								tokenRevocationUrl: string;
								version: string;
							};
		/** Primary key. */
							id: number;
		/** The name of the resource server (also in the Discovery Document). */
							name: string;
		/** All the revocations tied to this resource server. */
							revocations: any[];
		/** The scopes the authorization server and resource server support. */
							scope: string;
		/** The base URL for the resource server. */
							url: string;
		/** BitMap of entitytypes the source provides */
							sourceTypeId: any;
							isDeletable: boolean;
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
		/** Foreign key back to the resource server. */
						sourceForeignKey: number;
						userId: string;
						user: {
							displayName: string;
							profileImageUrl: string;
							addresses: any[];
							emails: any[];
							phoneNumbers: any[];
							paymentRequests: any[];
							credits: any[];
						};
						validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
						credentialPackages: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clr: {
		/** START Actual persisted dataForeign Keys */
						credentialPackageId?: number;
						verifiableCredentialId?: number;
						clrSetId?: number;
		/** Foreign key back to the authorization. */
						authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
						assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
						authorization: {
		/** Access token. */
							accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
							authorizationCode: string;
		/** All the CLRs tied to this authorization. */
							clrs: any[];
		/** PKCE code verifier. Only has a value during ACG flow. */
							codeVerifier: string;
							endpoint: string;
		/** Primary key. GUID to obfuscate when it is used as the state in ACG flow. */
							id: string;
							method: string;
							payload: string;
		/** Refresh token (if issued by authorization server). */
							refreshToken: string;
		/** Scopes this user has permission to use. */
							scopes: string[];
		/** Resource server these credentials work with. */
							source: {
		/** All the authorizations tied to this resource server. */
								authorizations: any[];
		/** OAuth 2.0 client identifier string. */
								clientId: string;
		/** OAuth 2.0 client secret string. */
								clientSecret: string;
		/** The discovery document from the resource server. */
								discoveryDocument: {
									key: number;
									source: any;
									sourceForeignKey: number;
		/** BadgeConnect additional properties */
									id: string;
									apiBase: string;
									tokenRevocationUrl: string;
									version: string;
								};
		/** Primary key. */
								id: number;
		/** The name of the resource server (also in the Discovery Document). */
								name: string;
		/** All the revocations tied to this resource server. */
								revocations: any[];
		/** The scopes the authorization server and resource server support. */
								scope: string;
		/** The base URL for the resource server. */
								url: string;
		/** BitMap of entitytypes the source provides */
								sourceTypeId: any;
								isDeletable: boolean;
								isDeleted: boolean;
								createdAt: Date;
								modifiedAt: Date;
							};
		/** Foreign key back to the resource server. */
							sourceForeignKey: number;
							userId: string;
							user: {
								displayName: string;
								profileImageUrl: string;
								addresses: any[];
								emails: any[];
								phoneNumbers: any[];
								paymentRequests: any[];
								credits: any[];
							};
							validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
							credentialPackages: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
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
						links: any[];
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
		/** END Actual persisted datapossible Parents */
						credentialPackage: any;
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
							clrSets: any[];
							clrs: any[];
							isDeleted: boolean;
							createdAt: Date;
							modifiedAt: Date;
						};
						clrSet: {
							id: number;
							credentialPackageId?: number;
							verifiableCredentialId?: number;
							clrsCount: number;
							identifier: string;
							json: string;
							credentialPackage: any;
							verifiableCredential: {
								credentialPackageId: number;
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
								credentialPackage: any;
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
						credentialPackageId?: number;
						verifiableCredentialId?: number;
						clrsCount: number;
						identifier: string;
						json: string;
						credentialPackage: any;
						verifiableCredential: {
							credentialPackageId: number;
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
							credentialPackage: any;
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
						credentialPackageId: number;
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
						credentialPackage: any;
						badgrAssertions: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					verifiableCredential: {
						credentialPackageId: number;
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
						credentialPackage: any;
						clrSets: any[];
						clrs: any[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					name: string;
		/** This Package is tied to a specific application user. */
					userId: string;
					user: {
						displayName: string;
						profileImageUrl: string;
						addresses: any[];
						emails: any[];
						phoneNumbers: any[];
						paymentRequests: any[];
						credits: any[];
					};
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				allAssertions: any[];
				parentAssertions: any[];
				pdfs: any[];
			};
			clrId: number;
			assertionId: string;
			artifactId: number;
			evidenceName: string;
			artifactName: string;
			artifactUrl: string;
			isUrl: boolean;
			isPdf: boolean;
			mediaType: string;
		};
	}
}
