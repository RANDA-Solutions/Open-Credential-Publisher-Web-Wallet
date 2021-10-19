declare module server {
	interface pdfRequest {
		id: string;
		clrId?: number;
		assertionId: string;
		evidenceName: string;
		artifactId?: number;
		artifactName: string;
		createLink: boolean;
	}
}
