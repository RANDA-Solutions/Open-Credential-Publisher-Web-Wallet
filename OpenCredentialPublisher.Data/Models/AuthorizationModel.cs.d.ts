declare module server {
	/** OAuth 2.0 data for an application user and resource server. */
	interface authorizationModel {
		/** Access token. */
		accessToken: string;
		/** Authorization code. Only has a value during ACG flow. */
		authorizationCode: string;
		/** All the CLRs tied to this authorization. */
		clrs: server.clrModel[];
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
			authorizations: server.authorizationModel[];
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
		user: server.applicationUser;
		validTo: Date;
		/** All the CredentialPackages tied to this authorization. */
		credentialPackages: any[];
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
	}
}
