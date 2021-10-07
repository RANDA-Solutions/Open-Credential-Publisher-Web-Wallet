declare module server {
	/** Represents an ResultDescription entity in the CLR model. */
	interface resultDescriptionModel {
		/** Primary key. */
		resultDescriptionId: number;
		order: number;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		/** Unique IRI for the ResultDescription. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'ResultDescription'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** The ordered from 'low' to 'high' set of allowed values. Model Primitive Datatype = String. */
		allowedValues: string[];
		/** The name of the result. Model Primitive Datatype = String. */
		name: string;
		/** The id of the RubricCriterionLevel required to 'pass'. Model Primitive Datatype = NormalizedString. */
		requiredLevel: string;
		/** The value from allowedValues required to 'pass'. Model Primitive Datatype = String. */
		requiredValue: string;
		/** The type of result. This is an extensible enumerated vocabulary. */
		resultType: string;
		/** The maximum possible result that may be asserted. Model Primitive Datatype = String. */
		valueMax: string;
		/** The minimum possible result that may be asserted. Model Primitive Datatype = String. */
		valueMin: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
		/** End From ResultDescriptionDTypeForeignKeys */
		achievementId: number;
		/** Relationships */
		achievement: server.achievementModel;
		resultDescriptionAlignments: any[];
		rubricCriterionLevels: server.rubricCriterionLevelModel[];
	}
}
