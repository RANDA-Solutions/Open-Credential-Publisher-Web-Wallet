declare module server {
	interface criteriaVM {
		criteriaId: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		narrative: string;
		additionalProperties: { [index: string]: any };
	}
}
