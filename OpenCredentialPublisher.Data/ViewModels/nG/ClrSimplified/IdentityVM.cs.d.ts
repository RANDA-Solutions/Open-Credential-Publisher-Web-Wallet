declare module server {
	interface identityVM {
		identityId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		identity: string;
		hashed: boolean;
		salt: string;
		additionalProperties: { [index: string]: any };
	}
}
