declare module server {
	interface endorsementVM {
		signedEndorsement: string;
		endorsementId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		isSigned: boolean;
		id: string;
		type: string;
		issuedOn: Date;
		revocationReason: string;
		revoked?: boolean;
		additionalProperties: { [index: string]: any };
		endorsementClaim: server.endorsementClaimVM;
		issuer: server.profileVM;
		verification: server.verificationVM;
	}
}
