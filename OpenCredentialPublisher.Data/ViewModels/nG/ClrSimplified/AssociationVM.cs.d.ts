declare module server {
	interface associationVM {
		associationId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		associationType: any;
		targetId: string;
		title: string;
		additionalProperties: { [index: string]: any };
	}
}
