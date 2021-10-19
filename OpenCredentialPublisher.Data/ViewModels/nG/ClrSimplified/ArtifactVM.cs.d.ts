declare module server {
	interface artifactVM {
		artifactId: number;
		isPdf: boolean;
		isUrl: boolean;
		mediaType: string;
		nameContainsTranscript: boolean;
		/** EnhancedArtifactFields */
		clrId?: number;
		assertionId: string;
		clrIssuedOn?: Date;
		clrName: string;
		evidenceName: string;
		/** IBaseEntity properties */
		isDeleted: boolean;
		createdAt: Date;
		modifiedAt: Date;
		type: string;
		description: string;
		name: string;
		url: string;
		additionalProperties: { [index: string]: any };
	}
}
