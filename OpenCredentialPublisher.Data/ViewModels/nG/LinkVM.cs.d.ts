declare module server {
	interface linkVM {
		id: string;
		nickname: string;
		displayCount: number;
		clrId: number;
		clrPublisherName: string;
		clrIssuedOn: Date;
		url: string;
		packageCreatedAt: Date;
		pdfs: server.clrPdfVM[];
	}
}
