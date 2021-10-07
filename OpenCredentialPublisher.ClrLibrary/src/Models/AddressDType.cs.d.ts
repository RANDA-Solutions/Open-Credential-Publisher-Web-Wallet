declare module server {
	/** Based on schema.org Address object. */
	interface addressDType {
		/** Unique IRI for the Address. Model Primitive Datatype = NormalizedString. */
		id: string;
		/** The JSON-LD type of this object. Normally 'Address'. Model Primitive Datatype = NormalizedString. */
		type: string;
		/** The country. For example, USA. You can also provide the two-letter ISO 3166-1 alpha-2 country code. Model Primitive Datatype = String. */
		addressCountry: string;
		/** The locality. For example, Mountain View. Model Primitive Datatype = String. */
		addressLocality: string;
		/** The region. For example, CA. Model Primitive Datatype = String. */
		addressRegion: string;
		/** The postal code. For example, 94043. Model Primitive Datatype = String. */
		postalCode: string;
		/** The post office box number for PO box addresses. Model Primitive Datatype = String. */
		postOfficeBoxNumber: string;
		/** The street address. For example, 1600 Amphitheater Pkwy. Model Primitive Datatype = String. */
		streetAddress: string;
		additionalProperties: { [index: string]: any };
	}
}
