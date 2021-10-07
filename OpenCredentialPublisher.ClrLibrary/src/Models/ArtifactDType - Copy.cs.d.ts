declare module server {
	/** An artifact that is part of an evidence object. */
	interface artifactDType {
		/** The JSON-LD type of the object. Normally 'Artifact'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** A description of the artifact. Model Primitive Datatype = String. */
		description: string;
		/** The name of the artifact. Model Primitive Datatype = String. */
		name: string;
		/** IRI of the artifact. May be a Data URI or the URL where the artifact may be found. Model Primitive Datatype = AnyURI. */
		url: string;
		/** Additional properties of the object */
		additionalProperties: { [index: string]: any };
	}
}
