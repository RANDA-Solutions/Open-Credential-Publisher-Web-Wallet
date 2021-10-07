declare module server {
	interface evidenceVM {
		evidenceId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		audience: string;
		description: string;
		genre: string;
		name: string;
		narrative: string;
		additionalProperties: { [index: string]: any };
		artifacts: any[];
	}
}
