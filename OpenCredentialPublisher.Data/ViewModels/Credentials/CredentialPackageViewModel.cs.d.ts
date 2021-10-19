declare module server {
	interface credentialPackageViewModel {
		credentialPackage: {
		/** Primary key */
			id: number;
			typeId: server.packageTypeEnum;
			revoked: boolean;
			revocationReason: string;
		/** Foreign key back to the authorization. */
			authorizationForeignKey: string;
			authorization: server.authorizationModel;
			clr: server.clrModel;
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
					clrs: server.clrModel[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				clrs: server.clrModel[];
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
				clrs: server.clrModel[];
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
			containedClrs: server.clrModel[];
		};
		clrVM: server.clrViewModel;
		clrSetVM: {
			clrSet: {
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
					clr: server.clrModel;
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
						clrs: server.clrModel[];
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
					containedClrs: server.clrModel[];
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
						clr: server.clrModel;
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
						containedClrs: server.clrModel[];
					};
					clrSets: any[];
					clrs: server.clrModel[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				clrs: server.clrModel[];
				isDeleted: boolean;
				createdAt: Date;
				modifiedAt: Date;
			};
			clrVMs: server.clrViewModel[];
			pdfs: any[];
			hasPdfs: boolean;
			assertionsCount: number;
		};
		verifiableCredentialVM: {
			credentialPackageVM: server.credentialPackageViewModel;
			clrSetVMs: any[];
			clrVMs: server.clrViewModel[];
			allClrs: server.clrViewModel[];
			verifiableCredential: {
				contexts: string[];
				id: string;
				types: string[];
				issuer: string;
				issuanceDate: Date;
				credentialSubjects: any[];
				credentialStatus: {
					id: string;
					type: string;
				};
				proof: {
					type: string;
					created: Date;
					proofPurpose: string;
					verificationMethod: string;
					signature: string;
					challenge: string;
					domain: string;
				};
			};
			assertionsCount: number;
			pdfs: any[];
			hasPdfs: boolean;
		};
		assertionsCount: number;
		backpack: {
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
			parentCredentialPackage: {
		/** Primary key */
				id: number;
				typeId: server.packageTypeEnum;
				revoked: boolean;
				revocationReason: string;
		/** Foreign key back to the authorization. */
				authorizationForeignKey: string;
				authorization: server.authorizationModel;
				clr: server.clrModel;
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
						clrs: server.clrModel[];
						isDeleted: boolean;
						createdAt: Date;
						modifiedAt: Date;
					};
					clrs: server.clrModel[];
					isDeleted: boolean;
					createdAt: Date;
					modifiedAt: Date;
				};
				badgrBackpack: any;
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
					clrs: server.clrModel[];
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
				containedClrs: server.clrModel[];
			};
			badgrAssertions: any[];
			isDeleted: boolean;
			createdAt: Date;
			modifiedAt: Date;
		};
		pdfs: any[];
		hasPdfs: boolean;
	}
}
