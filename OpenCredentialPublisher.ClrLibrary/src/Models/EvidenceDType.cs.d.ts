declare module server {
	/** One or more artifacts that represent supporting evidence for the record. Examples include text, media, websites, etc. */
	interface evidenceDType {
		/** The URI of a webpage presenting evidence of achievement. Model Primitive Datatype = AnyURI. */
		id: string;
		/** The JSON-LD type of this entity. Normally 'Evidence'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** Artifacts that are part of the evidence. */
		artifacts: server.artifactDType[];
		/** A description of the intended audience for a piece of evidence. Model Primitive Datatype = String. */
		audience: string;
		/** A longer description of the evidence. Model Primitive Datatype = String. */
		description: string;
		/** A string that describes the type of evidence. For example, Poetry, Prose, Film. Model Primitive Datatype = String. */
		genre: string;
		/** The name of the evidence. Model Primitive Datatype = String. */
		name: string;
		/** A narrative that describes the evidence and process of achievement that led to an assertion. Markdown allowed. Model Primitive Datatype = String. */
		narrative: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
