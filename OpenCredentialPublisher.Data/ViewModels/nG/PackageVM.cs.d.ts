declare module server {
	interface packageVM {
		id: number;
		typeId: server.packageTypeEnum;
		assertionCount: number;
		createdAt: Date;
		modifiedAt: Date;
		showDownloadPdfButton: boolean;
		showDownloadVCJsonButton: boolean;
		clrIds: number[];
		newestPdfTranscript: {
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
		};
	}
}
