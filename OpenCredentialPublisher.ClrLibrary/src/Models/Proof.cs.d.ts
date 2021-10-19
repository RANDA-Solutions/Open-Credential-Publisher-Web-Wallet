declare module server {
	interface proof {
		type: string;
		created: Date;
		proofPurpose: string;
		verificationMethod: string;
		signature: string;
		challenge: string;
		domain: string;
	}
}
