declare module server {
	/** Descriptive metadata about the achievements necessary to be recognized with an Assertion of a particular AchievementType. This data is added to the AchievementType so that it may be rendered when that AchievementType is displayed, instead of simply a link to human-readable criteria external to the Achievement Assertion. Embedding criteria allows either enhancement of an external criteria page or increased portability and ease of use by allowing issuers to skip hosting the formerly-required external criteria page altogether. */
	interface criteriaDType {
		/** The URI of a webpage that describes the criteria for the Achievement in a human-readable format. Model Primitive Datatype = AnyURI. */
		id: string;
		/** The JSON-LD type of this object. Normally 'Criteria'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** A narrative of what is needed to earn the achievement. Markdown allowed. Model Primitive Datatype = String. */
		narrative: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
