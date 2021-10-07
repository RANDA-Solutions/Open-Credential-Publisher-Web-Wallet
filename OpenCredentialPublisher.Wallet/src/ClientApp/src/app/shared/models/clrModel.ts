	/** Represents a CLR for an application user. The complete CLR is stored as JSON. */
	export class ClrModel {
		/** START Actual persisted dataForeign Keys */
		parentCredentialPackageId?: number;
		parentVerifiableCredentialId?: number;
		parentClrSetId?: number;
		/** Foreign key back to the authorization. */
		authorizationForeignKey: string;
		/** Number of assertions in this CLR. */
		assertionsCount: number;
		/** The resource server authorization that was used to get this CLR. */
		authorization: any;
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
		credentialPackageId: number;
		assertions: any[];
		/** true indicates this CLR's Id was received in a revocation list from the source */
		isRevoked: boolean;
	}
