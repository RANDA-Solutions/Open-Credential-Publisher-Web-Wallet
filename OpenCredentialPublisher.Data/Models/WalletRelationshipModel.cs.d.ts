declare module server {
	interface walletRelationshipModel {
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
		user: server.applicationUser;
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
	}
}
