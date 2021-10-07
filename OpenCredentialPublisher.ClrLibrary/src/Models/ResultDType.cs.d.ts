declare module server {
	/** Describes a result of an achievement. */
	interface resultDType {
		/** Unique IRI for the object. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'Result'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** The id of the RubricCriterionLevel achieved. Model Primitive Datatype = NormalizedString. */
		achievedLevel: string;
		/** The alignments between this result and nodes in external frameworks. This set of alignments are in addition to the set of alignments defined in the corresponding ResultDescription object. */
		alignments: any[];
		/** The id of the ResultDescription describing this result. Model Primitive Datatype = NormalizedString. */
		resultDescription: string;
		status: string;
		/** A grade or value representing the result of the performance, or demonstration, of the achievement.  For example, 'A' if the recipient received a grade of A in the class.  Model Primitive Datatype = String. */
		value: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
