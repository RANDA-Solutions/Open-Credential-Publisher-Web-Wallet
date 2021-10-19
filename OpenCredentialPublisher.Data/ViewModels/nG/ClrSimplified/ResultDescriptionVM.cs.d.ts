declare module server {
	interface resultDescriptionVM {
		resultDescriptionId: number;
		order: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		allowedValues: string[];
		name: string;
		requiredLevel: string;
		requiredValue: string;
		resultType: string;
		valueMax: string;
		valueMin: string;
		additionalProperties: { [index: string]: any };
		achievementId: number;
		alignments: server.alignmentModel[];
		rubricCriterionLevels: server.rubricCriterionLevelModel[];
	}
}
