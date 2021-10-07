declare module server {
	interface clrLinkVM {
		clrId: number;
		createdAt: Date;
		addedOn: Date;
		name: string;
		nickname: string;
		publisherName: string;
		sourceId?: number;
		sourceName: string;
	}
}
