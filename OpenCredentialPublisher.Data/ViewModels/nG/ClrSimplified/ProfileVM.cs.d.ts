declare module server {
	interface profileVM {
		profileId: number;
		isEndorsementProfile: boolean;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		id: string;
		type: string;
		additionalName: string;
		birthDate: Date;
		description: string;
		email: string;
		familyName: string;
		givenName: string;
		identifiers: any[];
		image: string;
		name: string;
		official: string;
		publicKey: {
			context: string;
			id: string;
			type: string;
			owner: string;
			publicKeyPem: string;
			additionalProperties: { [index: string]: any };
		};
		revocationList: string;
		sourcedId: string;
		studentId: string;
		telephone: string;
		url: string;
		address: {
			id: string;
			type: string;
			addressCountry: string;
			addressLocality: string;
			addressRegion: string;
			postalCode: string;
			postOfficeBoxNumber: string;
			streetAddress: string;
			additionalProperties: { [index: string]: any };
		};
		verification: server.verificationVM;
	}
}
