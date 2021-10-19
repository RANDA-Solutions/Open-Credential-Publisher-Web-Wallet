declare module server {
	interface resultVM {
		resultId: number;
		order: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		achievedLevel: string;
		resultDescription: string;
		status: string;
		value: string;
		additionalProperties: { [index: string]: any };
		/** * End From ResultDType********************************************************************************************ForeignKeys */
		assertionId: number;
	}
}
