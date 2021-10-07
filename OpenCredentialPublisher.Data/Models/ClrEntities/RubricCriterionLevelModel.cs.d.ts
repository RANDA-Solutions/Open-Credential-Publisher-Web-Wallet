declare module server {
	interface rubricCriterionLevelModel {
		/** Primary key. */
		rubricCriterionLevelId: number;
		order: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		/** Unique IRI for the ResultCriterionLevel. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'RubricCriterionLevel'. Model Primitive Datatype = String. */
		type: string;
		/** A description of the rubric criterion level. Model Primitive Datatype = String. */
		description: string;
		/** The rubric performance level in terms of success. Model Primitive Datatype = String. */
		level: string;
		/** The name of the RubricCriterionLevel. Model Primitive Datatype = String. */
		name: string;
		/** The number of grade points corresponding to a specific rubric level. Model Primitive Datatype = String. */
		points: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
		/** End From RubricCriterionLevelDTypeRelationships */
		resultDescriptionId: number;
		resultDescription: server.resultDescriptionModel;
		rubricCriterionLevelAlignments: any[];
	}
}
