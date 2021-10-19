declare module server {
	/** Represents a Shareable link to a CLR. */
	interface linkModel {
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
	}
}
