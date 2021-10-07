declare module server {
	interface clrPdfVM {
		assertionId: string;
		artifactId: number;
		evidenceName: string;
		artifactName: string;
		artifactUrl: string;
		isUrl: boolean;
		isPdf: boolean;
		mediaType: string;
	}
}
