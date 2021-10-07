declare module server {
	interface pdfShareViewModel {
		clrId: number;
		clrEvidenceName: string;
		clrName: string;
		clrIssuedOn: Date;
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
