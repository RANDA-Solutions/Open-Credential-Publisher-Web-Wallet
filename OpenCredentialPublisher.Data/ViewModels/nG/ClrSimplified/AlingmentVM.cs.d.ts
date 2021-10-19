declare module server {
	interface alignmentVM {
		alignmentId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		educationalFramework: string;
		targetCode: string;
		targetDescription: string;
		targetName: string;
		targetType: string;
		targetUrl: string;
		additionalProperties: { [index: string]: any };
	}
}
