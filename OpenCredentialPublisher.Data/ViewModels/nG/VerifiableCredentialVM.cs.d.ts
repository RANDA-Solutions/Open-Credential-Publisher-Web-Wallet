declare module server {
	interface verifiableCredentialVM {
		id: number;
		identifier: string;
		issuer: string;
		issuedOn: Date;
		credentialsCount: number;
		proof: {
			type: string;
			created: Date;
			proofPurpose: string;
			verificationMethod: string;
			signature: string;
			challenge: string;
			domain: string;
		};
		clrIds: number[];
	}
}
