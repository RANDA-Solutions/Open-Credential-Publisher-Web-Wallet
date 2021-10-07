declare module server {
	interface verificationVM {
		verificationId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: any;
		allowedOrigins: string[];
		creator: string;
		startsWith: string[];
		verificationProperty: string;
		additionalProperties: { [index: string]: any };
	}
}
